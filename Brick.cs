using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Brick : Obstacle
    {
        private Rectangle _cuttedRect;

        public Brick(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
        {
            _cuttedRect = new Rectangle(0, 0, rect.Width, rect.Height);
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            var clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                var g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_Brick, Rect.X + _cuttedRect.X, Rect.Y + _cuttedRect.Y, _cuttedRect, GraphicsUnit.Pixel);
            }
        }

        protected override void ShellCollision(Shell shell)
        {
            base.ShellCollision(shell);
            var creator = shell.Creator;
            if(creator is PlayerTank && creator.Stars <= 2 || creator is CompTank)
            {
                switch(shell.Direction)
                {
                    case Direction.Up:
                        _cuttedRect = new Rectangle(_cuttedRect.X, _cuttedRect.Y, _cuttedRect.Width, _cuttedRect.Height - 16);
                        break;
                    case Direction.Left:
                        _cuttedRect = new Rectangle(_cuttedRect.X, _cuttedRect.Y, _cuttedRect.Width - 16, _cuttedRect.Height);
                        break;
                    case Direction.Down:
                        _cuttedRect = new Rectangle(_cuttedRect.X, _cuttedRect.Y + 16, _cuttedRect.Width, _cuttedRect.Height);
                        break;
                    case Direction.Right:
                        _cuttedRect = new Rectangle(_cuttedRect.X + 16, _cuttedRect.Y, _cuttedRect.Width, _cuttedRect.Height);
                        break;
                    default:
                        break;
                }
                if(_cuttedRect.X == _cuttedRect.Width || _cuttedRect.Y == _cuttedRect.Height)
                    InvokeDestroyed();
            }
            else if(creator is PlayerTank && creator.Stars == 3)
                InvokeDestroyed();
        }
    }
}