﻿// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Diagnostics;
using System.Threading;
using TankWars;
using System.Xml;

namespace Server
{
    /// <summary>
    /// Represents the server running the game
    /// </summary>
    public class Server
    {
        private static World world;
        private static ServerController control;

        static void Main(string[] args)
        {
            control = new ServerController();
            control.ServerStartupEvent += ServerRunning;
            control.NewClentConnectedEvent += ClientConnected;
            control.ErrorEvent += HandleError;
            world = control.getWorld();

            ParseSettings();

            control.StartServer();

            // Loops once per frame
            while (true)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while(stopwatch.ElapsedMilliseconds < Constants.MSPerFrame)
                {
                    // Do nothing
                }
                stopwatch.Reset();

                Thread thread = new Thread(control.updateWorld);
                thread.Start();

                //control.CheckForDisconnected();

                control.SendWorld();

            }
           
        }

        /// <summary>
        /// Parses the game settings from the given settings file and
        /// initializes the world accordingly
        /// </summary>
        public static void ParseSettings()
        {
            using (XmlReader reader = XmlReader.Create("..\\..\\..\\..\\Resources\\settings.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "UniverseSize":
                                Constants.worldSize = reader.ReadElementContentAsInt();
                                break;
                            case "MSPerFrame":
                                Constants.MSPerFrame = reader.ReadElementContentAsInt();
                                break;
                            case "FramesPerShot":
                                Constants.framesPerShot = reader.ReadElementContentAsInt();
                                break;
                            case "RespawnRate":
                                Constants.respawnRate = reader.ReadElementContentAsInt();
                                break;
                            case "Hitpoints":
                                Constants.hitpoints = reader.ReadElementContentAsInt();
                                break;
                            case "ProjectileSpeed":
                                Constants.projectileSpeed = reader.ReadElementContentAsInt();
                                break;
                            case "EngineStrength":
                                Constants.engineStrength = reader.ReadElementContentAsInt();
                                break;
                            case "MaxPowerups":
                                Constants.maxPowerups = reader.ReadElementContentAsInt();
                                break;
                            case "MaxPowerupDelay":
                                Constants.maxPowerupDelay = reader.ReadElementContentAsInt();
                                break;
                            case "Wall":
                                // Parse walls???
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Write that the server is running, to the console
        /// </summary>
        /// <param name="message"></param>
        private static void ServerRunning(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Write that a client has connected, to the console
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="playerName"></param>
        private static void ClientConnected(int playerID, string playerName)
        {
            Console.WriteLine("Accepted new connection.");
            Console.WriteLine("Player(" + playerID + ") " + playerName + " joined");
        }

        /// <summary>
        /// Prints message to console when an error occurs
        /// </summary>
        /// <param name="message"></param>
        private static void HandleError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
