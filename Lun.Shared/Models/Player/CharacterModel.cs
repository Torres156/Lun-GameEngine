using Lun.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Shared.Models.Player
{
    public abstract class CharacterModel
    {                    
        public string     Name      { get; set; } = ""; 
        public int        ClassID   { get; set; } 
        public int        SpriteID  { get; set; }         
        public Vector2    Position  { get; set; } 
        public Directions Direction { get; set; } 
    }
}
