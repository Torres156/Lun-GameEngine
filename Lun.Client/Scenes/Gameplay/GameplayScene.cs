using Lun.Client.Services;
using Lun.Controls;
using Lun.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Scenes.Gameplay
{
    internal class GameplayScene : SceneBase
    {
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void LoadContent()
        {
            
        }

        public override void Draw()
        {
            PlayerService.My.Draw();

            PlayerService.Characters.ForEach(i => i.Draw());

            PlayerService.My.DrawTexts();
            PlayerService.Characters.ForEach(i => i.DrawTexts());


            base.Draw();
        }

        public override void Update()
        {
            PlayerService.My.Update();
            PlayerService.Characters.ForEach(i => i.Update());

            base.Update();
        }

        public override void FixedUpdate()
        {
            if (TextBox.Focus == null && Game.Window.HasFocus())
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.Up))
                    PlayerService.RequestMove(Directions.Up);

                if (Keyboard.IsKeyPressed(Keyboard.Key.S) || Keyboard.IsKeyPressed(Keyboard.Key.Down))
                    PlayerService.RequestMove(Directions.Down);

                if (Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.Left))
                    PlayerService.RequestMove(Directions.Left);

                if (Keyboard.IsKeyPressed(Keyboard.Key.D) || Keyboard.IsKeyPressed(Keyboard.Key.Right))
                    PlayerService.RequestMove(Directions.Right);
            }

            base.FixedUpdate();
        }
    }
}
