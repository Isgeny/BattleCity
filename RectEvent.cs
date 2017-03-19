using System.Drawing;

namespace BattleCity
{
    public delegate void RectEventHandler(RectangleF rect);

    public class RectEventArgs
    {
        private RectangleF rect;

        public RectangleF Rect
        {
            get { return rect; }
            set { rect = value; }
        }
    }
}