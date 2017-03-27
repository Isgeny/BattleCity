using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class PlayerTank : Tank
    {
        private int iceTicks;
        private bool onIce;

        public PlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, Direction.Up)
        {
            iceTicks = 28;
            onIce = false;
        }

        public bool OnIce
        {
            get { return onIce; }
            set { onIce = value; }
        }

        public int IceTicks
        {
            get { return iceTicks; }
            set { iceTicks = value; }
        }

        public override void SubscribeToPaint()
        {
            base.SubscribeToPaint();

            MoveTimer.Tick += OnMoveTimerTick;
            MoveTimer.Start();
        }

        public override void UnsubscribeFromPaint()
        {
            base.UnsubscribeFromPaint();

            MoveTimer.Tick -= OnMoveTimerTick;
            MoveTimer.Stop();
        }

        protected virtual void OnMoveTimerTick(object sender, EventArgs e)
        {
            Turn(Direction);
            if(iceTicks != 0)
            {
                iceTicks--;
                if(onIce)
                {
                    switch(Direction)
                    {
                        case Direction.Up:
                            Dx = 0.0f;
                            Dy = -2.75f;
                            break;
                        case Direction.Left:
                            Dx = -2.75f;
                            Dy = 0.0f;
                            break;
                        case Direction.Down:
                            Dx = 0.0f;
                            Dy = 2.75f;
                            break;
                        case Direction.Right:
                            Dx = 2.75f;
                            Dy = 0.0f;
                            break;
                    }
                    onIce = false;
                }
            }
            InvokeCheckPosition(new RectEventArgs(new RectangleF(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            Move();
        }

        protected abstract Bitmap GetCurrentSprite();

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Tank)
                {
                    ((Tank)sender).StopMoving();
                }
            }
        }
    }
}