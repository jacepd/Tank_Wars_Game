// Written by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TankWars;

namespace World
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ControlCommand
    {
        [JsonProperty(PropertyName = "moving")]
        string moveDirection;

        [JsonProperty(PropertyName = "fire")]
        string fire;

        [JsonProperty(PropertyName = "tdir")]
        string turretAimDirection;

        public ControlCommand()
        {

        }
    }
}
