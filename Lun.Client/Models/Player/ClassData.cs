using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Models.Player
{
    internal class ClassData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int[] MaleSprite { get; set; } = { };
        public int[] FemaleSprite { get; set; } = { };
    }
}
