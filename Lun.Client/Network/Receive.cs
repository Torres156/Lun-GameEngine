using LiteNetLib.Utils;
using Lun.Client.Models.Player;
using Lun.Client.Scenes.Gameplay;
using Lun.Client.Scenes.Logged;
using Lun.Client.Services;
using Lun.Shared.Enums;
using Lun.Shared.Models.Player;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Network
{
    static class Receive
    {

        public static void Handle(NetDataReader buffer)
        {
            var packet = (PacketServer)buffer.GetInt();

            switch(packet)
            {
                case PacketServer.Alert              : Alert(buffer); break;
                case PacketServer.Logged             : Logged(buffer); break;
                case PacketServer.AllClassData       : AllClassData(buffer); break;
                case PacketServer.AllCharacterAccount: AllCharacterAccount(buffer); break;
                case PacketServer.MyCharacterData    : MyCharacterData(buffer); break;
                case PacketServer.ChangeToGameplay   : ChangeToGameplay(buffer); break;
                case PacketServer.MapCheck           : MapCheck(buffer); break;
                case PacketServer.PlayerData         : PlayerData(buffer); break;
                case PacketServer.PlayerRemove       : PlayerRemove(buffer); break;
                case PacketServer.PlayerMovement     : PlayerMovement(buffer); break;
            }
        }

        static void PlayerMovement(NetDataReader buffer)
        {
            var name = buffer.GetString();
            var player = PlayerService.Characters.Find(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (player == null)
                return;

            player.Direction  = (Directions)buffer.GetInt();
            player.Position   = buffer.GetVector2();
            player.TimerFrame = TickCount + 150;
        }

        static void PlayerRemove(NetDataReader buffer)
        {
            var name = buffer.GetString();
            var player = PlayerService.Characters.Find(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (player != null)
                PlayerService.Characters.Remove(player);
        }

        static void PlayerData(NetDataReader buffer)
        {
            var name = buffer.GetString();

            var player = PlayerService.Characters.Find(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (player == null)
            {
                player = new Character();
                PlayerService.Characters.Add(player);
            }

            player.Name      = name;
            player.SpriteID  = buffer.GetInt();
            player.Direction = (Directions)buffer.GetInt();
            player.Position  = buffer.GetVector2();
        }

        static void MapCheck(NetDataReader buffer)
        {
            Sender.MapAnswer(false); // Need to update map data?

            PlayerService.Characters.Clear();
        }

        static void ChangeToGameplay(NetDataReader buffer)
        {
            Game.SetScene<GameplayScene>();
        }

        static void MyCharacterData(NetDataReader buffer)
        {
            var player = PlayerService.My ?? new Character();
            player.Name      = buffer.GetString();
            player.SpriteID  = buffer.GetInt();
            player.Direction = (Directions)buffer.GetInt();
            player.Position  = buffer.GetVector2();

            PlayerService.My = player;
        }

        static void AllCharacterAccount(NetDataReader buffer)
        {
            PlayerService.CharacterName_Slot = new[] { "", "", "", "", "" }; // RESET

            var count = buffer.GetInt();
            if (count > 0)
            {
                for(int i = 0; i < count; i++)
                    PlayerService.CharacterName_Slot[buffer.GetInt()] = buffer.GetString();
            }                
        }

        static void AllClassData(NetDataReader buffer)
        {
            var json = buffer.GetString();

            ClassService.Items = JsonConvert.DeserializeObject<ClassModel[]>(json);
        }

        static void Logged(NetDataReader buffer)
        {
            Game.SetScene<LoggedScene>();
        }

        static void Alert(NetDataReader buffer)
        {
            var msg = buffer.GetString();
            Game.Scene.Alert(msg);
        }
    }
}
