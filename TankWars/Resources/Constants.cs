// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWars;

namespace TankWars
{
    /// <summary>
    /// Contains constant values read from a settings file and used by the game
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The height and width of the world
        /// </summary>
        public static int worldSize{ get; set; }

        /// <summary>
        /// The amount of time it takes the server to update the world
        /// </summary>
        public static int MSPerFrame { get; set; }

        /// <summary>
        /// The amount of frames a tank must wait before firing another shot
        /// </summary>
        public static int framesPerShot { get; set; }

        /// <summary>
        /// The number of frames until a tank will respawn after being destroyed
        /// </summary>
        public static int respawnRate { get; set; }

        /// <summary>
        /// The maximum number of hitpoints a tank can have
        /// </summary>
        public static int hitpoints { get; set; }

        /// <summary>
        /// The velocity of a projectile
        /// </summary>
        public static int projectileSpeed { get; set; }

        /// <summary>
        /// The velocity of a tank
        /// </summary>
        public static int engineStrength { get; set; }

        /// <summary>
        /// The maximum number of powerups that can exist in the game at one time
        /// </summary>
        public static int maxPowerups { get; set; }

        /// <summary>
        /// The maximum number of frames that can pass before a powerup is attempted to be created
        /// </summary>
        public static int maxPowerupDelay { get; set; }
    }
}
