using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Services
{
    static class ResourceService
    {
        public static List<Texture> Sprite;

        public static void LoadSprites()
        {
            // Check if sprite has loaded
            if (Sprite != null)
                return;

            int i = 1;
            Sprite = new List<Texture>();
            Sprite.Add(null); // Starting index 1
            while(File.Exists($"res/character/{i}.png"))
            {
                Sprite.Add(new Texture($"res/character/{i}.png"));
                i++;
            }
        }
    }
}
