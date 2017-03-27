using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Tank : DynamicObject
    {
        private int hp;
        private int lives;
        private int stars;
        private bool immortal;
        private bool amphibian;
        private bool gun;
        private int ammo;
        private int points;

        public Tank(GUIForm guiForm, RectangleF rect, Direction direction) : base(guiForm, rect, Direction.Up)
        {
            hp = 1;
            lives = 1;
            stars = 0;
            immortal = false;
            amphibian = false;
            gun = false;
            ammo = 1;
            points = 0;
        }

        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public int Stars
        {
            get { return stars; }
            set { stars = value; }
        }

        public bool Immortal
        {
            get { return immortal; }
            set { immortal = value; }
        }

        public bool Amphibian
        {
            get { return amphibian; }
            set { amphibian = value; }
        }

        public bool Gun
        {
            get { return gun; }
            set { gun = value; }
        }

        public int Ammo
        {
            get { return ammo; }
            set { ammo = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
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

        protected abstract void Respawn();

        protected void Die()
        {
            lives--;
            if(lives < 0)
            {
                InvokeDestroyed(new EventArgs());
            }
        }

        public bool IsAlive()
        {
            return (lives >= 0) ? true : false;
        }

        public event EventHandler Destroyed;

        protected void InvokeDestroyed(EventArgs e)
        {
            Destroyed?.Invoke(this, e);
        }

        public event ShellEventHandler Shoot;

        protected void InvokeShoot(ShellEventArgs e)
        {
            Shoot?.Invoke(this, e);
        }
    }
}