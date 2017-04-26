using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Brick : GraphicsObject
    {
        private Rectangle _cuttedRect;

        public Brick(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
        {
            _cuttedRect = new Rectangle(0, 0, rect.Width, rect.Height);
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            Destroyed += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            Destroyed -= OnDestroyed;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Rectangle clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_0, Rect.X + _cuttedRect.X, Rect.Y + _cuttedRect.Y, _cuttedRect, GraphicsUnit.Pixel);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is PlayerTank)
                {
                    ((PlayerTank)sender).StopMoving();
                }
                else if(sender is CompTank)
                {
                    ((CompTank)sender).StopMoving();
                }
                else if(sender is Shell)
                {
                    Shell s = sender as Shell;
                    s.InvokeDestroyed();
                    int creatorStars = s.Creator.Stars;
                    Tank creator = s.Creator;
                    Rectangle oldRect = Rect;
                    if(creator is PlayerTank && creatorStars >= 0 && creatorStars <= 2 || creator is CompTank && creatorStars >= 0 && creatorStars <= 3)
                    {
                        switch(s.Direction)
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
                        }
                        if(_cuttedRect.X >= 32 || _cuttedRect.Y >= 32 || _cuttedRect.Width <= 0 || _cuttedRect.Height <= 0)
                            InvokeDestroyed();
                    }
                    else if(creator is PlayerTank && creatorStars == 3)
                        InvokeDestroyed();
                    GUIForm.Invalidate(oldRect);
                }
            }
        }
    }
}