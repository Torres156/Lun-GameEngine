global using static Lun.Server.Services.DatabaseService;
global using Lun.Shared;
global using Lun.Shared.Extends;

using Lun.Server.Core;
using Lun.Server.Services;
using System;
using System.Threading;
using System.Globalization;

namespace Lun.Server
{
    class Program
    {        

        static void Main(string[] args)
        {
            var culture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = "."; // Float separator
            Thread.CurrentThread.CurrentCulture = culture;

            Console.Title = "Lun server";
            Console.WriteLine("Starting server!");
            Console.WriteLine("");

            // Start database manager
            DatabaseService.Initialize();
            Console.WriteLine("Database manager started!");            

            // Start class data
            ClassService.Initialize();
            Console.WriteLine("Classes loaded!");
            Console.WriteLine("");

            // Start network device
            Network.Socket.Initialize();
            Console.WriteLine($"Network device started on port<{Network.Socket.PORT}>!");
            Console.WriteLine("");

            ServerCore.Running = true;
            var threadServer = new Thread(ServerCore.Core);
            threadServer.Start();

            ConsoleCore.Core();

            DatabaseService.Close();
        }
    }
}
