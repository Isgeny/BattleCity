using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class DynamicObject : GraphicsObject
    {
        public Timer MoveTimer { get; set; }
        private int _dx;
        private int _dy;
        private Direction _direction;

        public DynamicObject(GUIForm guiForm, Rectangle rect, Direction direction) : base(guiForm, rect)
        {
            MoveTimer = new Timer();
            MoveTimer.Interval = 15;
            _dx = 0;
            _dy = 0;
            _direction = direction;
        }

        public int Dx
        {
            get { return _dx; }
            set { _dx = value; }
        }

        public int Dy
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
            if(_dx != 0 || _dy != 0)
            {
                Rectangle oldRect = Rect;
                Rect = new Rectangle(Rect.X + _dx, Rect.Y + _dy, Rect.Width, Rect.Height);
                GUIForm.Invalidate(Rectangle.Ceiling(Rect));
                GUIForm.Invalidate(Rectangle.Ceiling(oldRect));
            }
        }

        public virtual void StopMoving()
        {
            _dx = 0;
            _dy = 0;
        }

        public event EventHandler Destroyed;

        public void InvokeDestroyed()
        {
            Destroyed?.Invoke(this, new EventArgs());
        }

        public EventHandler GetDestroyedListener()
        {
            return OnDestroyed;
        }

        protected virtual void OnDestroyed(object sender, EventArgs e)
        {
            GUIForm.Invalidate(Rect);
        }

        public event RectEventHandler CheckPosition;

        protected void InvokeCheckPosition(RectEventArgs e)
        {
            CheckPosition?.Invoke(this, e);
        }
    }
}