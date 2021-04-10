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
    /// Represents input information sent to the server
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ControlCommand
    {
        /// <summary>
        /// The direction the player is moving.
        /// Can be "up", "down", "left", "right", or "none".
        /// </summary>
        [JsonProperty(PropertyName = "moving")]
        string moveDirection;

        /// <summary>
        /// The type of projectile being fired.
        /// Can be "main", "alt", or "none".
        /// </summary>
        [JsonProperty(PropertyName = "fire")]
        string fire;

        /// <summary>
        /// The direction the turret is aiming
        /// </summary>
        [JsonProperty(PropertyName = "tdir")]
        Vector2D turretAimDirection;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ControlCommand()
        {

        }

        /// <summary>
        /// Sets the move direction to the given string
        /// </summary>
        /// <param name="dir"></param>
        public void setMoveDirection(string dir)
        {
            moveDirection = dir;
        }

        /// <summary>
        /// Sets the fire type to the given string
        /// </summary>
        /// <param name="fir"></param>
        public void setFire(string fir)
        {
            fire = fir;
        }

        /// <summary>
        /// Sets the turret direction to the given string
        /// </summary>
        /// <param name="dir"></param>
        public void setTurretAimDirection(Vector2D dir)
        {
            turretAimDirection = dir;
            turretAimDirection.Normalize();
        }
    }
}
