using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TankWars;

namespace World
{
    [JsonObject(MemberSerialization.OptIn)]
    class Wall
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
    }
}
