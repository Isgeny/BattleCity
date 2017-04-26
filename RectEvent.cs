using System.Drawing;

namespace BattleCity
{
    public delegate void RectEventHandler(object sender, RectEventArgs e);

    public class RectEventArgs
    {
        private Rectangle _rect;

        public RectEventArgs(Rectangle rect)
        {
            _rect = rect;
        }

        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }
    }
}