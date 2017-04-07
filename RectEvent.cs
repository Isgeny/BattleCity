using System.Drawing;

namespace BattleCity
{
    public delegate void RectEventHandler(object sender, RectEventArgs e);

    public class RectEventArgs
    {
        private RectangleF _rect;

        public RectEventArgs(RectangleF rect)
        {
            this._rect = rect;
        }

        public RectangleF Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }
    }
}