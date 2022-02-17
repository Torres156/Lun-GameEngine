using LiteNetLib;
using LiteNetLib.Utils;
using Lun.Server.Models.Player;
using Lun.Server.Network.Interfaces;
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
