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

        public PlayerTank(GUIForm guiForm, Rectangle rect) : base(guiForm, rect, Direction.Up)
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

        public override int HP
        {
            get { return base.HP; }
            set
            {
                if(!Immortal)
                {
                    base.HP = value;
                    if(base.HP == 0)
                    {
                        ShowRespawnAnimation();
                        GUIForm.Invalidate(Rect);
                        Respawn();
                        Stars = 0;
                        Ammo = 1;
                        Amphibian = false;
                        Gun = false;
                        GUIForm.Invalidate(Rect);
                    }
                }
            }
        }

        public override int Stars
        {
            get { return base.Stars; }
            set
            {
                base.Stars = value;
                if(Stars == 2)
                    Ammo++;
            }
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.KeyUp += OnKeyUp;
            MoveTimer.Tick += OnMoveTimerTick;
            MoveTimer.Start();
            ShowRespawnAnimation();
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
            Turn();
            if(_iceTicks != 0)
            {
                _iceTicks--;
                if(_onIce)
                {
                    switch(Direction)
                    {
                        case Direction.Up:
                            Dx = 0;
                            Dy = -3;
                            break;
                        case Direction.Left:
                            Dx = -3;
                            Dy = 0;
                            break;
                        case Direction.Down:
                            Dx = 0;
                            Dy = 3;
                            break;
                        case Direction.Right:
                            Dx = 3;
                            Dy = 0;
                            break;
                    }
                    _onIce = false;
                }
            }
            InvokeCheckPosition(new RectEventArgs(new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
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

        protected override void OnRespawnDelayTimer(object sender, EventArgs e)
        {
            base.OnRespawnDelayTimer(sender, e);
            ImmortalDelayTimer.Interval = 4000;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _shootPressed = false;
        }

        public override void InitializeTank()
        {
            HP = 1;
            Lives = 2;
            Stars = 0;
            Amphibian = false;
            Gun = false;
            Ammo = 1;
            Points = 0;
            _iceTicks = 28;
            _onIce = false;
            _shootPressed = false;
            Respawn();
        }

        public override void Respawn()
        {
            base.Respawn();   
            Direction = Direction.Up;
        }
    }
}