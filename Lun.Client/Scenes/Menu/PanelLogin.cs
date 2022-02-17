using Lun.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Scenes.Menu
{
    class PanelLogin : Panel
    {
        Button btnClose, btnLogin;
        TextBox txtUsername, txtPassword;

        bool hoverRegister, hoverWebsite;

        public PanelLogin(Bond bond) : base(bond)
        {
            Anchor = Anchors.Center;
            Size = new Vector2(250, 230);
            FillColor = new Color(0, 0, 0, 200);
            OutlineThickness = 0;

            btnClose = new Button(this)
            {
                Anchor = Anchors.TopRight,
                Size = new Vector2(20),
                Position = new Vector2(5),
                Text = "X",
                FillColor = Color.Transparent,
                FillColor_Hover = new Color(30,30,30),
                FillColor_Press = Color.Transparent,
                OutlineThickness = 0,
            };
            btnClose.OnMouseReleased += (sender, e) => Game.Running = false;

            txtUsername = new TextBox(this)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(230,20),
                Position = new Vector2(0,60),
            };

            txtPassword = new TextBox(this)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(230, 20),
                Position = new Vector2(0, 100),
                Password = true,
            };

            btnLogin = new Button(this)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(100,30),
                Text = "Enter",
                Position = new Vector2(0,140)
            };

            OnDraw += PanelLogin_OnDraw;
            OnMouseMove += PanelLogin_OnMouseMove;
            OnMouseReleased += PanelLogin_OnMouseReleased;
            btnLogin.OnMouseReleased += BtnLogin_OnMouseReleased;
        }

        private void BtnLogin_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            var user = txtUsername.Text.Trim();
            var pwd = txtPassword.Text.Trim();

            if (user.Length < 3 || pwd.Length < 3)
            {
                Game.Scene.Alert("Username or password is not valid!");
                return;
            }

            Network.Sender.Login(user, pwd);
        }

        private void PanelLogin_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            if (hoverRegister)
            {
                Hide();
                Game.Scene.FindControl<PanelRegister>().Show();
            }
        }

        private void PanelLogin_OnMouseMove(ControlBase sender, Vector2 e)
        {
            var gp = GlobalPosition();
            hoverRegister = false;
            if (new Rectangle(gp + new Vector2(10 + GetTextWidth("Don't have an account?", 12), 190),
                new Vector2(GetTextWidth("Click here!",12), 14)).Contains(e))
                hoverRegister = true;

            hoverWebsite = false;
            if (new Rectangle(gp + new Vector2(5, 210),
                new Vector2(GetTextWidth("Enter our website!", 12), 14)).Contains(e))
                hoverWebsite = true;
        }

        private void PanelLogin_OnDraw(ControlBase sender)
        {
            var gp = GlobalPosition();

            DrawText("Account login", 18, gp + new Vector2(10,10), Color.White);

            DrawText("Username:", 12, txtUsername.GlobalPosition() + new Vector2(0, -16), Color.White);
            DrawText("Password:", 12, txtPassword.GlobalPosition() + new Vector2(0, -16), Color.White);

            DrawText("Don't have an account?", 12, gp + new Vector2(5, 190), Color.White, true);

            DrawText("Click here!", 12,
                gp + new Vector2(10 + GetTextWidth("Don't have an account?", 12), 190),
                hoverRegister ? new Color(135, 126, 199) : Color.White, true);

            DrawText("Enter our website!", 12, gp + new Vector2(5, 210),
                hoverWebsite ? new Color(135, 126, 199) : Color.White, true);
        }
    }
}
