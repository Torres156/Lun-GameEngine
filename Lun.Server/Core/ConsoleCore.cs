using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Core
{
    class ConsoleCore
    {
        public static void Core()
        {
            string msg = "";
            Console.Write("Write a command:");
            while((msg = Console.ReadLine()).Trim().ToLower() != "exit")
            {
                var commands = msg.Split().Select(i => i.ToLower()).ToArray();
                
                switch(commands[0])
                {
                    default:
                        break;
                }


                Console.Write("\nWrite a command:");
            }
        }
    }
}
