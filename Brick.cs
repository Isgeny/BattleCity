using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Brick : Object
    {
        private RectangleF _cuttedRect;

        public Brick(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            _cuttedRect = new RectangleF(0.0f, 0.0f, rect.Width, rect.Height);
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
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_0, Rect.X, Rect.Y, _cuttedRect, GraphicsUnit.Pixel);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Tank)
                {
                    ((Tank)sender).StopMoving();
                }
                else if(sender is Shell)
                {
                    Shell s = sender as Shell;
                    s.InvokeDestroyed();
                    int creatorStars = s.Creator.Stars;
                    if(creatorStars >= 0 && creatorStars <= 2)
                    {
                        RectangleF oldRect = Rect;
                        switch(s.Direction)
                        {
                            case Direction.Up:
                                _cuttedRect = new RectangleF(_cuttedRect.X, _cuttedRect.Y, _cuttedRect.Width, _cuttedRect.Height - 16.0f);
                                Rect = new RectangleF(Rect.X, Rect.Y, Rect.Width, Rect.Height - 16.0f);
                                break;
                            case Direction.Left:
                                _cuttedRect = new RectangleF(_cuttedRect.X, _cuttedRect.Y, _cuttedRect.Width - 16.0f, _cuttedRect.Height);
                                Rect = new RectangleF(Rect.X, Rect.Y, Rect.Width - 16.0f, Rect.Height);
                                break;
                            case Direction.Down:
                                _cuttedRect = new RectangleF(_cuttedRect.X, _cuttedRect.Y + 16.0f, _cuttedRect.Width, _cuttedRect.Height);
                                Rect = new RectangleF(Rect.X, Rect.Y + 16.0f, Rect.Width, Rect.Height);
                                break;
                            case Direction.Right:
                                _cuttedRect = new RectangleF(_cuttedRect.X + 16.0f, _cuttedRect.Y, _cuttedRect.Width, _cuttedRect.Height);
                                Rect = new RectangleF(Rect.X + 16.0f, Rect.Y, Rect.Width, Rect.Height);
                                break;
                        }
                        if(Rect.IsEmpty || _cuttedRect.X > 31.0f || _cuttedRect.Y > 31.0f)
                        {
                            InvokeDestroyed();
                        }
                        GUIForm.Invalidate(new Region(oldRect));
                    }
                    else if(creatorStars == 3)
                    {
                        InvokeDestroyed();
                    }
                }
            }
        }
    }
}