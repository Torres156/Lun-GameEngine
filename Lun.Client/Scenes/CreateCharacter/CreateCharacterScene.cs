using Lun.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Scenes.CreateCharacter
{
    internal class CreateCharacterScene : SceneBase
    {
        Texture Background;

        int currentSlot = 0;

        PanelCreate panelCreate;

        public CreateCharacterScene(int currentSlot) : base()
        {
            this.currentSlot = currentSlot;
        }

        public override void UnloadContent()
        {
            Background.Destroy();
            base.UnloadContent();
        }

        public override void LoadContent()
        {
            Background = new Texture("res/ui/background-createchar.jpg", true) { Smooth = true };

            panelCreate = new PanelCreate(this);
        }

        public override void Draw()
        {
            DrawTexture(Background, new Rectangle(Vector2.Zero, Game.WindowSize));

            base.Draw();
        }
    }
}
