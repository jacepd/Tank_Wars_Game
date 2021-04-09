// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Text;
using TankWars;

namespace TankWars
{
    public class World
    {
        private int size; // The length and width of the game screen
        private int playerID;

        // Items that need to be drawn every frame
        private Dictionary<int, Wall> Walls;
        private Dictionary<int, Tank> Tanks;
        private Dictionary<int, Projectile> Projectiles;
        private Dictionary<int, Powerup> Powerups;

        /// <summary>
        /// Creates a new world to store drawable objects
        /// </summary>
        /// <param name="size"></param>
        public World(int size)
        {
            this.size = size;
            Walls = new Dictionary<int, Wall>();
            Tanks = new Dictionary<int, Tank>();
            Projectiles = new Dictionary<int, Projectile>();
            Powerups = new Dictionary<int, Powerup>();
        }

        public void setPlayerID(int id)
        {
            playerID = id;
        }

        public int getWorldSize()
        {
            return size;
        }

        public bool containsTank()
        {
            return Tanks.ContainsKey(playerID);
        }

        public Tank getPlayerTank()
        {
            return Tanks[playerID];
        }

        public IEnumerable<Tank> getTanks()
        {
            return Tanks.Values;
        }

        public IEnumerable<Wall> getWalls()
        {
            return Walls.Values;
        }

        public IEnumerable<Projectile> getProjectiles()
        {
            return Projectiles.Values;
        }

        public IEnumerable<Powerup> getPowerups()
        {
            return Powerups.Values;
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
            if (Tanks.ContainsKey(newTank.getID()))
            {
                Tanks.Remove(newTank.getID());
            }
            Tanks.Add(newTank.getID(), newTank);
        }

        /// <summary>
        /// Adds a new projectile to the world
        /// </summary>
        /// <param name="newProjectile"></param>
        public void addProjectile(Projectile newProjectile)
        {
            if (Projectiles.ContainsKey(newProjectile.getID()))
            {
                Projectiles.Remove(newProjectile.getID());
            }
            Projectiles.Add(newProjectile.getID(), newProjectile);
        }

        /// <summary>
        /// Adds a new powerup to the world
        /// </summary>
        /// <param name="newPowerup"></param>
        public void addPowerup(Powerup newPowerup)
        {
            if (Powerups.ContainsKey(newPowerup.getID()))
            {
                Powerups.Remove(newPowerup.getID());
            }
            Powerups.Add(newPowerup.getID(), newPowerup);
        }

        /// <summary>
        /// Removes the given tank from the world
        /// </summary>
        /// <param name="tank"></param>
        public void removeTank(Tank tank)
        {
            Tanks.Remove(tank.getID());
        }

        /// <summary>
        /// Removes the given projectile from the world
        /// </summary>
        /// <param name="projectile"></param>
        public void removeProjectile(Projectile projectile)
        {
            Projectiles.Remove(projectile.getID());
        }

        /// <summary>
        /// Removes the given powerup from the world
        /// </summary>
        /// <param name="tank"></param>
        public void removePowerup(Powerup powerup)
        {
            Powerups.Remove(powerup.getID());
        }


        public void setWorldSize(int size)
        {
            this.size = size;
        }
    }
}
