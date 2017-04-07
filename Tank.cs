using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Tank : DynamicObject
    {
        private int _hp;
        private int _lives;
        private int _stars;
        private bool _immortal;
        private bool _amphibian;
        private bool _gun;
        private int _ammo;
        private int _points;

        public Tank(GUIForm guiForm, RectangleF rect, Direction direction) : base(guiForm, rect, Direction.Up)
        {
            _hp = 1;
            _lives = 1;
            _stars = 0;
            _immortal = false;
            _amphibian = false;
            _gun = false;
            _ammo = 1;
            _points = 0;
        }

        public int HP
        {
            get { return _hp; }
            set { _hp = value; }
        }

        public int Lives
        {
            get { return _lives; }
            set { _lives = value; }
        }

        public int Stars
        {
            get { return _stars; }
            set { _stars = value; }
        }

        public bool Immortal
        {
            get { return _immortal; }
            set { _immortal = value; }
        }

        public bool Amphibian
        {
            get { return _amphibian; }
            set { _amphibian = value; }
        }

        public bool Gun
        {
            get { return _gun; }
            set { _gun = value; }
        }

        public int Ammo
        {
            get { return _ammo; }
            set { _ammo = value; }
        }

        public int Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                float currentFrame = ((Rect.X + Rect.Y) % 8 < 4) ? 0.0f : 64.0f;
                Bitmap bmp = GetCurrentSprite();

                g.DrawImage(bmp, Rect, new RectangleF(currentFrame, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);

                if(Amphibian)
                {
                    g.DrawImage(Properties.Resources.Ship_Shield, Rect.X, Rect.Y);
                }
            }
        }

        protected abstract void Respawn();

        private Bitmap GetCurrentSprite()
        {
            ResourceManager rm = Properties.Resources.ResourceManager;
            string filename = GetType().Name + "_" + Stars + "_" + (int)Direction;
            Bitmap bmp = (Bitmap)rm.GetObject(filename);
            return bmp;
        }

        protected void Turn(Direction direction)
        {
            //Округление координат перед поворотом
            RectangleF oldRect = Rect;
            RectangleF newRect;
            if(Direction == Direction.Up || Direction == Direction.Down)
            {
                if(Direction == Direction.Right)
                {
                    float newX = (float)Math.Round(Rect.Left / (Rect.Height / 2.0f)) * Rect.Height / 2.0f;
                    newRect = new RectangleF(newX, Rect.Y, Rect.Width, Rect.Height);
                    Rect = newRect;
                }
                else
                {
                    float newX = (float)Math.Round(Rect.Right / (Rect.Height / 2.0f)) * Rect.Height / 2.0f;
                    newRect = new RectangleF(newX - Rect.Width, Rect.Y, Rect.Width, Rect.Height);
                    Rect = newRect;
                }
            }
            else
            {
                if(Direction == Direction.Up)
                {
                    float newY = (float)Math.Round(Rect.Bottom / (Rect.Height / 2.0f)) * Rect.Height / 2.0f;
                    newRect = new RectangleF(Rect.X, newY - Rect.Height, Rect.Width, Rect.Height);
                    Rect = newRect;
                }
                else
                {
                    float newY = (float)Math.Round(Rect.Top / (Rect.Height / 2.0f)) * Rect.Height / 2.0f;
                    newRect = new RectangleF(Rect.X, newY, Rect.Width, Rect.Height);
                    Rect = newRect;
                }
            }
            Rect = newRect;
            GUIForm.Invalidate(new Region(oldRect));
            GUIForm.Invalidate(new Region(newRect));
            //Поворот танка
            Direction = direction;
        }

        protected void Die()
        {
            _lives--;
            if(_lives < 0)
            {
                InvokeDestroyed();
            }
        }

        public bool IsAlive()
        {
            return (_lives >= 0) ? true : false;
        }

        public event ShellEventHandler Shoot;

        protected void InvokeShoot(ShellEventArgs e)
        {
            Shoot?.Invoke(this, e);
        }
    }
}