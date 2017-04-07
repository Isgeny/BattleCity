using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class HQ : Object
    {
        private bool _destroyed;

        public HQ(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            _destroyed = false;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            Destroyed += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            Destroyed -= OnDestroyed;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                float currentFrame = (!_destroyed) ? 0.0f : 64.0f;
                g.DrawImage(Properties.Resources.Tile_5, Rect, new RectangleF(currentFrame, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Tank)
                {
                    ((Tank)sender).StopMoving();
                }
                else if(sender is Shell)
                {
                    Shell s = sender as Shell;
                    s.InvokeDestroyed();
                    _destroyed = true;
                    InvokeDestroyed();
                }
            }
        }
    }
}