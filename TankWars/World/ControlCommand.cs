using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TankWars;

namespace World
{
    [JsonObject(MemberSerialization.OptIn)]
    class ControlCommand
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
