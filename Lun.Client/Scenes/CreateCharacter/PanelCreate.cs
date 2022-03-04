using Lun.Client.Scenes.Logged;
using Lun.Client.Services;
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
        Button btnBack, btnCreate, btnLeft, btnRight;
        TextBox txtName;
        ComboBox cmbClass, cmbSex;

        int currentSprite = 0;
        int currentSlot { get; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public PanelCreate(Bond bond, int currentSlot) : base(bond)
        {
            this.currentSlot = currentSlot;

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

            txtName = new TextBox(this)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(360,20),
                Position = new Vector2(0,50),
                MaxLength = 20,
            };

            cmbClass = new ComboBox(this)
            {
                Position = new Vector2(20,100),
                Size = new Vector2(200,20),
            };

            foreach (var i in ClassService.Items)
                cmbClass.Items.Add(i.Name);

            cmbClass.SelectIndex = cmbClass.Items.Count > 0 ? 0 : -1;
            cmbClass.OnSelectIndexChanged += (sender) => { currentSprite = 0; };

            cmbSex = new ComboBox(this)
            {
                Anchor = Anchors.TopRight,
                Position = new Vector2(20, 100),
                Size = new Vector2(150, 20),
            };
            cmbSex.Items.AddRange(new[]{ "Male", "Female"});
            cmbSex.SelectIndex = Rand(0, 1);
            cmbSex.OnSelectIndexChanged += (sender) => { currentSprite = 0; };

            btnCreate = new Button(this)
            {
                Anchor = Anchors.BottomCenter,
                Position = new Vector2(0,10),
                Size = new Vector2(150,25),
                Text = "Create"
            };
            btnCreate.OnMouseReleased += BtnCreate_OnMouseReleased;

            btnLeft = new Button(this)
            {
                Size = new Vector2(20, 20),
                Text = "<",
                Anchor = Anchors.TopRight,
                Position = new Vector2(150,162),
            };
            btnLeft.OnMouseReleased += BtnLeft_OnMouseReleased;

            btnRight = new Button(this)
            {
                Size = new Vector2(20, 20),
                Text = ">",
                Anchor = Anchors.TopRight,
                Position = new Vector2(20, 162),
            };
            btnRight.OnMouseReleased += BtnRight_OnMouseReleased;

            OnDraw += PanelCreate_OnDraw;
        }

        private void BtnCreate_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            var name = txtName.Text.Trim();

            if (name.Length < 3)
            {
                Game.Scene.Alert("Minimum 3 letters for name!");
                return;
            }

            if (ClassService.Items.Length == 0)
                return;

            var currentClass = ClassService.Items[cmbClass.SelectIndex];
            var sprites = cmbSex.SelectIndex == 0 ? currentClass.MaleSprite : currentClass.FemaleSprite;

            Network.Sender.CreateCharacter(currentSlot, name, cmbClass.SelectIndex, sprites[currentSprite]);
        }

        private void BtnRight_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            if (ClassService.Items.Length == 0)
                return;

            var currentClass = ClassService.Items[cmbClass.SelectIndex];
            var sprites = cmbSex.SelectIndex == 0 ? currentClass.MaleSprite : currentClass.FemaleSprite;

            currentSprite++;
            if (currentSprite >= sprites.Length)
                currentSprite = 0;
        }

        private void BtnLeft_OnMouseReleased(ControlBase sender, MouseButtonEventArgs e)
        {
            if (ClassService.Items.Length == 0)
                return;

            var currentClass = ClassService.Items[cmbClass.SelectIndex];
            var sprites = cmbSex.SelectIndex == 0 ? currentClass.MaleSprite : currentClass.FemaleSprite;

            currentSprite--;
            if (currentSprite < 0)
                currentSprite = sprites.Length - 1;
        }

        private void PanelCreate_OnDraw(ControlBase sender)
        {
            var gp = GlobalPosition();

            DrawText("Character name:", 14, txtName.GlobalPosition() + new Vector2(0, -18), Color.White);
            DrawText("Class:", 14, cmbClass.GlobalPosition() + new Vector2(0, -18), Color.White);
            DrawText("Appearance:", 14, cmbClass.GlobalPosition() + new Vector2(210, -18), Color.White);

            if (ClassService.Items.Length == 0)
                return;

            var currentClass = ClassService.Items[cmbClass.SelectIndex];
                        
            var wrap = GetTextWrap(currentClass.Description, 200, 12);
            if (wrap.Length > 0)
                for (int i = 0; i < wrap.Length; i++)
                    DrawText(wrap[i], 12, gp + new Vector2(20, 130 + 14 * i), new Color(180, 180, 180));


            var sprites = cmbSex.SelectIndex == 0 ? currentClass.MaleSprite : currentClass.FemaleSprite;
            var texSprite = ResourceService.Sprite[sprites[currentSprite]];
            var sizeSprite = texSprite.size / 4;
            var position = gp + new Vector2(230 + 75, 130);
            var origin = new Vector2(sizeSprite.x / 2, 0);
            DrawTexture(texSprite,
                new Rectangle(position, sizeSprite),
                new Rectangle(Vector2.Zero, sizeSprite),
                Color.White,
                origin);


        }
    }
}
