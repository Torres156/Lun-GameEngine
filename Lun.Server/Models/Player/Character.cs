using LiteNetLib;
using Lun.Server.Models.Map;
using Lun.Server.Network.Interfaces;
using Lun.Shared.Models.Player;
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
        // Data
        public int ID              { get; set; }
        public int AccountID       { get; set; }
        public int CharacterSlotID { get; set; }
        public int MapID           { get; set; }


        public WorldInstance Instance { get; set; }
        public NetPeer       Peer     { get; set; } 


        public void Save()
        {
            ExecuteNonQuery(@$"UPDATE    { TABLE_CHARACTERS } SET
            name                         = '{ Name }',
            classid                      = { ClassID },
            spriteid                     = { SpriteID },
            dir                          = { (int)Direction },
            mapid                        = { MapID },
            x                            = { Position.x },
            y                            = { Position.y }
            WHERE                     id = { ID };
            ");
        }
    }
}
