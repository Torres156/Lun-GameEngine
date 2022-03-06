using Lun.Server.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Services
{
    static class PlayerService
    {
        public static List<Account> Accounts = new List<Account>();
        public static List<Character> Characters = new List<Character>();   

        public static void JoinGame(Character player)
        {
            // Sending my data
            Network.Sender.MyCharacterData(player);

            // Sending change to GameplayScene
            Network.Sender.ChangeToGameplay(player);
        }
    }
}
