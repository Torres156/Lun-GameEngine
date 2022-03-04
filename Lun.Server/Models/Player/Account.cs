using LiteNetLib;
using Lun.Server.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Models.Player
{
    class Account : INetPeer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }


        public NetPeer Peer { get; set; }


        public Account()
        { }
    }
}
