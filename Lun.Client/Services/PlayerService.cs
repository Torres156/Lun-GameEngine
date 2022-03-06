using Lun.Client.Models.Player;
using Lun.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Services
{
    internal static class PlayerService
    {
        public static string[]        CharacterName_Slot = new string[] { "", "", "", "", "" };
        public static Character       My                 = null;
        public static List<Character> Characters         = new List<Character>();

        public static void RequestMove(Directions direction)
        {
            var speed = (150 / 100f).Round();

            if (My.Direction != direction)
            {
                My.Direction = direction;
                // SEND UPDATE
            }
            
            if (CanMove(direction, speed))
            {
                var pos = My.Position;

                My.TimerFrame = TickCount + 150;                

                switch(direction)
                {
                    case Directions.Up:
                        pos.y -= speed;
                        break;

                    case Directions.Down:
                        pos.y += speed;
                        break;

                    case Directions.Left:
                        pos.x -= speed;
                        break;

                    case Directions.Right:
                        pos.x += speed;
                        break;
                }

                My.Position = pos;
                Network.Sender.PlayerMovement();
            }
        }

        static bool CanMove(Directions direction, float speed)
        {
            var nextPos = My.Position;
            switch(direction)
            {
                case Directions.Up:
                    nextPos.y -= speed + 8;
                    break;
                case Directions.Down:
                    nextPos.y += speed + 8;
                    break;
                case Directions.Left:
                    nextPos.x -= speed + 8;
                    break;
                case Directions.Right:
                    nextPos.x += speed + 8;
                    break;
            }

            if (direction == Directions.Up && nextPos.y < 8f)
            {
                // Map warps

                return false;
            }

            if (direction == Directions.Down && nextPos.y > Game.WindowSize.y - 8)
            {
                // Map warps

                return false;
            }

            if (direction == Directions.Left && nextPos.x < 8f)
            {
                // Map warps

                return false;
            }

            if (direction == Directions.Right && nextPos.x > Game.WindowSize.x - 8)
            {
                // Map warps

                return false;
            }

            return true;
        }
    }
}
