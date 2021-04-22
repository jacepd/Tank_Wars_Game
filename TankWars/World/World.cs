// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Text;
using TankWars;

namespace TankWars
{
    /// <summary>
    /// Represents the Model containing all drawable objects present in the game
    /// </summary>
    public class World
    {
        private int size; // The length and width of the game screen
        private int playerID;  // Stores the playerID for this client

        // Number of objects created since game started; used for IDs
        private int numProjectilesCreated;
        private int numBeamsCreated;
        private int numPowerupsCreated;
        private int numWallsCreated;

        // Items that need to be drawn every frame
        private Dictionary<int, Wall> Walls;
        private Dictionary<int, Tank> Tanks;
        private Dictionary<int, Projectile> Projectiles;
        private Dictionary<int, Powerup> Powerups;
        private Dictionary<int, Beam> Beams;

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
            Beams = new Dictionary<int, Beam>();
            numPowerupsCreated = 0;
            numProjectilesCreated = 0;
            numBeamsCreated = 0;
            numWallsCreated = 0;
        }

        /// <summary>
        /// Checks if the world contains this clients tank
        /// </summary>
        /// <returns></returns>
        public bool containsTank()
        {
            return Tanks.ContainsKey(playerID);
        }

        /// <summary>
        /// Sets the world size
        /// </summary>
        /// <param name="size"></param>
        public void setWorldSize(int size)
        {
            this.size = size;
        }

        /// <summary>
        /// Sets this clients playerID
        /// </summary>
        /// <param name="id"></param>
        public void setPlayerID(int id)
        {
            playerID = id;
        }

        /// <summary>
        /// Returns the world size
        /// </summary>
        /// <returns></returns>
        public int getWorldSize()
        {
            return size;
        }

        /// <summary>
        /// Returns this clients tank
        /// </summary>
        /// <returns></returns>
        public Tank getPlayerTank()
        {
            return Tanks[playerID];
        }

        /// <summary>
        /// Returns Dictionary of all the tanks in this world
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Tank> getTanks()
        {
            return Tanks;
        }

        /// <summary>
        /// Returns Dictionary of all the walls in this world
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Wall> getWalls()
        {
            return Walls;
        }

        /// <summary>
        /// Returns Dictionary of all projectiles in the world
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Projectile> getProjectiles()
        {
            return Projectiles;
        }

        /// <summary>
        /// Returns Dictionary of all the powerups in this world
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Powerup> getPowerups()
        {
            return Powerups;
        }

        /// <summary>
        /// Returns Dictionary of all the beams in this world
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Beam> getBeams()
        {
            return Beams;
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
            numProjectilesCreated++;
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
            numPowerupsCreated++;
        }

        /// <summary>
        /// Adds a new Beam to the world
        /// </summary>
        /// <param name="newBeam"></param>
        public void addBeam(Beam newBeam)
        {
            Beams.Add(newBeam.getID(), newBeam);
            numBeamsCreated++;
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

        /// <summary>
        /// Removes the given beam from the world
        /// </summary>
        /// <param name="beam"></param>
        public void removeBeam(Beam beam)
        {
            Beams.Remove(beam.getID());
        }

        /// <summary>
        /// Returns the number of projectiles that have been created
        /// since the start of the game
        /// </summary>
        /// <returns></returns>
        public int getNumProjectileCreated()
        {
            return numProjectilesCreated;
        }

        /// <summary>
        /// Returns the number of powerups that have been created
        /// since the start of the game
        /// </summary>
        /// <returns></returns>
        public int getNumPowerupsCreated()
        {
            return numPowerupsCreated;
        }

        /// <summary>
        /// Returns the number of beams that have been created
        /// since the start of the game
        /// </summary>
        /// <returns></returns>
        public int getNumBeamsCreated()
        {
            return numBeamsCreated;
        }

        public int getNumWallsCreated()
        {
            return numWallsCreated;
        }

        public void incrementNumWalls()
        {
            numWallsCreated++;
        }

    }
}
