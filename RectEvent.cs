using System.Drawing;

namespace BattleCity
{
    public delegate void RectEventHandler(object sender, RectEventArgs e);

    public class RectEventArgs
    {
        public Rectangle CurrentRect { get; set; }
        public Rectangle NewRect { get; set; }

        public RectEventArgs(Rectangle currentRect, Rectangle newRect)
        {
            CurrentRect = currentRect;
            NewRect = newRect;
        }
    }
}