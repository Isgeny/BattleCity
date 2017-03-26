using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Water : Obstacle
    {
        private Timer spriteTimer;

        public Water(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 0, false)
        {
            spriteTimer = new Timer();
            spriteTimer.Interval = 1500;
            spriteTimer.Tick += OnSpriteTimer;
        }

        private void OnPaint(object sender, PaintEventArgs e)
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

        public override void ShellCollision(Shell shell)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;
            spriteTimer.Start();
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;
            spriteTimer.Stop();
        }

        public override void SubscribeToObjectPosition(Object obj)
        {
            base.SubscribeToObjectPosition(obj);
        }

        public override void UnsubscribeFromObjectPosition(Object obj)
        {
            base.UnsubscribeFromObjectPosition(obj);
        }

        private void OnChekPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Tank)
                {
                    ((Tank)sender).StopMoving();
                }
            }
        }
    }
}