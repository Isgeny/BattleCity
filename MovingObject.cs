using System;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class MovingObject : GameObject
    {
        public virtual int Speed { get; set; }
        public int Dx { get; set; }
        public int Dy { get; set; }
        public virtual Direction Direction { get; set; }

        public Timer MoveTimer { get; set; }

        public Timer ExplosionTimer { get; set; }
        protected int ExplosionFrame { get; set; }

        public event RectEventHandler CheckPosition;
        public event EventHandler Destroyed;

        public MovingObject(GUIForm guiForm) : base(guiForm)
        {
            MoveTimer = new Timer();
            MoveTimer.Interval = 10;
        }

        protected void InvokeCheckPosition(RectEventArgs e)
        {
            CheckPosition?.Invoke(this, e);
        }

        public void InvokeDestroyed()
        {
            Destroyed?.Invoke(this, new EventArgs());
        }

        public void RemoveCheckPositionListeners()
        {
            CheckPosition = null;
        }

        public void RemoveDestroyedListeners()
        {
            Destroyed = null;
        }

        protected void CalcDxDyAlongDirection()
        {
            switch(Direction)
            {
                case Direction.Up:
                    Dx = 0;
                    Dy = -Speed;
                    break;
                case Direction.Left:
                    Dx = -Speed;
                    Dy = 0;
                    break;
                case Direction.Down:
                    Dx = 0;
                    Dy = Speed;
                    break;
                case Direction.Right:
                    Dx = Speed;
                    Dy = 0;
                    break;
                default:
                    break;
            }
        }
    }
}