using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class CompTank : Tank
    {
        public bool IsBonus { get; set; }

        public event EventHandler BonusShoot;
        
        public CompTank(GUIForm guiForm, bool isBonus) : base(guiForm, new Rectangle(), Direction.Down)
        {
            IsBonus = isBonus;
            InitializeTank();
            ImmortalDelayTimer.Interval = 10000;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            MoveTimer.Tick += OnMoveTimerTick;
            MoveTimer.Start();
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            MoveTimer.Tick -= OnMoveTimerTick;
            MoveTimer.Stop();
        }

        public override int Stars
        {
            get { return base.Stars; }
            set
            {
                base.Stars = value;
                switch(Stars)
                {
                    case 0:
                        Points = 100;
                        Speed = 2;
                        break;
                    case 1:
                        Points = 200;
                        Speed = 4;
                        break;
                    case 2:
                        Points = 300;
                        Speed = 2;
                        break;
                    case 3:
                        Points = 400;
                        Speed = 2;
                        HP = 4;
                        break;
                    default:
                        break;
                }
            }
        }

        public override int HP
        {
            get { return base.HP; }
            set
            {
                if(!Immortal)
                    base.HP = value;
            }
        }

        protected override int GetCurrentSpriteFrame()
        {
            int currentFrame = ((Rect.X + Rect.Y) % 8 < 4) ? 0 : 64;
            if(Stars == 3 && HP < 4)
                currentFrame += 256 + (4 - HP) * 64 * 2;
            if(IsBonus && DateTime.Now.Millisecond % 200 < 100)
                currentFrame = ((Rect.X + Rect.Y) % 8 < 4) ? 128 : 192;
            return currentFrame;
        }

        protected virtual void OnMoveTimerTick(object sender, EventArgs e)
        {
            if(!RespawnTimer.Enabled)
            {
                if(Dx == 0 && Dy == 0 && GameRandom.RandNumber(0, 10) == 0)
                    ChangeDirection();
                Turn();
                CheckTileReach();
                MakeShoot();
                InvokeCheckPosition(new RectEventArgs(new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
                Move();
            }
        }

        private void CheckTileReach()
        {
            if(Rect.X % 64 == 0 && Rect.Y % 64 == 0 && GameRandom.RandNumber(0, 16) == 0)
                ChangeDirection();
            else if(!MoveTimer.Enabled && GameRandom.RandNumber(0, 16) == 0)
                if(Rect.X % 64 != 0 || Rect.Y % 64 != 0)
                    InvertDirection();
                else
                    Rotate();
        }

        private void Rotate()
        {
            if(GameRandom.RandNumber(0, 1) == 0)
                ChangeDirection();
            else if(GameRandom.RandNumber(0, 1) == 0)
                RotateClockwise();
            else
                RotateAnticlockwise();
        }

        private void RotateClockwise()
        {
            Direction = (Direction == Direction.Up) ? Direction.Right : Direction--;
        }
   
        private void RotateAnticlockwise()
        {
            Direction = (Direction == Direction.Right) ? Direction.Up : Direction++;
        }

        public void ChangeDirection()
        {
            Direction = (Direction)GameRandom.RandNumber(0, 3);
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
            GUIForm.Invalidate(Rect);
        }

        private void InvertDirection()
        {
            int temp = Dx;
            Dx = Dy;
            Dy = temp;
            switch(Direction)
            {
                case Direction.Up:
                    Direction = Direction.Down;
                    break;
                case Direction.Left:
                    Direction = Direction.Right;
                    break;
                case Direction.Down:
                    Direction = Direction.Up;
                    break;
                case Direction.Right:
                    Direction = Direction.Left;
                    break;
                default:
                    break;
            }
        }

        public override void InitializeTank()
        {
            //Respawn();
            HP = 1;
            Lives = 0;
            Immortal = false;
            Amphibian = false;
            Gun = false;
            Ammo = 1;
            switch(GameRandom.RandNumber(1, 4))
            {
                case 1:
                    Stars = 0;
                    Points = 100;
                    Speed = 2;
                    break;
                case 2:
                    Stars = 1;
                    Points = 200;
                    Speed = 4;
                    break;
                case 3:
                    Stars = 2;
                    Points = 300;
                    Speed = 2;
                    break;
                case 4:
                    HP = 4;
                    Stars = 3;
                    Points = 400;
                    Speed = 2;
                    break;
                default:
                    break;
            }
        }

        private void MakeShoot()
        {
            if(GameRandom.RandNumber(0, 75) == 0 && Ammo > 0)
            {
                Ammo--;
                Shell shell = new Shell(GUIForm, this);
                InvokeShoot(new ShellEventArgs(shell));
            }
        }

        public override void Respawn()
        {
            switch(GameRandom.RandNumber(1, 3))
            {
                case 1:
                    Rect = new Rectangle(64, 64, 64, 64);
                    break;
                case 2:
                    Rect = new Rectangle(448, 64, 64, 64);
                    break;
                case 3:
                    Rect = new Rectangle(832, 64, 64, 64);
                    break;
                default:
                    break;
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Tank)
                {
                    var tank = sender as Tank;
                    tank.StopMoving();
                    if(sender is CompTank)
                        StopMoving();
                }
                /*else if(sender is Shell)
                {
                    var shell = sender as Shell;
                    if(shell.Creator is PlayerTank)
                    {
                        if(IsBonus && !Immortal)
                        {
                            InvokeBonusShoot();
                            IsBonus = false;
                        }
                        HP--;
                        InvokeDestroyed();
                    }
                    shell.InvokeDestroyed();
                }*/
            }
        }

        protected override void OnRespawnDelayTimer(object sender, EventArgs e)
        {
            base.OnRespawnDelayTimer(sender, e);
            Immortal = false;
        }

        public void InvokeBonusShoot()
        {
            BonusShoot?.Invoke(this, new EventArgs());
        }
    }
}