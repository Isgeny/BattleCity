using System.Drawing;

namespace BattleCity
{
    public class HQ : Obstacle
    {
        Sprite sprite;
        private bool destroyed;

        public HQ(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 2, false)
        {
            sprite = new Sprite(GUIForm, Properties.Resources.Tile_5, rect, 1, 64.0f, true);
            destroyed = false;
        }

        public bool Destroyed
        {
            get { return destroyed; }
            set
            {
                destroyed = value;
                if(destroyed)
                {
                    sprite.NextFrame();
                }
            }
        }

        public override void ShellCollision(Shell shell)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            sprite.SubscribeToForm();
        }

        public override void UnsubscribeFromForm()
        {
            sprite.UnsubscribeFromForm();
        }
    }
}