using LiteNetLib;
using LiteNetLib.Utils;
using Lun.Server.Models.Map;
using Lun.Server.Models.Player;
using Lun.Server.Network.Interfaces;
using Lun.Server.Services;
using Lun.Shared.Enums;
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
        public static void PlayerMovement(Character player)
        {
            var buffer = Create(PacketServer.PlayerMovement);
            buffer.Put(player.Name);
            buffer.Put((int)player.Direction);
            buffer.Put(player.Position);

            SendToInstanceBut(player, buffer, DeliveryMethod.Unreliable);
        }

        public static void PlayerRemove(Character player)
        {
            var buffer = Create(PacketServer.PlayerRemove);
            buffer.Put(player.Name);
            SendToInstanceBut(player, buffer);
        }

        public static void CheckMap(Character player)
        {
            var buffer = Create(PacketServer.MapCheck);
            SendTo(player, buffer);
        }

        public static void ChangeToGameplay(INetPeer peer)
            => SendTo(peer, Create(PacketServer.ChangeToGameplay));

        static void packetCharacterData(Character player, NetDataWriter buffer)
        {
            buffer.Put(player.Name);
            buffer.Put(player.SpriteID);
            buffer.Put((int)player.Direction);
            buffer.Put(player.Position);
        }

        public static void PlayerDataAllToMe(Character player)
        {
            PlayerService.Characters.ForEach(i =>
            {
                if (i != player && i.Instance == player.Instance)
                {
                    var buffer = Create(PacketServer.PlayerData);
                    packetCharacterData(i, buffer);
                    SendTo(player, buffer);
                }
            });
        }

        public static void PlayerDataMeToMap(Character player)
        {
            var buffer = Create(PacketServer.PlayerData);
            packetCharacterData(player, buffer);
            SendToInstanceBut(player, buffer);
        }

        public static void MyCharacterData(Character player)
        {
            var buffer = Create(PacketServer.MyCharacterData);
            packetCharacterData(player, buffer);

            SendTo(player, buffer);
        }

        public static void AllCharacterAccount(Account account)
        {
            var buffer = Create(PacketServer.AllCharacterAccount);

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

            var buffer = Create(PacketServer.AllClassData);
            buffer.Put(json);
            SendTo(peer, buffer);
        }

        public static void Logged(Account account)
        {
            var buffer = Create(PacketServer.Logged);
            SendTo(account, buffer);
        }

        public static void Alert(INetPeer peer, string msg)
            => Alert(peer.Peer, msg);

        public static void Alert(NetPeer peer, string msg)
        {
            var buffer = Create(PacketServer.Alert);
            buffer.Put(msg);
            SendTo(peer, buffer);
        }

        static NetDataWriter Create(PacketServer packet)
        {
            var buffer = new NetDataWriter();
            buffer.Put((int)packet);

            return buffer;
        }

        static void SendToInstance(WorldInstance instance, NetDataWriter buffer, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            PlayerService.Characters.ForEach(i =>
            {
                if (i.Instance == instance)
                    SendTo(i, buffer, deliveryMethod);
            });
        }

        static void SendToInstanceBut(Character player, NetDataWriter buffer, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            PlayerService.Characters.ForEach(i =>
            {
                if (i != player && i.Instance == player.Instance)
                    SendTo(i, buffer, deliveryMethod);
            });
        }

        static void SendToInstance(Character player, NetDataWriter buffer, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            PlayerService.Characters.ForEach(i =>
            {
                if (i.Instance == player.Instance)
                    SendTo(i, buffer, deliveryMethod);
            });
        }

        static void SendTo(NetPeer peer, NetDataWriter buffer, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            peer.Send(buffer, deliveryMethod);
        }

        static void SendTo(INetPeer peer, NetDataWriter buffer, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
            => SendTo(peer.Peer, buffer, deliveryMethod);
    }
}
