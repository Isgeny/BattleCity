using System;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;
using System.Collections.Generic;

namespace BattleCity
{
    public class Shell : DynamicObject
    {
        private Tank _creator;
        private Timer _explosionTimer;
        private int _explosionFrame;

        public Shell(GUIForm guiForm, Tank creator) : base(guiForm, new Rectangle(), creator.Direction)
        {
            _creator = creator;
            MoveTimer.Tick += OnMoveTimer;
            MoveTimer.Start();

            int speed = 7;
            if(creator is PlayerTank && creator.Stars >= 1 || creator is CompTank && creator.Stars == 2)
                speed = 10;

            switch(Direction)
            {
                case Direction.Up:
                    Dx = 0;
                    Dy = -speed;
                    Rect = new Rectangle(creator.Rect.X + 22, creator.Rect.Y - 6, 16, 16);
                    break;
                case Direction.Left:
                    Dx = -speed;
                    Dy = 0;
                    Rect = new Rectangle(creator.Rect.X - 6, creator.Rect.Y + 22, 16, 16);
                    break;
                case Direction.Down:
                    Dx = 0;
                    Dy = speed;
                    Rect = new Rectangle(creator.Rect.X + 22, creator.Rect.Y + 50, 16, 16);
                    break;
                case Direction.Right:
                    Dx = speed;
                    Dy = 0;
                    Rect = new Rectangle(creator.Rect.X + 50, creator.Rect.Y + 22, 16, 16);
                    break;
            }

            _explosionTimer = new Timer();
            _explosionTimer.Interval = 50;
            _explosionTimer.Tick += OnExplosionTimer;
            _explosionFrame = 0;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            Destroyed += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            Destroyed -= OnDestroyed;
        }

        public Tank Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Rectangle clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                if(!_explosionTimer.Enabled)
                {
                    ResourceManager rm = Properties.Resources.ResourceManager;
                    string filename = "Shell_" + (int)Direction;
                    Bitmap bmp = (Bitmap)rm.GetObject(filename);
                    g.DrawImage(bmp, Rect, new Rectangle(0, 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
                }
                else
                    g.DrawImage(Properties.Resources.Bullet_Explosion, new Rectangle(Rect.X - 20, Rect.Y - 20, 64, 64), new Rectangle(_explosionFrame, 0, 64, 64), GraphicsUnit.Pixel);
            }   
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Shell)
                {
                    var shell = sender as Shell;
                    InvokeDestroyed();
                    shell.InvokeDestroyed();
                }
                else if(sender is PlayerTank && _creator is CompTank || sender is CompTank && _creator is PlayerTank || sender is PlayerTank && _creator is PlayerTank && Properties.Settings.Default.FriendlyFire)
                {
                    if(sender is CompTank && ((CompTank)sender).IsBonus && !((CompTank)sender).Immortal)
                    {
                        ((CompTank)sender).InvokeBonusShoot();
                        ((CompTank)sender).IsBonus = false;
                    }
                    ((Tank)sender).HP--;
                    InvokeDestroyed();
                }
                else if(sender is Tank)
                    InvokeDestroyed();
            }
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
                _creator.Ammo++;
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