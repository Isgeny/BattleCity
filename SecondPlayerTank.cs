using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using System.Resources;

namespace BattleCity
{
    public class SecondPlayerTank : PlayerTank
    {
        public SecondPlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
        }

        protected override void Respawn()
        {
            RectangleF newRect = new RectangleF(576.0f, 832.0f, Rect.Width, Rect.Height);
            Rect = newRect;
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                float currentFrame = ((Rect.X + Rect.Y) % 8 < 4) ? 0.0f : 64.0f;
                Bitmap bmp = GetCurrentSprite();

                if(OnIce && IceTicks != 27)
                {
                    currentFrame = 0.0f;
                }

                g.DrawImage(bmp, Rect, new RectangleF(currentFrame, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);

                if(Amphibian)
                {
                    g.DrawImage(Properties.Resources.Ship_Shield, Rect.X, Rect.Y);
                }
            }
        }

        protected override void OnMoveTimerTick(object sender, EventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.W))
            {
                Dx = 0.0f;
                Dy = -2.75f;
                Direction = Direction.Up;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.A))
            {
                Dx = -2.75f;
                Dy = 0.0f;
                Direction = Direction.Left;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.S))
            {
                Dx = 0.0f;
                Dy = 2.75f;
                Direction = Direction.Down;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.D))
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

        protected override Bitmap GetCurrentSprite()
        {
            ResourceManager rm = Properties.Resources.ResourceManager;
            string filename = "P2_" + Stars + "_" + (int)Direction;
            Bitmap bmp = (Bitmap)rm.GetObject(filename);
            return bmp;
        }

        protected override void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space && Ammo > 0)
            {
                Ammo--;
                Shell shell = new Shell(GUIForm, this);
                InvokeShoot(new ShellEventArgs(shell));
            }
        }
    }
}