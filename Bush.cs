using System.Drawing;

namespace BattleCity
{
    public class Bush : Obstacle
    {
        public Bush(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
        {
        }

        protected override void TankCollision(Tank tank)
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.Paint += OnPaint;
        }

        protected override void ShellCollision(Shell shell)
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.Paint += OnPaint;
            if(shell.Creator is PlayerTank && shell.Creator.Gun)
                InvokeDestroyed();
        }
    }
}