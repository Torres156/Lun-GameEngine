using LiteNetLib.Utils;
using Lun.Client.Models.Player;
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
        }

        public static void Handle(NetDataReader buffer)
        {
            var packet = (Packet)buffer.GetInt();

            switch(packet)
            {
                case Packet.Alert: Alert(buffer); break;
                case Packet.Logged: Logged(buffer); break;
                case Packet.AllClassData: AllClassData(buffer); break;
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
