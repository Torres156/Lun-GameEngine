using Lun.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Scenes.Menu
{
    class MenuScene : SceneBase
    {
        Texture Background;

        PanelLogin panelLogin;
        PanelRegister panelRegister;

        long timerConnect;

        public override void LoadContent()
        {
            Background = new Texture("res/ui/background-title.png", true) { Smooth = true };

            panelLogin = new PanelLogin(this);
            panelRegister = new PanelRegister(this);
        }
        
        public override void Draw()
        {
            DrawTexture(Background, new Rectangle(Vector2.Zero, Game.WindowSize));

            base.Draw();
        }

        public override void Update()
        {
            if (!Network.Socket.IsConnected && Environment.TickCount64 > timerConnect)
            {
                Network.Socket.Connect();
                timerConnect = Environment.TickCount64 + 1000;
            }

            base.Update();
        }

        public override void Destroy()
        {
            base.Destroy();

            Background.Destroy();
        }
    }
}
