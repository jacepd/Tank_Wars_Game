using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using NetworkUtil;

namespace TankWars
{
    public class ServerController
    {
        World theWorld;
        Dictionary<int, SocketState> clients;
        int worldSize = 500;
        int numPlayers;

        public delegate void ServerStartupHandler(string message);
        public event ServerStartupHandler ServerStartupEvent;

        public delegate void NewClientConnectedHandler(int playerID, string playerName);
        public event NewClientConnectedHandler NewClentConnectedEvent;

        public delegate void ErrorHandler(string message);
        public event ErrorHandler ErrorEvent;

        public ServerController()
        {
            theWorld = new World(worldSize);
            clients = new Dictionary<int, SocketState>();
            numPlayers = 0;
        }

        public void StartServer()
        {
            Networking.StartServer(NewClientConnected, 11000);

            ServerStartupEvent("Server is running.  Accepting clients");
        }

        private void NewClientConnected(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                ErrorEvent("Issue with connection");
            }

            state.OnNetworkAction = HandleClient;

            Networking.GetData(state);
        }

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
                    clients.Add(numPlayers, state);

                    Tank tank = new Tank(numPlayers, playerName);
                    theWorld.addTank(tank);

                    Networking.Send(state.TheSocket, numPlayers + "\n");
                    Networking.Send(state.TheSocket, worldSize + "\n");

                    SendWalls(state);

                    numPlayers++;
                }

                // Remove the ID from the message data
                state.RemoveData(0, parts[0].Length);

                // Parse World size
                // state.OnNetworkAction = ReceiveCommands;
            }
        }

        private void SendWalls(SocketState state)
        {
            foreach (Wall wall in theWorld.getWalls())
            {
                // JSon.Serialize(wall);
            }
        }
    }
}
