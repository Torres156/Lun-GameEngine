using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Network
{
    static class Sender
    {
        enum Packet
        {
            Register,
            Login,
            CreateCharacter,
            UseCharacter,
        }

        public static void UseCharacter(int slotID)
        {
            var buffer = Create(Packet.UseCharacter);
            buffer.Put(slotID);
            SendTo(buffer);
        }

        public static void CreateCharacter(int slotID, string name, int classID, int spriteID)
        {
            var buffer = Create(Packet.CreateCharacter);
            buffer.Put(slotID);
            buffer.Put(name);
            buffer.Put(classID);
            buffer.Put(spriteID);
            SendTo(buffer);
        }

        public static void Login(string accountName, string accountPassword)
        {
            var buffer = Create(Packet.Login);
            buffer.Put(accountName);
            buffer.Put(accountPassword);
            SendTo(buffer);
        }

        public static void Register(string accountName, string accountPassword)
        {
            var buffer = Create(Packet.Register);
            buffer.Put(accountName);
            buffer.Put(accountPassword);
            SendTo(buffer);
        }

        static NetDataWriter Create(Packet packet)
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
