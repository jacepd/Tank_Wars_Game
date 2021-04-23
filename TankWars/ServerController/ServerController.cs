// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

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
        private int worldSize;
        private int numPlayers; // The number of players who have connected to this server since it started

        private int powerupDelay; // Number of frames that should pass before a powerup is created
        private int framesSinceLastPowerup;

        // Maps all client connections to their playerID
        private Dictionary<SocketState, int> clientToID;

        // Maps all playerIDs to their client connection
        private Dictionary<int, SocketState> IDToClient;

        // Maps inputs sent by clients to the playerID of the client that sent them
        private Dictionary<int, ControlCommand> clientInputs; 

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
            worldSize = 2000;
            theWorld = new World(worldSize);
            clientToID = new Dictionary<SocketState, int>();
            IDToClient = new Dictionary<int, SocketState>();
            numPlayers = 0;
            powerupDelay = Constants.maxPowerupDelay;
            framesSinceLastPowerup = 0;

            clientInputs = new Dictionary<int, ControlCommand>();
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
                return;
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
                ErrorEvent("Issue handling client");
                return;
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
                    clientToID.Add(state, numPlayers);
                    IDToClient.Add(numPlayers, state);

                    Vector2D tankLocation = theWorld.generateRandomLocation(30);
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
        /// Sends the initial walls to a client
        /// </summary>
        /// <param name="state"></param>
        private void SendWalls(SocketState state)
        {
            foreach (Wall wall in theWorld.getWalls().Values)
            {
                string json = JsonConvert.SerializeObject(wall);

                Networking.Send(state.TheSocket, json + "\n");
            }
        }

        /// <summary>
        /// Receives input data from client
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveCommands(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                ErrorEvent("Client " + clientToID[state] + " disconnected");

                // Notify clients that tank has disconnected
                int playerID = clientToID[state];
                Tank tank = theWorld.getTanks()[playerID];
                tank.setDisconnected();
                return;
            }

            string inputData = state.GetData();
            string[] inputs = Regex.Split(inputData, @"(?<=[\n])");

            int lastItemLength = 0; // The length of the last item in the message buffer

            lock (clientInputs)
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

                    // Prevents multiple inputs in one frame
                    if (!clientInputs.ContainsKey(clientToID[state]))
                    {
                        clientInputs.Add(clientToID[state], newInput);
                    }
                }
            }

            // Continue event loop
            state.RemoveData(0, inputData.Length);
            Networking.GetData(state);
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

                // Contains powerups to be removed
                List<Powerup> powerUpsToRemove = new List<Powerup>();

                // Contains disconnected tanks to be removed
                List<Tank> tanksToRemove = new List<Tank>();

                foreach(SocketState client in clientToID.Keys)
                {
                    // Send tanks
                    foreach(Tank tank in theWorld.getTanks().Values)
                    {
                        if (tank.getDisconnected())
                        {
                            tanksToRemove.Add(tank);
                        }
                        string json = JsonConvert.SerializeObject(tank);
                        Networking.Send(client.TheSocket, json + "\n");
                    }

                    // Send projectiles
                    foreach(Projectile proj in theWorld.getProjectiles().Values)
                    {
                        string json = JsonConvert.SerializeObject(proj);
                        Networking.Send(client.TheSocket, json + "\n");
                    }

                    // Send beams
                    foreach(Beam beam in theWorld.getBeams().Values)
                    {
                        string json = JsonConvert.SerializeObject(beam);
                        Networking.Send(client.TheSocket, json + "\n");
                        beamsToRemove.Add(beam);
                    }

                    // Send powerups
                    foreach(Powerup power in theWorld.getPowerups().Values)
                    {
                        string json = JsonConvert.SerializeObject(power);
                        Networking.Send(client.TheSocket, json + "\n");

                        if (power.getDied())
                        {
                            powerUpsToRemove.Add(power);
                        }
                        
                    }
                }

                // Remove beams once they've been sent
                foreach(Beam beam in beamsToRemove)
                {
                    theWorld.removeBeam(beam);
                }

                // Remove collected powerups from world
                foreach (Powerup power in powerUpsToRemove)
                {
                    theWorld.removePowerup(power);
                }

                // Remove disconnected Tanks
                foreach (Tank tank in tanksToRemove)
                {
                    // Remove client associated with this tank
                    SocketState client = IDToClient[tank.getID()];
                    clientToID.Remove(client);
                    IDToClient.Remove(tank.getID());

                    theWorld.removeTank(tank);                    
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
                    foreach (ControlCommand input in clientInputs.Values)
                    {
                        int playerID = 0;
                        foreach(int index in clientInputs.Keys)
                        {
                            if (clientInputs[index].Equals(input))
                            {
                                playerID = index;
                            }
                        }
                        // Update tanks
                        Tank tank = theWorld.getTanks()[playerID];
                        Vector2D prevLocation = tank.getLocation();
                        tank.updateTank(input);

                        // Check for tank collisions
                        if (theWorld.collidesWithTankOrWall(out object collidedWith, tank.getLocation(), 30))
                        {
                            if (collidedWith.GetType().Equals(typeof(Wall)))
                            {
                                tank.setLocation(prevLocation);
                            }
                        }

                        // Create new projectile
                        if (input.getFire().Equals("main"))
                        {                 
                            if (tank.getAbleToFire())
                            {
                                // Arguments
                                int projID = theWorld.getNumProjectileCreated();
                                Vector2D projLocation = tank.getLocation();
                                Vector2D projDirection = input.getTurretDirection();
                                int projOwner = tank.getID();

                                // Create projectile
                                Projectile proj = new Projectile(projID, projLocation, projDirection, false, projOwner);
                                theWorld.addProjectile(proj);

                                // Reset Cooldown
                                tank.startFireCooldown();
                            }
                            
                        }

                        // Create beams
                        else if(input.getFire().Equals("alt"))
                        {
                            if (tank.getAbleToFireBeam())
                            {
                                // Arguments
                                int beamID = theWorld.getNumBeamsCreated();
                                Vector2D beamLocation = tank.getLocation();
                                Vector2D beamDirection = input.getTurretDirection();
                                int beamOwner = tank.getID();

                                Beam beam = new Beam(beamID, beamLocation, beamDirection, beamOwner);
                                theWorld.addBeam(beam);

                                tank.setBeamNotFireable();
                            }                           
                        }
                    }

                    // Remove inputs after they've been processed
                    clientInputs.Clear();

                    // Update existing projectile location
                    theWorld.updateProjectileLocations();

                    // Check beam collisions
                    foreach (Beam beam in theWorld.getBeams().Values)
                    {
                        foreach(Tank tank in theWorld.getTanks().Values)
                        {
                            Vector2D beamOrigin = beam.getOrigin();
                            Vector2D beamDir = beam.getDirection();
                            Vector2D tankLocation = tank.getLocation();
                            double radius = 30;

                            if (Intersects(beamOrigin, beamDir, tankLocation, radius))
                            {
                                tank.kill();
                                Tank ownerTank = theWorld.getTanks()[beam.getOwnerID()];
                                ownerTank.incrementScore();
                            }
                        }
                    }

                    // Update tanks if dead or on cooldown
                    foreach (Tank tank in theWorld.getTanks().Values)
                    {
                        // Respawn dead tanks
                        if (tank.getDied())
                        {
                            if (tank.getFramesSinceDied() >= Constants.respawnRate)
                            {
                                tank.resetDeath();
                                tank.setLocation(theWorld.generateRandomLocation(30));
                            }
                            else
                            {
                                tank.incrementFramesSinceDead();
                            }
                        }

                        // Advance the cooldown timer
                        tank.incrementFramesSinceFired();
                    }

                    // Check powerup collisions
                    foreach(Powerup power in theWorld.getPowerups().Values)
                    {
                        if (theWorld.collidesWithTankOrWall(out object collidedWith, power.getLocation(), 20))
                        {
                            if (!power.getDied() && collidedWith.GetType().Equals(typeof(Tank)))
                            {
                                Tank collidedTank = (Tank)collidedWith;
                                collidedTank.setBeamFireable();
                                power.setDead();

                                if (Constants.mode.Equals("Power"))
                                {
                                    collidedTank.restoreHealth();
                                }
                            }
                        }
                    }

                    // Create powerups
                    if(framesSinceLastPowerup > powerupDelay && theWorld.getPowerups().Count < Constants.maxPowerups)
                    {
                        Powerup powerup = new Powerup(theWorld.getNumPowerupsCreated(), theWorld.generateRandomLocation(0), false);
                        theWorld.addPowerup(powerup);

                        // Reset frame counter and randomize delay
                        framesSinceLastPowerup = 0;
                        Random rand = new Random();
                        powerupDelay = rand.Next(Constants.maxPowerupDelay);
                    }

                }
            }
            framesSinceLastPowerup++;
        }

        /// <summary>
        /// Determines if a ray interescts a circle
        /// </summary>
        /// <param name="rayOrig">The origin of the ray</param>
        /// <param name="rayDir">The direction of the ray</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="r">The radius of the circle</param>
        /// <returns></returns>
        private static bool Intersects(Vector2D rayOrig, Vector2D rayDir, Vector2D center, double r)
        {
            // ray-circle intersection test
            // P: hit point
            // ray: P = O + tV
            // circle: (P-C)dot(P-C)-r^2 = 0
            // substituting to solve for t gives a quadratic equation:
            // a = VdotV
            // b = 2(O-C)dotV
            // c = (O-C)dot(O-C)-r^2
            // if the discriminant is negative, miss (no solution for P)
            // otherwise, if both roots are positive, hit

            double a = rayDir.Dot(rayDir);
            double b = ((rayOrig - center) * 2.0).Dot(rayDir);
            double c = (rayOrig - center).Dot(rayOrig - center) - r * r;

            // discriminant
            double disc = b * b - 4.0 * a * c;

            if (disc < 0.0)
                return false;

            // find the signs of the roots
            // technically we should also divide by 2a
            // but all we care about is the sign, not the magnitude
            double root1 = -b + Math.Sqrt(disc);
            double root2 = -b - Math.Sqrt(disc);

            return (root1 > 0.0 && root2 > 0.0);
        }

    }
}
