using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Bush : Object
    {
        public Bush(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_3, Rect.X, Rect.Y);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Shell)
                {
                    Shell s = sender as Shell;
                    UnsubscribeFromPaint();
                    SubscribeToPaint();
                }
                else if(sender is Tank)
                {
                    UnsubscribeFromPaint();
                    SubscribeToPaint();
                }
            }
        }
    }
}