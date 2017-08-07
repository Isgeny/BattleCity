using System;
using System.Drawing;

namespace BattleCity
{
    public abstract class PlayerTank : Tank
    {
        public override int HP
        {
            get { return base.HP; }
            set
            {
                if(!Immortal && !RespawnTimer.Enabled)
                {
                    base.HP = value;
                    if(base.HP == 0)
                        GUIForm.KeyDown -= OnKeyDown;
                }
            }
        }

        public override int Stars
        {
            get { return base.Stars; }
            set
            {
                base.Stars = value;
                if(base.Stars == 2)
                    ++Ammo;
            }
        }

        public override int Ammo
        {
            get { return base.Ammo; }
            set
            {
                switch(Stars)
                {
                    case 0:
                    case 1:
                        base.Ammo = (value > 1) ? 1 : value;
                        break;
                    case 2:
                    case 3:
                        base.Ammo = (value > 2) ? 2 : value;
                        break;
                    default:
                        break;
                }
            }
        }

        public int IceTicks { get; set; }
        public bool OnIce { get; set; }

        public PlayerTank(GUIForm guiForm) : base(guiForm)
        {
            Speed = 3;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            MoveTimer.Tick += OnMoveTimerTick;
            GUIForm.KeyUp += OnKeyUp;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            MoveTimer.Tick -= OnMoveTimerTick;
            GUIForm.KeyUp -= OnKeyUp;
            GUIForm.KeyDown -= OnKeyDown;
        }

        protected override int GetCurrentSpriteFrame()
        {
            int currentFrame = base.GetCurrentSpriteFrame();
            if(OnIce && IceTicks != 28 - 1)
                currentFrame = 0;
            return currentFrame;
        }

        protected override void ShellCollision(Shell shell)
        {
            base.ShellCollision(shell);
            if(shell.Creator is CompTank || shell.Creator is PlayerTank && Properties.Settings.Default.FriendlyFire)
                HP--;
        }

        private bool _shotPressed = false;
        protected virtual void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(Ammo > 0 && !_shotPressed && !RespawnTimer.Enabled && !ExplosionTimer.Enabled)
            {
                Ammo--;
                var shell = new Shell(GUIForm, this);
                shell.Subscribe();
                InvokeShot(new ShellEventArgs(shell));
                _shotPressed = true;
            }
        }

        private void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            _shotPressed = false;
        }

        protected override void OnRespawnTimerTick(object sender, EventArgs e)
        {
            base.OnRespawnTimerTick(sender, e);
            if(RespawnFrame == 0)
                GUIForm.KeyDown += OnKeyDown;
        }

        protected override void OnExplosionTimerTick(object sender, EventArgs e)
        {
            base.OnExplosionTimerTick(sender, e);
            if(ExplosionFrame == 0 && Lives >= 0)
            {
                Stars = 0;
                MoveToStartPosition();
                RespawnTimer.Start();
                Direction = Direction.Up;
            }
        }

        protected abstract bool UpPressed();
        protected abstract bool LeftPressed();
        protected abstract bool DownPressed();
        protected abstract bool RightPressed();

        private void OnMoveTimerTick(object sender, EventArgs e)
        {
            Dx = 0;
            Dy = 0;

            if(UpPressed())
            {
                Dx = 0;
                Dy = -Speed;
                Direction = Direction.Up;
            }
            else if(LeftPressed())
            {
                Dx = -Speed;
                Dy = 0;
                Direction = Direction.Left;
            }
            else if(DownPressed())
            {
                Dx = 0;
                Dy = Speed;
                Direction = Direction.Down;
            }
            else if(RightPressed())
            {
                Dx = Speed;
                Dy = 0;
                Direction = Direction.Right;
            }

            if(Dx != 0 || Dy != 0)
                IceTicks = 28;

            RoundCoordsOnTurning();

            if(IceTicks != 0)
            {
                IceTicks--;
                if(OnIce)
                {
                    CalcDxDyAlongDirection();
                    OnIce = false;
                }
            }

            InvokeCheckPosition(new RectEventArgs(Rect, new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            if(Dx != 0 || Dy != 0)
                Rect = new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height);
        }

        public override void SetNewGameParameters()
        {
            MoveToStartPosition();
            Dx = 0;
            Dy = 0;
            Direction = Direction.Up;
            HP = 1;
            Lives = 2;
            Stars = 0;
            Immortal = false;
            Amphibian = false;
            Gun = false;
            Ammo = 1;
            Points = 0;
            MoveTimer.Stop();
        }

        public override void SetNewStageParameters()
        {
            MoveToStartPosition();
            Dx = 0;
            Dy = 0;
            Direction = Direction.Up;
            Immortal = false;
            MoveTimer.Stop();
            RespawnTimer.Start();
        }
    }
}