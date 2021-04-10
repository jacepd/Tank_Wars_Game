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
    /// Represents a beam attack in the game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Beam
    {
        /// <summary>
        /// The unique ID of the beam
        /// </summary>
        [JsonProperty(PropertyName = "beam")]
        private int ID;

        /// <summary>
        /// The location the beam originates from
        /// </summary>
        [JsonProperty(PropertyName = "org")]
        private Vector2D origin;

        /// <summary>
        /// The direction the beam travels
        /// </summary>
        [JsonProperty(PropertyName = "dir")]
        private Vector2D direction;

        /// <summary>
        /// The ID of the tank that fired the beam
        /// </summary>
        [JsonProperty(PropertyName = "owner")]
        private int ownerID;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Beam()
        {

        }

        /// <summary>
        /// Returns the ID of this beam
        /// </summary>
        /// <returns></returns>
        public int getID()
        {
            return ID;
        }

        /// <summary>
        /// Returns the origin of the beam
        /// </summary>
        /// <returns></returns>
        public Vector2D getOrigin()
        {
            return origin;
        }

        /// <summary>
        /// Returns the direction fo the beam
        /// </summary>
        /// <returns></returns>
        public Vector2D getDirection()
        {
            return direction;
        }

        /// <summary>
        /// Returns the ID of the tank that fired the beam
        /// </summary>
        /// <returns></returns>
        public int getOwnerID()
        {
            return ownerID;
        }
    }
}
