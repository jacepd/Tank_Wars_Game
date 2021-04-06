﻿// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Text.RegularExpressions;
using NetworkUtil;
using TankWars;
using Newtonsoft.Json;

namespace TankWars
{
    public class Controller
    {
        private string playerName;  // The name of the player in the game
        private int playerID;       // The unique ID of the player in the game
        private int worldSize = 500;      // The height and width of the world
        private World world;     // The world containing all drawable objects in the game

        public delegate void ServerUpdateHandler();
        public event ServerUpdateHandler UpdateArrived; // event to be called after new data has been received

        /// <summary>
        /// Creates a new Controller to run the game
        /// </summary>
        public Controller()
        {
            playerID = -1;
            world = new World(500);
        }

        /// <summary>
        /// Returns the world owned by this controller
        /// </summary>
        /// <returns></returns>
        public World getWorld()
        {
            return world;
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

                world = new World(worldSize);

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

            string data = state.GetData();
            string[] parts = Regex.Split(data, @"(?<=[\n])");

            lock (world)
            {
                foreach (string item in parts)
                {
                    // Skip empty strings (idk if we really need this here)
                    if (item.Length == 0)
                        continue;
                    // Last message is incomplete, stop parsing
                    if (item[item.Length - 1] != '\n')
                    {
                        break;
                    }

                    // The last of the walls have been added
                    if (!item.Contains("wall"))
                    {                        
                        // Change GetData's OnNetworkAction to its final 'RecieveUpdate' method that
                        // is capable of parsing and drawing any JSON objects that are sent to it
                        state.OnNetworkAction = ReceiveUpdate;                      
                        break;
                    }

                    // Add the wall to the world
                    parseMessageData(item);

                    // Remove update data from message buffer
                    state.RemoveData(0, item.Length);
                }                
            }

            // Draw walls
            UpdateArrived();

            // Continue event loop
            Networking.GetData(state);
        }

        /// <summary>
        /// Receives data from server and uses it to update the world
        /// </summary>
        /// <param name="state"></param>
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

                    parseMessageData(item);
                }                
            }
            /*///////////////////////Data has been added to World/////////////////////////*/

            // Remove update data from message buffer
            state.RemoveData(0, data.Length - lastItemLength);

            // TODO: Notify View to draw new data (using event?)
            UpdateArrived();

            // Continue the event loop
            Networking.GetData(state);
        }

        /// <summary>
        /// Creates the corresponding object from the given Json string and uses it to
        /// update the World.
        /// </summary>
        /// <param name="json"></param>
        private void parseMessageData (string json)
        {
            if (json.Contains("wall"))
            {
                Wall newWall = JsonConvert.DeserializeObject<Wall>(json);
                world.addWall(newWall);
            }
            else if (json.Contains("tank"))
            {
                Tank newTank = JsonConvert.DeserializeObject<Tank>(json);
                if (newTank.getDied())
                {
                    world.removeTank(newTank);
                }
                else
                {
                    world.addTank(newTank);
                }
            }
            else if (json.Contains("proj"))
            {
                Projectile newProj = JsonConvert.DeserializeObject<Projectile>(json);
                if (newProj.getDied())
                {
                    world.removeProjectile(newProj);
                }
                else
                {
                    world.addProjectile(newProj);
                }
            }
            else if (json.Contains("power"))
            {
                Powerup newPower = JsonConvert.DeserializeObject<Powerup>(json);
                if (newPower.getDied())
                {
                    world.removePowerup(newPower);
                }
                else
                {
                    world.addPowerup(newPower);
                }
            }
            else if (json.Contains("beam"))
            {
                Beam newBeam = JsonConvert.DeserializeObject<Beam>(json);
                world.addBeam(newBeam);
            }
            else
            {

            }
        }
    }
}