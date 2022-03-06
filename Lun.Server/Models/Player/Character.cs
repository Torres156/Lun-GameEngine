using LiteNetLib;
using Lun.Server.Network.Interfaces;
using Lun.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Models.Player
{
    internal class Character : CharacterModel, INetPeer
    {
        public int ID              { get; set; }
        public int AccountID       { get; set; }
        public int CharacterSlotID { get; set; }

        public NetPeer Peer { get; set; } 

        public void Save()
        {
            ExecuteNonQuery(@$"UPDATE {TABLE_CHARACTERS} SET
name='{Name}',
classid={ClassID},
spriteid={SpriteID},
x={Position.x},
y={Position.y}
WHERE id={ID};
");
        }
    }
}
