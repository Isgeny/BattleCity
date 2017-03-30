using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Water : Object
    {
        public Water(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_2, Rect);
            }
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