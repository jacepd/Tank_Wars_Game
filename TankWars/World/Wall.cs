// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TankWars;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        [JsonProperty(PropertyName = "wall")]
        private int ID;        

        [JsonProperty(PropertyName = "p1")]
        private Vector2D firstEndpoint;

        [JsonProperty(PropertyName = "p2")]
        private Vector2D secondEndpoint;

        public Wall()
        {

        }

        public int getID()
        {
            return ID;
        }
    }
}
