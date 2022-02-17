using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Network.Interfaces
{
    interface INetPeer
    {
        NetPeer Peer { get; }
    }
}
