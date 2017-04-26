using System.Drawing;

namespace BattleCity
{
    public static class RectangleExtansion
    {
        public static bool LittleIntersectsWith(this Rectangle current, Rectangle dest)
        {
            Rectangle temp = Rectangle.Intersect(current, dest);
            float area1 = temp.Width * temp.Height;
            float area2 = current.Width * current.Height;
            return (area1 / area2 <= 0.75) ? true : false;
        }
    }
}
