using Lun.Client.Scenes.Logged;
using Lun.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Client.Scenes.CreateCharacter
{
    internal class PanelCreate : Panel
    {
        Button btnBack;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public PanelCreate(Bond bond) : base(bond)
        {
            Anchor = Anchors.Center;
            Size = new Vector2(400, 300);
            FillColor = new Color(0, 0, 0, 200);
            OutlineThickness = 0;

            btnBack = new Button(this)
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
            btnBack.OnMouseReleased += (sender, e) => Game.SetScene<LoggedScene>();
        }


    }
}
