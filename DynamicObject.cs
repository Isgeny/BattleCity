using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class DynamicObject : Object
    {
        private System.Threading.Timer moveTimer;
        private float dx;
        private float dy;
        private Direction direction;

        public event BattleCity.RectEventHandler CheckPosition;

        public DynamicObject(GUIForm guiForm, RectangleF rect, float speed, Direction direction)
        {
            throw new System.NotImplementedException();
        }

        public System.Threading.Timer MoveTimer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set { }
        }

        public float Speed
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public float Dx
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public float Dy
        {
            get => default(int);
            set
            {
            }
        }

        public Direction Direction
        {
            get => default(Direction);
            set
            {
            }
        }

        public virtual void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}