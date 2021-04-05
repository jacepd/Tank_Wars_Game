// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Text.RegularExpressions;
using NetworkUtil;
using World;
using Newtonsoft.Json;

namespace GameController
{
    public class Controller
    {
        private string playerName;  // The name of the player in the game
        private int playerID;       // The unique ID of the player in the game
        private int worldSize;      // The height and width of the world
        private WorldBox world;     // The world containing all drawable objects in the game

        public delegate void ServerUpdateHandler();
        public event ServerUpdateHandler UpdateArrived;

        /// <summary>
        /// Creates a new Controller to run the game
        /// </summary>
        public Controller()
        {
            playerID = -1;
            world = null;
        }

        /// <summary>
        /// Connect to the given server using the given player name
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="playerName"></param>
        public void ConnectToServer(string serverName, string playerName)
        {
            this.playerName = playerName;
            Networking.ConnectToServer(OnFirstServerConnect, serverName, 11000);
        }

        /// <summary>
        /// Sends the player name once connected to the server to establish 'handshake'
        /// </summary>
        /// <param name="state"></param>
        private void OnFirstServerConnect(SocketState state)
        {
            if(state.ErrorOccurred)
            {
                // Show Error Message?
            }
            Networking.Send(state.TheSocket, playerName + "\n");

            state.OnNetworkAction = ReceivePlayerID;

            Networking.GetData(state);
        }

        /// <summary>
        /// Receives and stores the player ID resulting from a successful 'handshake' with
        /// the server.
        /// </summary>
        /// <param name="state"></param>
        private void ReceivePlayerID(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                // Show Error Message?
            }

            string firstData = state.GetData();
            string[] parts = Regex.Split(firstData, @"(?<=[\n])");

            // Check to make sure first message is complete
            if (parts[0].Contains("\n"))
            {
                // Parse and store the player ID
                playerID = Int32.Parse(parts[0].Substring(0, parts[0].Length - 1));

                // Remove the ID from the message data
                state.RemoveData(0, parts[0].Length);

                // Parse World size
                state.OnNetworkAction = ReceiveWorldSize;
                Networking.GetData(state);
            }
            else
            {
                // First message incomplete, request more data
                Networking.GetData(state);
            }

            
        }

        /// <summary>
        /// Receives and stores the world size resulting from a successful 'handshake' with
        /// the server
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveWorldSize(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                // Show Error Message?
            }

            string firstData = state.GetData();
            string[] parts = Regex.Split(firstData, @"(?<=[\n])");

            // Check to make sure message is complete
            if (parts[0].Contains("\n"))
            {
                // Parse world size and create new World
                worldSize = Int32.Parse(parts[0].Substring(0, parts[0].Length - 1));

                world = new WorldBox(worldSize);

                // Remove the ID from the message data
                state.RemoveData(0, parts[0].Length);

                // Parse Walls
                state.OnNetworkAction = ReceiveWalls;
                Networking.GetData(state);
            }
            else
            {
                // First message incomplete, request more data
                Networking.GetData(state);
            }
        }

        /// <summary>
        /// Receives and stores the set of walls given when first
        /// connecting to the server.
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveWalls(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                // Show Error Message?
            }

            while (false) // While next JSON object is a wall
            {
                // Parse wall
                // Create wall object
                // Add wall to world using addWall() method
            }
            // Loop finished: all walls are in wall container

            // Draw all abjects that need to be drawn
            // (AKA. all objects in wall container + all objects in other containers?)

            /* Change GetData's OnNetworkAction to its final 'RecieveUpdate' method that
             * is capable of parsing and drawing any JSON objects that are sent to it
             */
        }

        private void ReceiveUpdate(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                // Show Error Message?
            }

            string data = state.GetData();
            string[] newItems = Regex.Split(data, @"(?<=[\n])");

            int lastItemLength = 0;

            /*/////////////////////Add data to World////////////////////////////*/
            lock (world)
            {
                foreach (string item in newItems)
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
                        
                    Object obj = JsonConvert.DeserializeObject(item);

                    string type = ""; // TODO: Find a way to figure this out

                    switch (type)
                    {
                        case "tank":
                            if (false /*dead*/)
                            {
                                // world.RemoveTank()obj
                            }
                            else
                            {
                                world.addTank((Tank)obj);
                            }
                            break;
                        case "powerup":
                            world.addPowerup((Powerup)obj);
                            break;

                            // TODO: etc. for all types

                    }

                }                
            }
            /*///////////////////////Data has been added to World/////////////////////////*/

            // Remove updte data from message buffer
            state.RemoveData(0, data.Length - lastItemLength);

            // TODO: Notify View to draw new data (using event?)

            // Continue the event loop
            Networking.GetData(state);
        }
    }
}
