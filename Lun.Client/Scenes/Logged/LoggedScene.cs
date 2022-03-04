using Lun.Client.Services;
using Lun.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Scenes.Logged
{
    class LoggedScene : SceneBase
    {
        Texture Background;

        PanelLogged panelLogged;

        public override void UnloadContent()
        {
            base.UnloadContent();
            Background.Destroy();
        }

        public override void LoadContent()
        {
            Background = new Texture("res/ui/background-title.png", true) { Smooth = true };

            panelLogged = new PanelLogged(this);

            ResourceService.LoadSprites();
        }

        public override void Draw()
        {
            DrawTexture(Background, new Rectangle(Vector2.Zero, Game.WindowSize));
            base.Draw();
        }
    }
}
