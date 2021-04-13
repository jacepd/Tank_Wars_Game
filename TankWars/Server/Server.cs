using System;
using System.Diagnostics;
using System.Threading;
using TankWars;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            ServerController control = new ServerController();
            control.StartServer();

            Thread thread = new Thread(Server.updateWorld);
            thread.Start();
        }

        public static void updateWorld()
        {
            Stopwatch stopwatch = new Stopwatch();


        }
    }
}
