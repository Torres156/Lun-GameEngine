﻿global using static Lun.Server.Services.DatabaseService;

using Lun.Server.Core;
using Lun.Server.Models.Data;
using Lun.Server.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

namespace Lun.Server
{
    class Program
    {        

        static void Main(string[] args)
        {
            Console.Title = "Lun server";
            Console.WriteLine("Starting server!");
            Console.WriteLine("");

            // Start database manager
            DatabaseManager = new MySQLContext();
            DatabaseManager.Run();
            Console.WriteLine("Database started!");
            Console.WriteLine("");

            // Start network device
            Network.Socket.Initialize();
            Console.WriteLine($"Network device started on port<{Network.Socket.PORT}>!");
            Console.WriteLine("");

            ServerCore.Running = true;
            var threadServer = new Thread(ServerCore.Core);
            threadServer.Start();

            ConsoleCore.Core();
        }
    }
}
