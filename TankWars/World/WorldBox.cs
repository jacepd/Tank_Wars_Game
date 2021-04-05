// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Text;

namespace World
{
    public class WorldBox
    {
        private int size; // The length and width of the game screen

        // Items that need to be drawn every frame
        private Dictionary<int, Wall> Walls;
        private Dictionary<int, Tank> Tanks;
        private Dictionary<int, Projectile> Projectiles;
        private Dictionary<int, Beam> Beams;
        private Dictionary<int, Powerup> Powerups;

        /// <summary>
        /// Creates a new world to store drawable objects
        /// </summary>
        /// <param name="size"></param>
        public WorldBox(int size)
        {
            this.size = size;
        }

        /// <summary>
        /// Adds a new wall to the world
        /// </summary>
        /// <param name="newWall"></param>
        public void addWall(Wall newWall)
        {
            Walls.Add(newWall.getID(), newWall);
        }

        /// <summary>
        /// Adds a new tank to the world
        /// </summary>
        /// <param name="newTank"></param>
        public void addTank(Tank newTank)
        {
            Tanks.Add(newTank.getID(), newTank);
        }

        /// <summary>
        /// Adds a new projectile to the world
        /// </summary>
        /// <param name="newProjectile"></param>
        public void addProjectile(Projectile newProjectile)
        {
            Projectiles.Add(newProjectile.getID(), newProjectile);
        }

        /// <summary>
        /// Adds a new beam to the world
        /// </summary>
        /// <param name="newBeam"></param>
        public void addBeam(Beam newBeam)
        {
            Beams.Add(newBeam.getID(), newBeam);
        }

        /// <summary>
        /// Adds a new powerup to the world
        /// </summary>
        /// <param name="newPowerup"></param>
        public void addPowerup(Powerup newPowerup)
        {
            Powerups.Add(newPowerup.getID(), newPowerup);
        }

        /* TODO:
         * - Add 'RemoveTank', 'RemovePowerup', etc. methods
         *   to remove items with a 'dead' property from dictionary
         * - Add 'beam animations' class somewhere maybe ???
         * - ...
         */
    }
}
