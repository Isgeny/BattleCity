using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Obstacle : Object
    {
        private int resistance;
        private bool passable;

        public Obstacle(GUIForm guiForm, RectangleF rect, int resistance, bool passable) : base(guiForm, rect)
        {
            throw new System.NotImplementedException();
        }

        public int Resistance
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public bool Passable
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public abstract void ShellCollision(Shell shell);
    }
}