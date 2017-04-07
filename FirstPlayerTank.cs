using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace BattleCity
{
    public class FirstPlayerTank : PlayerTank
    {
        public FirstPlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            Amphibian = true;
            Gun = true;
            Stars = 0;
            Lives = 9;
            Points = 666;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.KeyDown += OnKeyDown;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.KeyDown -= OnKeyDown;
        }

        protected override void OnMoveTimerTick(object sender, EventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.Up))
            {
                Dx = 0.0f;
                Dy = -2.75f;
                Direction = Direction.Up;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.Left))
            {
                Dx = -2.75f;
                Dy = 0.0f;
                Direction = Direction.Left;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.Down))
            {
                Dx = 0.0f;
                Dy = 2.75f;
                Direction = Direction.Down;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.Right))
            {
                Dx = 2.75f;
                Dy = 0.0f;
                Direction = Direction.Right;
                IceTicks = 28;
            }
            else
            {
                Dx = 0.0f;
                Dy = 0.0f;
            }
            base.OnMoveTimerTick(sender, e);
        }

        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && Ammo > 0 && !ShootPressed)
            {
                Ammo--;
                Shell shell = new Shell(GUIForm, this);
                InvokeShoot(new ShellEventArgs(shell));
                ShootPressed = true;
            }
        }

        protected override void Respawn()
        {
            RectangleF newRect = new RectangleF(320.0f, 832.0f, Rect.Width, Rect.Height);
            Rect = newRect;
        }
    }
}