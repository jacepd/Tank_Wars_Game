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
        private int hitPoints = Constants.hitpoints;

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
        /// How many frames have passed since this tank died
        /// </summary>
        private int framesSinceDied = 0;

        /// <summary>
        /// How many frames have passed since this tank last fired
        /// </summary>
        private int framesSinceFired = 0;

        /// <summary>
        /// Whether or not the tank is able to fire a normal shot
        /// </summary>
        private bool ableToFire = true;

        /// <summary>
        /// Whether oro not the tank is able to fire a beam
        /// </summary>
        private bool ableToFireBeam = false;

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
        /// Returns how many frames have passed since this tank died
        /// </summary>
        /// <returns></returns>
        public int getFramesSinceDied()
        {
            return framesSinceDied;
        }

        /// <summary>
        /// Returns how many frames have passed since this tank last fired
        /// </summary>
        /// <returns></returns>
        public int getFramesSinceFired()
        {
            return framesSinceFired;
        }

        /// <summary>
        /// Returns whether or not the tank is able to fire a normal shot
        /// </summary>
        /// <returns></returns>
        public bool getAbleToFire()
        {
            return ableToFire;
        }

        /// <summary>
        /// Return whether or noto the tank is able to fire a beam
        /// </summary>
        /// <returns></returns>
        public bool getAbleToFireBeam()
        {
            return ableToFireBeam;
        }

        /// <summary>
        /// Makes the tank able to fire a beam
        /// </summary>
        public void setBeamFireable()
        {
            ableToFireBeam = true;
        }

        /// <summary>
        /// Makes the tank unable to fire a beam
        /// </summary>
        public void setBeamNotFireable()
        {
            ableToFireBeam = false;
        }

        /// <summary>
        /// Increments the value of framesSinceDead
        /// </summary>
        public void incrementFramesSinceDead()
        {
            framesSinceDied++;
        }

        /// <summary>
        /// Increments the value of framesSinceFired
        /// </summary>
        public void incrementFramesSinceFired()
        {
            framesSinceFired++;
            if(framesSinceFired > Constants.framesPerShot)
            {
                ableToFire = true;
            }
        }

        /// <summary>
        /// Prevents tank from firing for a certain amount of time
        /// </summary>
        public void startFireCooldown()
        {
            ableToFire = false;
            framesSinceFired = 0;
        }

        /// <summary>
        /// Resets the values of died and framesSinceDead to defaults
        /// </summary>
        public void resetDeath()
        {
            framesSinceDied = 0;
            died = false;
            hitPoints = Constants.hitpoints;
        }

        /// <summary>
        /// Increments the score of a player when they killed another tank
        /// </summary>
        public void incrementScore()
        {
            score++;
        }

        /// <summary>
        /// Sets the tank's location to the given value
        /// </summary>
        /// <param name="loc"></param>
        public void setLocation(Vector2D loc)
        {
            location = loc;
        }

        /// <summary>
        /// Sets the value of died to true
        /// </summary>
        public void setDead()
        {
            died = true;
        }

        /// <summary>
        /// Sets the value of died to false
        /// </summary>
        public void setAlive()
        {
            died = false;
        }

        /// <summary>
        /// Sets the value of disconnected to true
        /// </summary>
        public void setDisconnected()
        {
            disconnected = true;
        }

        /// <summary>
        /// Decreases tank's hitpoints
        /// </summary>
        public void decreaseHitpoints()
        {
            hitPoints -= 1;
            if(hitPoints == 0)
            {
                died = true;
            }
        }

        /// <summary>
        /// Kills the tank
        /// </summary>
        public void kill()
        {
            hitPoints = 0;
            died = true;
        }

        /// <summary>
        /// Updates the tank to match the given input
        /// </summary>
        /// <param name="input"></param>
        public void updateTank(ControlCommand input)
        {
            double x = location.GetX();
            double y = location.GetY();

            switch (input.getMoveDirection())
            {
                case "left":
                    x -= Constants.engineStrength;
                    orientation = new Vector2D(-1, 0);
                    break;
                case "right":
                    x += Constants.engineStrength;
                    orientation = new Vector2D(1, 0);
                    break;
                case "up":
                    y -= Constants.engineStrength;
                    orientation = new Vector2D(0, -1);
                    break;
                case "down":
                    y += Constants.engineStrength;
                    orientation = new Vector2D(0, -1);
                    break;
            }

            // Set direction of turret
            aiming = input.getTurretDirection();
            
            // Wraparound
            if(x > Constants.worldSize / 2)
            {
                x -= Constants.worldSize;
            }
            if (x < -Constants.worldSize / 2)
            {
                x += Constants.worldSize;
            }
            if (y > Constants.worldSize / 2)
            {
                y -= Constants.worldSize;
            }
            if (y < -Constants.worldSize / 2)
            {
                y += Constants.worldSize;
            }

            location = new Vector2D(x, y);
        }
    }
}
