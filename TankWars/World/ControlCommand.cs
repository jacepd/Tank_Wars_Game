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
    public class ControlCommand
    {
        [JsonProperty(PropertyName = "moving")]
        string moveDirection;

        [JsonProperty(PropertyName = "fire")]
        string fire;

        [JsonProperty(PropertyName = "tdir")]
        Vector2D turretAimDirection;

        public ControlCommand()
        {

        }

        public void setMoveDirection(string dir)
        {
            moveDirection = dir;
        }

        public void setFire(string fir)
        {
            fire = fir;
        }

        public void setTurretAimDirection(Vector2D dir)
        {
            turretAimDirection = dir;
            turretAimDirection.Normalize();
        }
    }
}
