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
    /// Represents a wall in the game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        /// <summary>
        /// The unique ID of this wall
        /// </summary>
        [JsonProperty(PropertyName = "wall")]
        private int ID;        

        /// <summary>
        /// The location of the first endpoint of the wall
        /// </summary>
        [JsonProperty(PropertyName = "p1")]
        private Vector2D firstEndpoint;

        /// <summary>
        /// The location of the second endpoint of the wall
        /// </summary>
        [JsonProperty(PropertyName = "p2")]
        private Vector2D secondEndpoint;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Wall()
        {

        }

        /// <summary>
        /// Returns the ID of this wall
        /// </summary>
        /// <returns></returns>
        public int getID()
        {
            return ID;
        }

        /// <summary>
        /// Returns the first endpoint of the wall
        /// </summary>
        /// <returns></returns>
        public Vector2D getFirstEndpoint()
        {
            return firstEndpoint;
        }

        /// <summary>
        /// Returns the second endpoint of the wall
        /// </summary>
        /// <returns></returns>
        public Vector2D getSecondEndpoint()
        {
            return secondEndpoint;
        }
    }
}
