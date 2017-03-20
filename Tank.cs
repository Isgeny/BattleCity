using System;
using System.Drawing;
using System.Threading;
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

        public event EventHandler Destroyed;

        public Tank(GUIForm guiForm, RectangleF rect, float speed, Direction direction) : base(guiForm, rect, Direction.Up)
        {
            throw new System.NotImplementedException();
        }

        public int HP
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public int Lives
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public int Stars
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public bool Immortal
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public bool Amphibian
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public bool Gun
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public int Ammo
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public int Points
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        protected void Turn(Direction direction)
        {
            throw new System.NotImplementedException();
        }

        protected abstract void Respawn();

        protected void Die()
        {
            throw new System.NotImplementedException();
        }

        public bool IsAlive()
        {
            throw new System.NotImplementedException();
        }

        protected void Shoot()
        {
            throw new System.NotImplementedException();
        }
    }
}