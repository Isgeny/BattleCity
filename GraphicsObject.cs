using System.Drawing;

namespace BattleCity
{
    public abstract class GraphicsObject : Object
    {
        public Rectangle Rect { get; set; }

        public GraphicsObject(GUIForm guiForm, Rectangle rect) : base(guiForm)
        {
            Rect = rect;
        }

        protected virtual void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect) && sender is DynamicObject)
                ((DynamicObject)sender).StopMoving();
        }

        public RectEventHandler GetCheckPositionListener()
        {
            return OnCheckPosition;
        }
    }
}