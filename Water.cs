using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Water : Obstacle
    {
        private Timer spriteTimer;

        public Water(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            spriteTimer = new Timer();
            spriteTimer.Interval = 1500;
            spriteTimer.Tick += OnSpriteTimer;
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                float currentFrame = (DateTime.Now.Millisecond % 1000 < 500) ? 0.0f : 32.0f;
                g.DrawImage(Properties.Resources.Tile_2, Rect, new RectangleF(currentFrame, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
            }
        }

        private void OnSpriteTimer(object sender, EventArgs e)
        {
            GUIForm.Invalidate(new Region(Rect));
        }

        public override void SubscribeToForm()
        {
            base.SubscribeToForm();
            spriteTimer.Start();
        }

        public override void UnsubscribeFromForm()
        {
            base.UnsubscribeFromForm();
            spriteTimer.Stop();
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Tank && !((Tank)sender).Amphibian)
                {
                    ((Tank)sender).StopMoving();
                }
            }
        }
    }
}