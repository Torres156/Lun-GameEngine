using Lun.Animation;
using Lun.Client.Services;
using Lun.Shared.Enums;
using Lun.Shared.Models.Player;

namespace Lun.Client.Models.Player
{
    internal class Character : CharacterModel
    {
        AnimationSprite animation;

        public int TimerFrame = 0;

        public void Draw()
        {
            // Create animation if value is null
            CreateAnimation();

            var dir = "up";

            switch(Direction)
            {
                case Directions.Up   : dir = "up"; break;
                case Directions.Down : dir = "down"; break;
                case Directions.Left : dir = "left"; break;
                case Directions.Right: dir = "right"; break;
            }

            var action = "normal";

            // Moving frame
            if (TimerFrame > 0)
                action = "move";

            animation.Position = Position;
            animation.Play(action + "_" + dir);
        }

        public void DrawTexts()
        {
            if (animation == null)
                return;

            var tex  = ResourceService.Sprite[SpriteID];
            var size = tex.size / 4;

            var pos = Position + new Vector2(-(GetTextWidth(Name, 13)) / 2, -size.y - 14);
            DrawText(Name, 13, pos, Color.White, true);
        }

        public void Update()
        {
            // Clear timer
            if (TimerFrame > 0 && TickCount >= TimerFrame)
                TimerFrame = 0;
        }

        void CreateAnimation()
        {
            var tex = ResourceService.Sprite[SpriteID];

            if (animation != null && animation.texture == tex)
                return;

            var size = tex.size / 4;

            animation             = new AnimationSprite(tex);
            animation.repeat      = true;
            animation.frame_timer = 150;
            animation.origin      = new Vector2(size.x / 2, size.y - 4);

            // Normal frames
            animation.Add("normal_up", new Rectangle(0,size.y * 3, size.x, size.y));
            animation.Add("normal_down", new Rectangle(0, 0, size.x, size.y));
            animation.Add("normal_left", new Rectangle(0, size.y * 1, size.x, size.y));
            animation.Add("normal_right", new Rectangle(0, size.y * 2, size.x, size.y));

            // Move frames
            animation.Add("move_up", new Rectangle(size.x, size.y * 3, size.x, size.y), new Rectangle(size.x * 3, size.y * 3, size.x, size.y));
            animation.Add("move_down", new Rectangle(size.x, 0, size.x, size.y), new Rectangle(size.x * 3, 0, size.x, size.y));
            animation.Add("move_left", new Rectangle(size.x, size.y * 1, size.x, size.y), new Rectangle(size.x * 3, size.y * 1, size.x, size.y));
            animation.Add("move_right", new Rectangle(size.x, size.y * 2, size.x, size.y), new Rectangle(size.x * 3, size.y * 2, size.x, size.y));
        }
    }
}
