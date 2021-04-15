using System;
using System.Diagnostics;
using System.Threading;
using TankWars;

namespace Server
{
    public class Server
    {
        static void Main(string[] args)
        {
            ServerController control = new ServerController();
            control.ServerStartupEvent += ServerRunning;
            control.NewClentConnectedEvent += ClientConnected;
            control.StartServer();

            Thread thread = new Thread(Server.updateWorld);
            thread.Start();

            Console.Read();
        }

        private static void updateWorld()
        {
            Stopwatch stopwatch = new Stopwatch();


        }

        private static void ServerRunning(string message)
        {
            Console.WriteLine(message);
        }

        private static void ClientConnected(int playerID, string playerName)
        {
            Console.WriteLine("Accepted new connection.");
            Console.WriteLine("Player(" + playerID + ") " + playerName + " joined");
        }
    }
}
