using System.Drawing;

namespace BattleCity
{
    public class Concrete : Obstacle
    {
        public Concrete(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
        {
        }

        protected override void ShellCollision(Shell shell)
        {
            base.ShellCollision(shell);
            var creator = shell.Creator;
            if(creator is PlayerTank && creator.Stars == 3 || creator is CompTank && creator.Gun)
                InvokeDestroyed();
        }
    }
}