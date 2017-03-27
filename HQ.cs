using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class HQ : Object
    {
        private bool destroyed;

        public HQ(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
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

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                float currentFrame = (!destroyed) ? 0.0f : 64.0f;
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
                    s.InvokeDestroy();
                    Destroyed = true;
                }
            }
        }
    }
}