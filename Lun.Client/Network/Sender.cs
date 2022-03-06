using LiteNetLib;
using LiteNetLib.Utils;
using Lun.Client.Services;
using Lun.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Network
{
    static class Sender
    {
        public static void PlayerMovement()
        {
            var buffer = Create(PacketClient.PlayerMovement);
            buffer.Put((int)PlayerService.My.Direction);
            buffer.Put(PlayerService.My.Position);
            SendTo(buffer, DeliveryMethod.Unreliable);
        }

        public static void MapAnswer(bool value)
        {
            var buffer = Create(PacketClient.MapAnswer);
            buffer.Put(value);
            SendTo(buffer);
        }

        public static void UseCharacter(int slotID)
        {
            var buffer = Create(PacketClient.UseCharacter);
            buffer.Put(slotID);
            SendTo(buffer);
        }

        public static void CreateCharacter(int slotID, string name, int classID, int spriteID)
        {
            var buffer = Create(PacketClient.CreateCharacter);
            buffer.Put(slotID);
            buffer.Put(name);
            buffer.Put(classID);
            buffer.Put(spriteID);
            SendTo(buffer);
        }

        public static void Login(string accountName, string accountPassword)
        {
            var buffer = Create(PacketClient.Login);
            buffer.Put(accountName);
            buffer.Put(accountPassword);
            SendTo(buffer);
        }

        public static void Register(string accountName, string accountPassword)
        {
            var buffer = Create(PacketClient.Register);
            buffer.Put(accountName);
            buffer.Put(accountPassword);
            SendTo(buffer);
        }

        static NetDataWriter Create(PacketClient packet)
        {
            var buffer = new NetDataWriter();
            buffer.Put((int)packet);

            return buffer;
        }

        static void SendTo(NetDataWriter buffer, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            if (!Socket.IsConnected)
                return;

            Socket.Device.FirstPeer.Send(buffer, deliveryMethod);
        }
    }
}
