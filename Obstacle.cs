using System.Drawing;

namespace BattleCity
{
    public abstract class Obstacle : Object
    {
        private int resistance;
        private bool passable;

        public Obstacle(GUIForm guiForm, RectangleF rect, int resistance, bool passable = false) : base(guiForm, rect)
        {
            this.resistance = resistance;
            this.passable = passable;
        }

        public int Resistance
        {
            get { return resistance; }
        }

        public bool Passable
        {
            get { return passable; }
        }

        public abstract void ShellCollision(Shell shell);
    }
}