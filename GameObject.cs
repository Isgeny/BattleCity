using System.Drawing;

namespace BattleCity
{
    public abstract class GameObject : Object
    {
        private Rectangle _rect;

        public GameObject(GUIForm guiForm) : base(guiForm)
        {
            Rect = new Rectangle();
        }

        public GameObject(GUIForm guiForm, Rectangle rect) : base(guiForm)
        {
            Rect = rect;
        }

        public Rectangle Rect
        {
            get { return _rect; }
            set
            {
                if(!_rect.IsEmpty)
                    GUIForm.Invalidate(Rect);
                _rect = value;
                if(!_rect.IsEmpty)
                    GUIForm.Invalidate(Rect);
            }
        }

        protected virtual void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.NewRect))
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