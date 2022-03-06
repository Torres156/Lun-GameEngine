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

            // Warp to map
            Warp(player, player.MapID, player.Position, true);
        }

        public static void Warp(Character player, int mapID, Vector2 position, bool isEnterGame = false)
        {
            player.Position = position;

            if (isEnterGame)
            {
                if (player.MapID == mapID)
                { } // UDPATE POSITION
                else
                    Network.Sender.PlayerRemove(player); // REMOVE PLAYER FROM OTHER MAP                
            }

            player.MapID = mapID;
            Network.Sender.CheckMap(player);
        }

        public static void SendWarpInfo(Character player)
        {
            Network.Sender.PlayerDataMeToMap(player);
            Network.Sender.PlayerDataAllToMe(player);
        }
    }
}
