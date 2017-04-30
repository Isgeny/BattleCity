using System.Drawing;

namespace BattleCity
{
    public abstract class GraphicsObject : Object
    {
        public Rectangle Rect { get; set; }

        public GraphicsObject(GUIForm guiForm, Rectangle rect) : base(guiForm)
        {
            Rect = rect;
        }

        protected virtual void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
                if(sender is Tank)
                    TankCollision(sender as Tank);
                else if(sender is Shell)
                    ShellCollision(sender as Shell);
        }

        public RectEventHandler GetCheckPositionListener()
        {
            return OnCheckPosition;
        }

        protected virtual void TankCollision(Tank tank)
        {
            tank.StopMoving();
        }

        protected virtual void ShellCollision(Shell shell)
        {
            shell.InvokeDestroyed();
        }
    }
}