using Lun.Server.Models.Player;
using Lun.Shared.Models.Player;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Services
{
    internal static class ClassService
    {
        public static List<ClassModel> Items;

        /// <summary>
        /// Initialize Class Data
        /// </summary>
        public static void Initialize()
        {
            Items = new List<ClassModel>();

            var path = "data/classes/";

            // Check if exists directory
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Check if exists files
            if (Directory.GetFiles(path).Length == 0)
            {
                // Create default class data
                Items.Add(new ClassModel
                {
                    Name         = "Warrior",
                    Description  = "Good for melee battles, your focus is pure physical strenght.",
                    MaleSprite   = new[] { 1 },
                    FemaleSprite = new[] { 2 },
                });

                var json = JsonConvert.SerializeObject(Items[0], Formatting.Indented);
                File.WriteAllText(path + "0.json", json);
                return;
            }
            else
            {
                int i = 0;
                // Load class data
                while(File.Exists(path + $"{i}.json"))
                {
                    var json = File.ReadAllText(path + $"{i}.json");
                    var data = JsonConvert.DeserializeObject<ClassModel>(json);
                    Items.Add(data);
                    i++;
                }
            }
        }
    }
}
