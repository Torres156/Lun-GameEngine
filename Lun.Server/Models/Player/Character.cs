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
    internal class Character : INetPeer
    {
        public int AccountID { get; set; }
        public string Name { get; set; }
        public int ClassID { get; set; }
        public int SpriteID { get; set; }
        public Vector2 Position { get; set; }


        public NetPeer Peer { get; set; }
    }
}
