// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TankWars;

namespace TankWars
{
    /// <summary>
    /// Represents a tank in the game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        /// <summary>
        /// The int ID unique to this tank
        /// </summary>
        [JsonProperty(PropertyName = "tank")]
        private int ID;

        /// <summary>
        /// The location of the tank
        /// </summary>
        [JsonProperty(PropertyName = "loc")]
        private Vector2D location;

        /// <summary>
        /// The orientation of the tank's body
        /// </summary>
        [JsonProperty(PropertyName = "bdir")]
        private Vector2D orientation = new Vector2D(0, -1);

        /// <summary>
        /// The orientation of the tank's turret
        /// </summary>
        [JsonProperty(PropertyName = "tdir")]
        private Vector2D aiming = new Vector2D(0, -1);        

        /// <summary>
        /// The name associated with the tank
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        private string name;

        /// <summary>
        /// The amount of hp the tank has
        /// </summary>
        [JsonProperty(PropertyName = "hp")]
        private int hitPoints = 3;

        /// <summary>
        /// The current score of the tank
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        private int score = 0;

        /// <summary>
        /// Whether or not the tank has died
        /// </summary>
        [JsonProperty(PropertyName = "died")]
        private bool died = false;

        /// <summary>
        /// Whether or not the tank has disconnected
        /// </summary>
        [JsonProperty(PropertyName = "dc")]
        private bool disconnected = false;

        /// <summary>
        /// Whether or not the tank has joined
        /// </summary>
        [JsonProperty(PropertyName = "join")]
        private bool joined = false;

        /// <summary>
        /// Default tank constructor
        /// </summary>
        public Tank()
        {
           
        }

        /// <summary>
        /// Creates a Tank with the given ID, playername, and location
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="playerName"></param>
        public Tank(int ID, string playerName, Vector2D location)
        {
            this.ID = ID;
            name = playerName;
            this.location = location;          
        }

        /// <summary>
        /// Returns the ID
        /// </summary>
        /// <returns></returns>
        public int getID()
        {
            return ID;
        }

        /// <summary>
        /// Returns the location
        /// </summary>
        /// <returns></returns>
        public Vector2D getLocation()
        {
            return location;
        }

        /// <summary>
        /// Returns the orientation of the tank's body
        /// </summary>
        /// <returns></returns>
        public Vector2D getOrientation()
        {
            return orientation;
        }

        /// <summary>
        /// Returns whether or not the tank has died
        /// </summary>
        /// <returns></returns>
        public bool getDied()
        {
            return died;
        }

        /// <summary>
        /// Returns the direction of the turret
        /// </summary>
        /// <returns></returns>
        public Vector2D getTurretDirection()
        {
            return aiming;
        }

        /// <summary>
        /// Returns the name of the tank
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return name;
        }

        /// <summary>
        /// Returns the tank's score
        /// </summary>
        /// <returns></returns>
        public int getScore()
        {
            return score;
        }

        /// <summary>
        /// Returns the tank's current health
        /// </summary>
        /// <returns></returns>
        public int getHealth()
        {
            return hitPoints;
        }

        /// <summary>
        /// Returns whether or not the tank has disconnected
        /// </summary>
        /// <returns></returns>
        public bool getDisconnected()
        {
            return disconnected;
        }

        /// <summary>
        /// Returns whether or not this tank has joined the game
        /// </summary>
        /// <returns></returns>
        public bool getJoined()
        {
            return joined;
        }

        /// <summary>
        /// Updates the tank to match the given input
        /// </summary>
        /// <param name="input"></param>
        public void updateTank(ControlCommand input)
        {
            // logic
            throw new NotImplementedException();
        }
    }
}
