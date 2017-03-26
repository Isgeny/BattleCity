using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class HQ : Obstacle
    {
        private bool destroyed;

        public HQ(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 2, false)
        {
            destroyed = false;
        }

        public bool Destroyed
        {
            get { return destroyed; }
            set
            {
                destroyed = value;
                GUIForm.Invalidate(new Region(Rect));
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                float currentFrame = (!destroyed) ? 0.0f : 64.0f;
                g.DrawImage(Properties.Resources.Tile_5, Rect, new RectangleF(currentFrame, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
            }
        }


        public override void ShellCollision(Shell shell)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;
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