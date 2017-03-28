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
            moveTimer = new Timer();
            moveTimer.Interval = 15;
            dx = 0.0f;
            dy = 0.0f;
            this.direction = direction;
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
            Rect = new RectangleF(Rect.X + dx, Rect.Y + dy, Rect.Width, Rect.Height);
            GUIForm.Invalidate(new Region(new RectangleF(Rect.X - 8.0f, Rect.Y - 8.0f, Rect.Width + 16.0f, Rect.Height + 16.0f)));
        }

        public virtual void StopMoving()
        {
            dx = 0.0f;
            dy = 0.0f;
        }
    }
}