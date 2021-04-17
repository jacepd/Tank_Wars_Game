using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using NetworkUtil;
using Newtonsoft.Json;

namespace TankWars
{
    public class ServerController
    {
        private World theWorld;        
        private int worldSize = 500;
        private int numPlayers; // The number of players who have ever connected to this server

        // Maps all client connections to their playerID
        private Dictionary<SocketState, int> clients; 

        // Maps inputs sent by clients to the playerID of the client that sent them
        private Dictionary<ControlCommand, int> clientInputs; 

        public delegate void ServerStartupHandler(string message);
        public event ServerStartupHandler ServerStartupEvent;

        public delegate void NewClientConnectedHandler(int playerID, string playerName);
        public event NewClientConnectedHandler NewClentConnectedEvent;

        public delegate void ErrorHandler(string message);
        public event ErrorHandler ErrorEvent;

        /// <summary>
        /// Creates a new server controller
        /// </summary>
        public ServerController()
        {
            theWorld = new World(worldSize);
            clients = new Dictionary<SocketState, int>();
            numPlayers = 0;

            clientInputs = new Dictionary<ControlCommand, int>();
        }

        /// <summary>
        /// Returns the world
        /// </summary>
        /// <returns></returns>
        public World getWorld()
        {
            return theWorld;
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void StartServer()
        {
            Networking.StartServer(NewClientConnected, 11000);

            ServerStartupEvent("Server is running.  Accepting clients");
        }

        /// <summary>
        /// Creates new thread to handle client
        /// </summary>
        /// <param name="state"></param>
        private void NewClientConnected(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                ErrorEvent("Issue with connection");
            }

            state.OnNetworkAction = HandleClient;

            Networking.GetData(state);
        }

        /// <summary>
        /// Completes handshake with client bby sending playerID, world size, and walls
        /// </summary>
        /// <param name="state"></param>
        private void HandleClient(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                ErrorEvent("Issue with connection");
            }

            string firstData = state.GetData();
            string[] parts = Regex.Split(firstData, @"(?<=[\n])");
            string playerName;

            // Check to make sure first message is complete
            if (parts[0].Contains("\n"))
            {
                // Parse and store the player ID
                playerName = parts[0].Substring(0, parts[0].Length - 1);

                lock (theWorld)
                {
                    NewClentConnectedEvent(numPlayers, playerName);

                    // Save the client state
                    // Need to lock here because clients can disconnect at any time
                    clients.Add(state, numPlayers);

                    Vector2D tankLocation = generateRandomLocation();
                    Tank tank = new Tank(numPlayers, playerName, tankLocation);
                    theWorld.addTank(tank);

                    Networking.Send(state.TheSocket, numPlayers + "\n");
                    Networking.Send(state.TheSocket, worldSize + "\n");

                    SendWalls(state);

                    numPlayers++;
                }

                // Remove the ID from the message data
                state.RemoveData(0, parts[0].Length);

                // Continue the event loop to recieve input data from clients
                state.OnNetworkAction = ReceiveCommands;
                Networking.GetData(state);
            }
        }

        /// <summary>
        /// Returns the location where a tank should be spawned
        /// </summary>
        /// <returns></returns>
        private Vector2D generateRandomLocation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives input data from client
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveCommands(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                ErrorEvent("Issue receiving data");
            }

            string inputData = state.GetData();
            string[] inputs = Regex.Split(inputData, @"(?<=[\n])");

            int lastItemLength = 0; // The length of the last item in the message buffer

            lock(clientInputs)
            {
                foreach (string item in inputs)
                {
                    // Skip empty strings (idk if we really need this here)
                    if (item.Length == 0)
                        continue;
                    // Last message is incomplete, stop parsing
                    if (item[item.Length - 1] != '\n')
                    {
                        lastItemLength = item.Length;
                        break;
                    }

                    ControlCommand newInput = JsonConvert.DeserializeObject<ControlCommand>(item);
                    clientInputs.Add(newInput, clients[state]);
                }
            }

            // Continue event loop
            state.RemoveData(0, inputs.Length - lastItemLength);
            Networking.GetData(state);
        }

        /// <summary>
        /// Sends the initial walls to a client
        /// </summary>
        /// <param name="state"></param>
        private void SendWalls(SocketState state)
        {
            foreach (Wall wall in theWorld.getWalls())
            {
                string json = JsonConvert.SerializeObject(wall);

                Networking.Send(state.TheSocket, json + "\n");
            }
        }

        /// <summary>
        /// Sends the current state of the world to all clients
        /// </summary>
        public void SendWorld()
        {
            lock (theWorld)
            {
                // Contains beams to be removed after this frame
                List<Beam> beamsToRemove = new List<Beam>();

                foreach(SocketState client in clients.Keys)
                {
                    // Send tanks
                    foreach(Tank tank in theWorld.getTanks())
                    {
                        string json = JsonConvert.SerializeObject(tank);
                        Networking.Send(client.TheSocket, json + "\n");
                    }

                    // Send projectiles
                    foreach(Projectile proj in theWorld.getProjectiles())
                    {
                        string json = JsonConvert.SerializeObject(proj);
                        Networking.Send(client.TheSocket, json + "\n");
                    }

                    // Send beams
                    foreach(Beam beam in theWorld.getBeams())
                    {
                        string json = JsonConvert.SerializeObject(beam);
                        Networking.Send(client.TheSocket, json + "\n");
                        beamsToRemove.Add(beam);
                    }

                    // Send powerups
                    foreach(Powerup power in theWorld.getPowerups())
                    {
                        string json = JsonConvert.SerializeObject(power);
                        Networking.Send(client.TheSocket, json + "\n");
                    }
                }

                // Remove beams once they've been sent
                foreach (Beam beam in beamsToRemove)
                {
                    theWorld.removeBeam(beam);
                }
            }
        }

        /// <summary>
        /// Updates the locations of objects in the world
        /// </summary>
        public void updateWorld()
        {
            lock (theWorld)
            {
                lock(clientInputs)
                {
                    foreach (ControlCommand input in clientInputs.Keys)
                    {
                        // Tank movement logic
                        int playerID = clientInputs[input];
                        Tank tank = theWorld.getTank(playerID);
                        tank.updateTank(input);

                        // Projectile logic
                    }
                }
            }
        }

        /// <summary>
        /// Checks all clients in the clients list to see if one has disconnected
        /// </summary>
        public void CheckForDisconnected()
        {
            // Locks the world because its using clients
            lock (theWorld)
            {
                foreach(SocketState client in clients.Keys)
                {
                    if (client.ErrorOccurred)
                    {
                        ErrorEvent("Client " + clients[client] + " disconnected");
                    }
                }
            }
        }
        
    }
}
