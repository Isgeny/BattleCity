using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class PlayerTank : Tank
    {
        private int _iceTicks;
        private bool _onIce;
        private bool _shootPressed;

        public PlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, Direction.Up)
        {
            _iceTicks = 28;
            _onIce = false;
            _shootPressed = false;
        }

        public int IceTicks
        {
            get { return _iceTicks; }
            set { _iceTicks = value; }
        }

        public bool OnIce
        {
            get { return _onIce; }
            set { _onIce = value; }
        }

        public bool ShootPressed
        {
            get { return _shootPressed; }
            set { _shootPressed = value; }
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.KeyUp += OnKeyUp;
            MoveTimer.Tick += OnMoveTimerTick;
            MoveTimer.Start();
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.KeyUp -= OnKeyUp;
            MoveTimer.Tick -= OnMoveTimerTick;
            MoveTimer.Stop();
        }

        protected virtual void OnMoveTimerTick(object sender, EventArgs e)
        {
            Turn(Direction);
            if(_iceTicks != 0)
            {
                _iceTicks--;
                if(_onIce)
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
                    _onIce = false;
                }
            }
            InvokeCheckPosition(new RectEventArgs(new RectangleF(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            Move();
        }

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

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _shootPressed = false;
        }
    }
}