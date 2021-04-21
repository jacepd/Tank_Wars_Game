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
    /// Represents a projectile in the game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        /// <summary>
        /// The unique ID of this projectile
        /// </summary>
        [JsonProperty(PropertyName = "proj")]
        private int ID;

        /// <summary>
        /// The point that the projectile originates from
        /// </summary>
        [JsonProperty(PropertyName = "loc")]
        private Vector2D location;

        /// <summary>
        /// The direction the projectile travels
        /// </summary>
        [JsonProperty(PropertyName = "dir")]
        private Vector2D direction;

        /// <summary>
        /// Whether or not the projectile has hit something or left the bounds of the world
        /// </summary>
        [JsonProperty(PropertyName = "died")]
        private bool died;

        /// <summary>
        /// The ID of the tank that fired this projectile
        /// </summary>
        [JsonProperty(PropertyName = "owner")]
        private int ownerID;

        /// <summary>
        /// Default contructor
        /// </summary>
        public Projectile()
        {

        }

        public Projectile(int ID, Vector2D loc, Vector2D dir, bool died, int ownerID)
        {
            this.ID = ID;
            location = loc;
            direction = dir;
            this.died = died;
            this.ownerID = ownerID;
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
        /// Returns the location the projectile originates from
        /// </summary>
        /// <returns></returns>
        public Vector2D getLocation()
        {
            return location;
        }

        /// <summary>
        /// Returns the directino the projectile travels
        /// </summary>
        /// <returns></returns>
        public Vector2D getDirection()
        {
            return direction;
        }

        /// <summary>
        /// Returns whether or not the projectile has died
        /// </summary>
        /// <returns></returns>
        public bool getDied()
        {
            return died;
        }

        /// <summary>
        /// Returns the ID of the tank that fired the projectile
        /// </summary>
        /// <returns></returns>
        public int getOwnerID()
        {
            return ownerID;
        }

        /// <summary>
        /// Updates the projectile based on what the client sent
        /// </summary>
        /// <param name="input"></param>
        public void updateProjectile(ControlCommand input)
        {
            Vector2D aiming = input.getTurretDirection();

            direction = aiming * Constants.projectileSpeed;
        }
    }
}
