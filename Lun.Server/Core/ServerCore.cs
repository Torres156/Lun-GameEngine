using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Core
{
    class ServerCore
    {
        public static bool Running = false;
        public static double DeltaTime;

        static double FixedPhysicTime = 1.0 / 60.0;

        public static void Core()
        {
            var timerOld = Environment.TickCount64;
            var timerNew = Environment.TickCount64;
            double accumulative = 0;
            while(Running)
            {
                Network.Socket.Pool();

                timerOld = timerNew;
                timerNew = Environment.TickCount64;
                DeltaTime = (timerNew - timerOld) / 1000.0;

                accumulative += DeltaTime;
                while(accumulative >= FixedPhysicTime)
                {
                    accumulative -= FixedPhysicTime;
                    // UPDATE
                }
            }
        }
    }
}
