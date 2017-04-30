using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class DynamicObject : GraphicsObject
    {
        public Timer MoveTimer { get; set; }
        public int Speed { get; set; }
        public int Dx { get; set; }
        public int Dy { get; set; }
        public Direction Direction { get; set; }

        public DynamicObject(GUIForm guiForm, Rectangle rect, Direction direction) : base(guiForm, rect)
        {
            MoveTimer = new Timer();
            MoveTimer.Interval = 15;
            Dx = 0;
            Dy = 0;
            Direction = direction;
        }

        public virtual void Move()
        {
            if(Dx != 0 || Dy != 0)
            {
                var oldRect = Rect;
                Rect = new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height);
                GUIForm.Invalidate(Rect);
                GUIForm.Invalidate(oldRect);
            }
        }

        public virtual void StopMoving()
        {
            Dx = 0;
            Dy = 0;
        }

        public event EventHandler Destroyed;

        public void InvokeDestroyed()
        {
            Destroyed?.Invoke(this, new EventArgs());
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