using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class DynamicObject : Object
    {
        private Timer moveTimer;
        private float dx;
        private float dy;
        private Direction direction;

        public DynamicObject(GUIForm guiForm, RectangleF rect, Direction direction) : base(guiForm, rect)
        {
            dx = 0.0f;
            dy = 0.0f;
            this.direction = direction;
            moveTimer = null;
        }

        public Timer MoveTimer
        {
            get { return moveTimer; }
            set { moveTimer = value; }
        }

        public float Dx
        {
            get { return dx; }
            set { dx = value; }
        }

        public float Dy
        {
            get { return dy; }
            set { dy = value; }
        }

        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public virtual void Move()
        {
            RectangleF oldRect = Rect;
            RectangleF newRect = new RectangleF(Rect.X + dx, Rect.Y + dy, Rect.Width, Rect.Height);
            Rect = newRect;
            GUIForm.Invalidate(new Region(oldRect));
            GUIForm.Invalidate(new Region(newRect));
        }

        public virtual void StopMoving()
        {
            dx = 0.0f;
            dy = 0.0f;
        }
    }
}