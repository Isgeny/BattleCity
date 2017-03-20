using System.Drawing;

namespace BattleCity
{
    public class Water : Obstacle
    {
        private Sprite sprite;

        public Water(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 0, false)
        {
            sprite = new Sprite(guiForm, Properties.Resources.Tile_2, rect, 1000, 32.0f);
        }

        public override void ShellCollision(Shell shell)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            sprite.SubscribeToForm();
            sprite.StartAnimation();
        }

        public override void UnsubscribeFromForm()
        {
            sprite.UnsubscribeFromForm();
            sprite.StopAnimation();
        }
    }
}