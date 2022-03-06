global using static Lun.LunEngine;
global using Lun.Shared;
global using Lun.Shared.Extends;


using Lun.Client.Scenes.Menu;
using System;

namespace Lun.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.WindowSizeMin   = new Vector2(800, 600);
            Game.WindowSize      = new Vector2(1024, 600);
            Game.WindowTitle     = "Lun Engine";
            Game.WindowCanResize = true;            

            LoadFont("res/Rene Bieder  Milliard Medium.otf");
            Network.Socket.Initialize();

            Game.OnClosed += Game_OnClosed;
            Game.OnUpdate += Game_OnUpdate;            

            Game.SetScene<MenuScene>();
            Game.Run();            
        }

        private static void Game_OnUpdate()
        {
            Network.Socket.Poll();
        }

        private static void Game_OnClosed()
        {
            Network.Socket.Close();
        }

    }
}
