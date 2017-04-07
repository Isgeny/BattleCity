using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class DynamicObject : Object
    {
        private Timer _moveTimer;
        private float _dx;
        private float _dy;
        private Direction _direction;

        public DynamicObject(GUIForm guiForm, RectangleF rect, Direction direction) : base(guiForm, rect)
        {
            _moveTimer = new Timer();
            _moveTimer.Interval = 15;
            _dx = 0.0f;
            _dy = 0.0f;
            _direction = direction;
        }

        public Timer MoveTimer
        {
            get { return _moveTimer; }
            set { _moveTimer = value; }
        }

        public float Dx
        {
            get { return _dx; }
            set { _dx = value; }
        }

        public float Dy
        {
            get { return _dy; }
            set { _dy = value; }
        }

        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public virtual void Move()
        {
            Rect = new RectangleF(Rect.X + _dx, Rect.Y + _dy, Rect.Width, Rect.Height);
            GUIForm.Invalidate(new Region(new RectangleF(Rect.X - 8.0f, Rect.Y - 8.0f, Rect.Width + 16.0f, Rect.Height + 16.0f)));
        }

        public virtual void StopMoving()
        {
            _dx = 0.0f;
            _dy = 0.0f;
        }
    }
}