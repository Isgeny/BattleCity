using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class CompTank : Tank
    {
        public override int Speed
        {
            get { return base.Speed; }
            set
            {
                base.Speed = value;
                CalcDxDyAlongDirection();
            }
        }

        public override Direction Direction
        {
            get { return base.Direction; }
            set
            {
                base.Direction = value;
                CalcDxDyAlongDirection();
                GUIForm.Invalidate(Rect);
            }
        }

        public override int Lives
        {
            get { return base.Lives; }
            set
            {
                base.Lives = value;
                if(base.Lives >= 0)
                    Direction = Direction.Down;
            }
        }

        public override int Stars
        {
            get { return base.Stars; }
            set
            {
                base.Stars = value;
                switch(base.Stars)
                {
                    case 0:
                        HP = 1;
                        Points = 100;
                        Speed = 2;
                        break;
                    case 1:
                        HP = 1;
                        Points = 200;
                        Speed = 4;
                        break;
                    case 2:
                        HP = 1;
                        Points = 300;
                        Speed = 2;
                        break;
                    case 3:
                        HP = 4;
                        Points = 400;
                        Speed = 2;
                        break;
                    default:
                        break;
                }
                GUIForm.Invalidate(Rect);
            }
        }

        public override int Ammo
        {
            get { return base.Ammo; }
            set
            {
                base.Ammo = (value > 1) ? 1 : value;
            }
        }

        public bool IsBonus { get; set; }
        private Timer _bonusFrameUpdateTimer;
        private Timer _pointsTimer;

        public event EventHandler BonusShoot;

        public CompTank(GUIForm guiForm, bool isBonus) : base(guiForm)
        {
            IsBonus = isBonus;

            _bonusFrameUpdateTimer = new Timer();
            _bonusFrameUpdateTimer.Interval = 100;
            _bonusFrameUpdateTimer.Tick += OnBonusFrameUpdateTimerTick;
            _bonusFrameUpdateTimer.Enabled = IsBonus;

            _pointsTimer = new Timer();
            _pointsTimer.Interval = 300;
            _pointsTimer.Tick += OnPointsTimerTick;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            MoveTimer.Tick += OnMoveTimerTick;
            GUIForm.KeyDown += OnKeyDown;           //DEBUG
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            MoveTimer.Tick -= OnMoveTimerTick;
            GUIForm.KeyDown -= OnKeyDown;           //DEBUG
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            if(_pointsTimer.Enabled)
                e.Graphics.DrawString(Points.ToString(), MyFont.GetFont(12), Brushes.White, Rect.X, Rect.Y + 20);
            else
                base.OnPaint(sender, e);
        }

        protected override int GetCurrentSpriteFrame()
        {
            int currentFrame = base.GetCurrentSpriteFrame();
            if(IsBonus && DateTime.Now.Millisecond % 200 < 100)
                currentFrame = ((Rect.X + Rect.Y) % 8 < 4) ? 128 : 192;
            if(Stars == 3 && HP <= 4 && !IsBonus)
                currentFrame += 256 + (4 - HP) * 64 * 2;
            return currentFrame;
        }

        protected override void ShellCollision(Shell shell)
        {
            base.ShellCollision(shell);
            if(shell.Creator is PlayerTank && !ExplosionTimer.Enabled && !_pointsTimer.Enabled)
            {
                if(IsBonus && !Immortal)
                {
                    InvokeBonusShoot();
                    IsBonus = false;
                }
                HP--;
                if(HP == 0)
                    shell.Creator.Points += Points;
            }
        }

        //DEBUG
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Z)
            {
                Stars++;
            }
            else if(e.KeyCode == Keys.X)
            {
                Amphibian = !Amphibian;
            }
            else if(e.KeyCode == Keys.C)
            {
                Gun = !Gun;
            }
            else if(e.KeyCode == Keys.V)
            {
                HP--;
            }
            else if(e.KeyCode == Keys.B)
            {
                Immortal = !Immortal;
            }
        }

        private void InvokeBonusShoot()
        {
            BonusShoot?.Invoke(this, new EventArgs());
        }

        public void RemoveBonusShootListeners()
        {
            BonusShoot = null;
        }

        protected override void OnExplosionTimerTick(object sender, EventArgs e)
        {
            base.OnExplosionTimerTick(sender, e);
            if(ExplosionFrame == 0)
            {
                GUIForm.Paint += OnPaint;
                _pointsTimer.Start();
                GUIForm.Invalidate(Rect);
            }
        }

        private void OnMoveTimerTick(object sender, EventArgs e)
        {
            RoundCoordsOnTurning();
            CheckTileReach();
            MakeShoot();
            InvokeCheckPosition(new RectEventArgs(Rect, new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            if(Dx != 0 || Dy != 0)
                Rect = new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height);
        }

        private void OnPointsTimerTick(object sender, EventArgs e)
        {
            _pointsTimer.Stop();
            GUIForm.Invalidate(Rect);
            GUIForm.Paint -= OnPaint;
        }

        private void CheckTileReach()
        {
            if(Rect.X % 64 == 0 && Rect.Y % 64 == 0 && GameRandom.RandNumber(0, 64) == 0)
                ChangeDirection();
            else if(Dx == 0 && Dy == 0 && GameRandom.RandNumber(0, 16) == 0)
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
        }

        private void InvertDirection()
        {
            Direction = (Direction)((int)(Direction + 2) % 4);
        }

        private void MakeShoot()
        {
            if(GameRandom.RandNumber(0, 64) == 0 && Ammo > 0)
            {
                Ammo--;
                var shell = new Shell(GUIForm, this);
                shell.Subscribe();
                InvokeShot(new ShellEventArgs(shell));
            }
        }

        protected override void MoveToStartPosition()
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

        public override void SetNewGameParameters()
        {
            MoveToStartPosition();
            Direction = Direction.Down;
            Lives = 0;
            Stars = GameRandom.RandNumber(0, 3);
            Immortal = false;
            Amphibian = false;
            Gun = false;
            Ammo = 1;
            MoveTimer.Stop();
        }

        public override void SetNewStageParameters()
        {
            MoveToStartPosition();
            Direction = Direction.Down;
            Lives = 0;
            Stars = GameRandom.RandNumber(0, 3);
            Immortal = false;
            Amphibian = false;
            Gun = false;
            MoveTimer.Stop();
            RespawnTimer.Start();
        }

        private void OnBonusFrameUpdateTimerTick(object sender, EventArgs e)
        {
            GUIForm.Invalidate(Rect);
        }
    }
}