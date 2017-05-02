using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Shell : DynamicObject
    {
        public Tank Creator { get; private set; }
        private Timer _explosionTimer;
        private int _explosionFrame;

        public Shell(GUIForm guiForm, Tank creator) : base(guiForm, new Rectangle(), creator.Direction)
        {
            Creator = creator;
            MoveTimer.Tick += OnMoveTimer;
            MoveTimer.Start();

            Speed = 7;
            if(creator is PlayerTank && creator.Stars >= 1 || creator is CompTank && creator.Stars == 2)
                Speed = 10;

            switch(Direction)
            {
                case Direction.Up:
                    Dx = 0;
                    Dy = -Speed;
                    Rect = new Rectangle(creator.Rect.X + 22, creator.Rect.Y - 6, 16, 16);
                    break;
                case Direction.Left:
                    Dx = -Speed;
                    Dy = 0;
                    Rect = new Rectangle(creator.Rect.X - 6, creator.Rect.Y + 22, 16, 16);
                    break;
                case Direction.Down:
                    Dx = 0;
                    Dy = Speed;
                    Rect = new Rectangle(creator.Rect.X + 22, creator.Rect.Y + 50, 16, 16);
                    break;
                case Direction.Right:
                    Dx = Speed;
                    Dy = 0;
                    Rect = new Rectangle(creator.Rect.X + 50, creator.Rect.Y + 22, 16, 16);
                    break;
                default:
                    break;
            }

            _explosionTimer = new Timer();
            _explosionTimer.Interval = 50;
            _explosionTimer.Tick += OnExplosionTimer;
            _explosionFrame = 0;
        }

        public override void Subscribe()
        {
            GUIForm.Paint   += OnPaint;
            Destroyed       += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint   -= OnPaint;
            Destroyed       -= OnDestroyed;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                var g = e.Graphics;
                if(!_explosionTimer.Enabled)
                {
                    var rm = Properties.Resources.ResourceManager;
                    string filename = "Shell_" + (int)Direction;
                    var bmp = rm.GetObject(filename);
                    g.DrawImageUnscaled(bmp as Bitmap, Rect);
                }
                else
                    g.DrawImage(Properties.Resources.Bullet_Explosion, new Rectangle(Rect.X - 20, Rect.Y - 20, 64, 64), new Rectangle(_explosionFrame, 0, 64, 64), GraphicsUnit.Pixel);
            }   
        }

        protected override void TankCollision(Tank tank)
        {
            if(tank is PlayerTank && Creator is CompTank || tank is CompTank && Creator is PlayerTank || tank is PlayerTank && Creator is PlayerTank && Properties.Settings.Default.FriendlyFire)
            {
                if(tank is CompTank)
                {
                    var compTank = tank as CompTank;
                    if(compTank.IsBonus && !compTank.Immortal)
                    {
                        compTank.InvokeBonusShoot();
                        compTank.IsBonus = false;
                    }
                }
                tank.HP--;
                InvokeDestroyed();
            }
            else if(tank is CompTank && Creator is CompTank)
                InvokeDestroyed(); ;
        }

        protected override void ShellCollision(Shell shell)
        {
            base.ShellCollision(shell);
            InvokeDestroyed();
        }

        private void OnMoveTimer(object sender, EventArgs e)
        {
            InvokeCheckPosition(new RectEventArgs(new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            Move();
        }

        protected override void OnDestroyed(object sender, EventArgs e)
        {
            base.OnDestroyed(sender, e);
            _explosionTimer.Start();
            if(MoveTimer.Enabled)
            {
                MoveTimer.Stop();
                Creator.Ammo++;
            }
        }

        private void OnExplosionTimer(object sender, EventArgs e)
        {
            _explosionFrame = _explosionFrame + 64;
            if(_explosionFrame == 192)
            {
                _explosionTimer.Stop();
                _explosionTimer.Tick -= OnExplosionTimer;
                Unsubscribe();
            }
            GUIForm.Invalidate(Rectangle.Inflate(Rect, 64, 64));
        }
    }
}