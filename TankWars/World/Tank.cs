﻿// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TankWars;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        [JsonProperty(PropertyName = "tank")]
        private int ID;

        [JsonProperty(PropertyName = "loc")]
        private Vector2D location;

        [JsonProperty(PropertyName = "bdir")]
        private Vector2D orientation;

        [JsonProperty(PropertyName = "tdir")]
        private Vector2D aiming = new Vector2D(0, -1);        

        [JsonProperty(PropertyName = "name")]
        private string name;

        //[JsonProperty(PropertyName = "hp")]
        //private int hitPoints = Constants.MaxHP;

        [JsonProperty(PropertyName = "score")]
        private int score = 0;

        [JsonProperty(PropertyName = "died")]
        private bool died = false;

        [JsonProperty(PropertyName = "dc")]
        private bool disconnected = false;

        [JsonProperty(PropertyName = "join")]
        private bool joined = false;

        public Tank()
        {
           
        }

        public int getID()
        {
            return ID;
        }

        public Vector2D getLocation()
        {
            return location;
        }

        public Vector2D getOrientation()
        {
            return orientation;
        }

        public bool getDied()
        {
            return died;
        }
    }
}