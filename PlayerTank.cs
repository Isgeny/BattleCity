using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class PlayerTank : Tank
    {
        public PlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, Direction.Up)
        {
            MoveTimer = new Timer();
            MoveTimer.Interval = 1000 / 60;
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
            InvokeCheckPosition(new RectEventArgs(new RectangleF(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            Move();
            Turn(Direction);
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