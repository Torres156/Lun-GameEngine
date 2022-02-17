using Lun.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Scenes.Menu
{
    class PanelRegister : Panel
    {

        Button btnClose, btnRegister;
        TextBox txtUsername, txtPassword, txtPassword2;

        public PanelRegister(Bond bond) : base(bond)
        {
            Anchor = Anchors.Center;
            Size = new Vector2(250, 230);
            FillColor = new Color(0, 0, 0, 200);
            OutlineThickness = 0;
            Visible = false;

            btnClose = new Button(this)
            {
                Anchor = Anchors.TopRight,
                Size = new Vector2(20),
                Position = new Vector2(5),
                Text = "X",
                FillColor = Color.Transparent,
                FillColor_Hover = new Color(30, 30, 30),
                FillColor_Press = Color.Transparent,
                OutlineThickness = 0,
            };
            btnClose.OnMouseReleased += (sender, e) =>
            {
                Hide();
                Game.Scene.FindControl<PanelLogin>().Show();
            };

            txtUsername = new TextBox(this)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(230, 20),
                Position = new Vector2(0, 60),
            };

            txtPassword = new TextBox(this)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(230, 20),
                Position = new Vector2(0, 100),
                Password = true,
            };

            txtPassword2 = new TextBox(this)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(230, 20),
                Position = new Vector2(0, 140),
                Password = true,
            };

            btnRegister = new Button(this)
            {
                Anchor = Anchors.BottomCenter,
                Size = new Vector2(100, 30),
                Text = "Register",
                Position = new Vector2(0, 10)
            };            

            OnDraw += PanelRegister_OnDraw;
            btnRegister.OnMouseReleased += BtnRegister_OnMouseReleased;
        }

        private void BtnRegister_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            var user = txtUsername.Text.Trim();
            var pwd = txtPassword.Text.Trim();
            var pwd2 = txtPassword2.Text.Trim();

            if (user.Length < 3)
            {
                Game.Scene.Alert($"Username does not contain 3 characters!");
                return;
            }

            if (pwd.Length < 3)
            {
                Game.Scene.Alert($"Password does not contain 3 characters!");
                return;
            }

            if (pwd != pwd2)
            {
                Game.Scene.Alert($"Passwords do not match!");
                return;
            }

            Network.Sender.Register(user, pwd);
        }

        private void PanelRegister_OnDraw(ControlBase sender)
        {
            var gp = GlobalPosition();

            DrawText("New account", 18, gp + new Vector2(10, 10), Color.White);

            DrawText("Username:", 12, txtUsername.GlobalPosition() + new Vector2(0, -16), Color.White);
            DrawText("Password:", 12, txtPassword.GlobalPosition() + new Vector2(0, -16), Color.White);
            DrawText("Repeat Password:", 12, txtPassword2.GlobalPosition() + new Vector2(0, -16), Color.White);
        }
    }
}
