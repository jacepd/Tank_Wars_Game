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
    /// Represents a powerup in the game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Powerup
    {
        /// <summary>
        /// The unique ID of this powerup
        /// </summary>
        [JsonProperty(PropertyName = "power")]
        private int ID;

        /// <summary>
        /// The location of the powerup
        /// </summary>
        [JsonProperty(PropertyName = "loc")]
        private Vector2D location;

        /// <summary>
        /// Whether or not the powerup has been collected yet
        /// </summary>
        [JsonProperty(PropertyName = "died")]
        private bool died;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Powerup()
        {

        }

        /// <summary>
        /// Returns the ID of this powerup
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
        /// Returns whether or no the powerup has been collected yet
        /// </summary>
        /// <returns></returns>
        public bool getDied()
        {
            return died;
        }
    }
}
