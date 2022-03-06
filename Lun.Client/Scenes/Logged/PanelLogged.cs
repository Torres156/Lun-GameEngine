using Lun.Client.Scenes.CreateCharacter;
using Lun.Client.Scenes.Menu;
using Lun.Client.Services;
using Lun.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Scenes.Logged
{
    internal class PanelLogged : Panel
    {
        Button btnBack;
        Button btnCreateOrUse, btnDelete;

        int hoverSlot = -1, selectSlot = 0;


        public PanelLogged(Bond bond) : base(bond)
        {
            Anchor           = Anchors.Center;
            Size             = new Vector2(400, 300);
            FillColor        = new Color(0, 0, 0, 200);
            OutlineThickness = 0;

            btnBack = new Button(this)
            {
                Anchor           = Anchors.TopRight,
                Size             = new Vector2(20),
                Position         = new Vector2(5),
                Text             = "X",
                FillColor        = Color.Transparent,
                FillColor_Hover  = new Color(30, 30, 30),
                FillColor_Press  = Color.Transparent,
                OutlineThickness = 0,
            };
            btnBack.OnMouseReleased += BtnBack_OnMouseReleased;

            btnCreateOrUse = new Button(this)
            {
                Text     = PlayerService.CharacterName_Slot[selectSlot].Length > 0 ? "Use" : "Create",
                Size     = new Vector2(100, 25),
                Anchor   = Anchors.BottomCenter,
                Position = new Vector2(-55, 10),
            };
            btnCreateOrUse.OnMouseReleased += BtnCreateOrUse_OnMouseReleased;

            btnDelete = new Button(this)
            {
                Text            = "Delete",
                Size            = new Vector2(100, 25),
                Anchor          = Anchors.BottomCenter,
                Position        = new Vector2(55, 10),
                FillColor       = new Color(190, 89, 89),
                FillColor_Hover = new Color(225, 113, 113),
                FillColor_Press = new Color(144, 86, 86),
            };

            OnDraw          += PanelLogged_OnDraw;
            OnMouseMove     += PanelLogged_OnMouseMove;
            OnMouseReleased += PanelLogged_OnMouseReleased;
        }

        private void BtnCreateOrUse_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            if (PlayerService.CharacterName_Slot[selectSlot].Length == 0)
                Game.SetScene<CreateCharacterScene>(selectSlot);
            else
                Network.Sender.UseCharacter(selectSlot);
        }

        private void PanelLogged_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            if (hoverSlot > -1)
            {
                selectSlot = hoverSlot;
                btnCreateOrUse.Text = PlayerService.CharacterName_Slot[selectSlot].Length > 0 ? "Use" : "Create";
                return;
            }
        }

        private void PanelLogged_OnMouseMove(ControlBase sender, Vector2 e)
        {
            var gp       = GlobalPosition();
            var rec_size = new Vector2(Size.x - 20, 30);

            hoverSlot = -1;
            for(int i = 0; i < 5; i++)
                if (new Rectangle(gp + new Vector2((Size.x - rec_size.x) / 2, 40 + (rec_size.y + 10) * i), rec_size).Contains(e))
                {
                    hoverSlot = i;
                    return;
                }

        }

        private void BtnBack_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            Network.Socket.Device.DisconnectAll();
            Game.SetScene<MenuScene>();
        }

        private void PanelLogged_OnDraw(ControlBase sender)
        {
            var gp = GlobalPosition();
            var rec_size = new Vector2(Size.x - 20, 30);

            for (int i = 0; i < 5; i++)
            {
                var c1        = new Color(30, 30, 30);
                var c2        = new Color(50, 50, 50);
                var colorText = new Color(200, 200, 200);

                // Hover Style
                if (i == hoverSlot)
                {
                    c1 = new Color(50, 50, 50);
                    c2 = new Color(70, 70, 70);
                    colorText = Color.White;
                }

                // Select Style
                if (i == selectSlot)
                {
                    c1 = new Color(50, 50, 50);
                    c2 = new Color(234, 114, 58);
                    colorText = c2;
                }

                var rec_pos = gp + new Vector2((Size.x - rec_size.x) / 2, 40 + (rec_size.y + 10) * i);
                DrawRectangle(rec_pos, rec_size, c1, 2, c2);

                var text = "Empty Slot";
                if (PlayerService.CharacterName_Slot[i].Length > 0)
                    text = PlayerService.CharacterName_Slot[i];
                DrawText(text, 13, rec_pos + new Vector2((rec_size.x - GetTextWidth(text, 13)) / 2, (rec_size.y - GetTextHeight(text, 13)) / 2), colorText);
            }
        }
    }
}
