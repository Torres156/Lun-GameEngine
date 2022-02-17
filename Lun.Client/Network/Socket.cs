using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Network
{
    class Socket
    {
        static EventBasedNetListener listener;
        public static NetManager Device { get; private set; }

        public static void Initialize()
        {
            listener = new EventBasedNetListener();
            Device = new NetManager(listener);
            Device.Start();

            listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;
        }

        private static void Listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            Receive.Handle(reader);
        }

        public static void Poll()
        {
            Device.PollEvents();
        }

        public static void Connect()
        {
            if (Device == null)
                return;

            Device.Connect("localhost", 4000, "");
        }

        public static void Close()
        {
            Device.FirstPeer?.Disconnect();
            Device = null;
        }

        public static bool IsConnected
            => Device.FirstPeer != null && Device.FirstPeer.ConnectionState == ConnectionState.Connected;
    }
}
