using LiteNetLib.Utils;
using Lun.Client.Models.Player;
using Lun.Client.Scenes.Gameplay;
using Lun.Client.Scenes.Logged;
using Lun.Client.Services;
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
        enum Packet
        {
            Alert,
            Logged,
            AllClassData,
            AllCharacterAccount,
            MyCharacterData,
            ChangeToGameplay,
        }

        public static void Handle(NetDataReader buffer)
        {
            var packet = (Packet)buffer.GetInt();

            switch(packet)
            {
                case Packet.Alert: Alert(buffer); break;
                case Packet.Logged: Logged(buffer); break;
                case Packet.AllClassData: AllClassData(buffer); break;
                case Packet.AllCharacterAccount: AllCharacterAccount(buffer); break;
                case Packet.MyCharacterData: MyCharacterData(buffer); break;
                case Packet.ChangeToGameplay: ChangeToGameplay(buffer); break;
            }
        }

        static void ChangeToGameplay(NetDataReader buffer)
        {
            Game.SetScene<GameplayScene>();
        }

        static void MyCharacterData(NetDataReader buffer)
        {
            var player =  PlayerService.My ?? new Character();
            player.Name     = buffer.GetString();
            player.SpriteID = buffer.GetInt();
            player.Position = buffer.GetVector2();

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

            ClassService.Items = JsonConvert.DeserializeObject<ClassData[]>(json);
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
