using System.Drawing;

namespace BattleCity
{
    public delegate void RectEventHandler(object sender, RectEventArgs e);

    public class RectEventArgs
    {
        private RectangleF rect;

        public RectEventArgs(RectangleF rect)
        {
            this.rect = rect;
        }

        public RectangleF Rect
        {
            get { return rect; }
            set { rect = value; }
        }
    }
}