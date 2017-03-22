using System.Drawing;

namespace BattleCity
{
    public delegate void RectEventHandler(object sender, RectEventArgs e);

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