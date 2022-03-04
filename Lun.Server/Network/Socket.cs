using LiteNetLib;
using Lun.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Network
{
    class Socket
    {
        public const int PORT = 4000;
        public const int MAX_CONNECTIONS = 100;

        static EventBasedNetListener listener;
        public static NetManager Device;

        public static void Initialize()
        {
            listener = new EventBasedNetListener();

            Device = new NetManager(listener);
            Device.AutoRecycle = true;            
            Device.Start(PORT);

            listener.ConnectionRequestEvent += Listener_ConnectionRequestEvent;
            listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;
            listener.PeerConnectedEvent += Listener_PeerConnectedEvent;
            listener.PeerDisconnectedEvent += Listener_PeerDisconnectedEvent;
        }

        private static void Listener_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"Connection entry <{peer.EndPoint.ToString()}> has been disconnected!");

            var accountFound = PlayerService.Accounts.Find(i => i.Peer == peer);
            if (accountFound != null)
            {
                
                PlayerService.Accounts.Remove(accountFound);
            }

        }

        private static void Listener_PeerConnectedEvent(NetPeer peer)
        {
            Console.WriteLine($"New connection entry <{peer.EndPoint.ToString()}>!");
        }

        private static void Listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            Receive.Handle(peer, reader);
        }

        private static void Listener_ConnectionRequestEvent(ConnectionRequest request)
        {
            if (Device.ConnectedPeersCount < MAX_CONNECTIONS)
                request.Accept();
            else
                request.Reject();
        }

        public static void Pool() 
        {
            Device.PollEvents();
        }
    }
}
