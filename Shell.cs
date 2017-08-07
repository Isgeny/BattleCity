using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Shell : MovingObject
    {
        public Tank Creator { get; private set; }

        public Shell(GUIForm guiForm, Tank creator) : base(guiForm)
        {
            Speed = 8;

            Creator = creator;

            if(creator is PlayerTank && creator.Stars >= 1 || creator is CompTank && creator.Stars == 2)
                Speed = 15;

            Direction = Creator.Direction;

            switch(Direction)
            {
                case Direction.Up:
                    Dx = 0;
                    Dy = -Speed;
                    Rect = new Rectangle(creator.Rect.Left + 24, creator.Rect.Top - 16, 16, 16);
                    break;
                case Direction.Left:
                    Dx = -Speed;
                    Dy = 0;
                    Rect = new Rectangle(creator.Rect.Left - 16, creator.Rect.Top + 24, 16, 16);
                    break;
                case Direction.Down:
                    Dx = 0;
                    Dy = Speed;
                    Rect = new Rectangle(creator.Rect.Left + 24, creator.Rect.Bottom, 16, 16);
                    break;
                case Direction.Right:
                    Dx = Speed;
                    Dy = 0;
                    Rect = new Rectangle(creator.Rect.Right, creator.Rect.Top + 24, 16, 16);
                    break;
                default:
                    break;
            }

            ExplosionTimer = new Timer();
            ExplosionTimer.Interval = 40;
            ExplosionTimer.Tick += OnExplosionTimerTick;
            ExplosionFrame = 0;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            MoveTimer.Tick += OnMoveTimerTick;
            MoveTimer.Start();
            Destroyed += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            MoveTimer.Tick -= OnMoveTimerTick;
            MoveTimer.Stop();
            Destroyed -= OnDestroyed;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if(Rect.IntersectsWith(e.ClipRectangle) && !ExplosionTimer.Enabled)
            {
                var rm = Properties.Resources.ResourceManager;
                var filename = "Shell_" + (int)Direction;
                var bmp = rm.GetObject(filename);
                e.Graphics.DrawImageUnscaled(bmp as Bitmap, Rect);
            }
            else if(ExplosionTimer.Enabled)
                e.Graphics.DrawImage(Properties.Resources.Bullet_Explosion, new Rectangle(Rect.X - 20, Rect.Y - 20, 64, 64), new Rectangle(ExplosionFrame, 0, 64, 64), GraphicsUnit.Pixel);
        }

        private void OnMoveTimerTick(object sender, EventArgs e)
        {
            CalcDxDyAlongDirection();
            InvokeCheckPosition(new RectEventArgs(Rect, new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            Rect = new Rectangle(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height);
            GUIForm.Invalidate(Rectangle.Inflate(Rect, Dx*4, Dy*4));
        }

        private void OnDestroyed(object sender, EventArgs e)
        {
            MoveTimer.Stop();
            ExplosionTimer.Start();
        }

        private void OnExplosionTimerTick(object sender, EventArgs e)
        {
            ExplosionFrame += 64;
            if(ExplosionFrame == 192)
            {
                ExplosionTimer.Stop();
                ExplosionTimer.Tick -= OnExplosionTimerTick;
                Unsubscribe();
                Creator.Ammo++;
            }
            GUIForm.Invalidate(Rectangle.Inflate(Rect, 64, 64));
        }

        protected override void ShellCollision(Shell shell)
        {
            base.ShellCollision(shell);
            InvokeDestroyed();
        }
    }
}