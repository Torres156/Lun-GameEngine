using LiteNetLib;
using LiteNetLib.Utils;
using Lun.Server.Models.Player;
using Lun.Server.Network.Interfaces;
using Lun.Server.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Network
{
    class Sender
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

        public static void ChangeToGameplay(INetPeer peer)
            => SendTo(peer, Create(Packet.ChangeToGameplay));

        public static void MyCharacterData(Character player)
        {
            var buffer = Create(Packet.MyCharacterData);

            buffer.Put(player.Name);
            buffer.Put(player.SpriteID);
            buffer.Put(player.Position);

            SendTo(player, buffer);
        }

        public static void AllCharacterAccount(Account account)
        {
            var buffer = Create(Packet.AllCharacterAccount);

            var count = Convert.ToInt32(ExecuteScalar($"SELECT COUNT(*) FROM {TABLE_CHARACTERS} WHERE accountid='{account.ID}';"));
            buffer.Put(count);

            if (count > 0)
            {
                var reader = ExecuteReader($"SELECT slotid, name FROM {TABLE_CHARACTERS} WHERE accountid='{account.ID}';");
                while (reader.Read())
                {
                    buffer.Put(reader.GetInt32(0));
                    buffer.Put(reader.GetString(1));
                }
                reader.Close();
            }
            SendTo(account, buffer);
        }


        public static void AllClassData(NetPeer peer)
        {
            var json = JsonConvert.SerializeObject(ClassService.Items.ToArray());

            var buffer = Create(Packet.AllClassData);
            buffer.Put(json);
            SendTo(peer, buffer);
        }

        public static void Logged(Account account)
        {
            var buffer = Create(Packet.Logged);
            SendTo(account, buffer);
        }

        public static void Alert(INetPeer peer, string msg)
            => Alert(peer.Peer, msg);

        public static void Alert(NetPeer peer, string msg)
        {
            var buffer = Create(Packet.Alert);
            buffer.Put(msg);
            SendTo(peer, buffer);
        }

        static NetDataWriter Create(Packet packet)
        {
            var buffer = new NetDataWriter();
            buffer.Put((int)packet);

            return buffer;
        }

        static void SendTo(NetPeer peer, NetDataWriter buffer, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            peer.Send(buffer, deliveryMethod);
        }

        static void SendTo(INetPeer peer, NetDataWriter buffer, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
            => SendTo(peer.Peer, buffer, deliveryMethod);
    }
}
