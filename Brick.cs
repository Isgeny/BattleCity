using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Brick : Object
    {
        private RectangleF cuttedRect;

        public Brick(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            cuttedRect = new RectangleF(0.0f, 0.0f, rect.Width, rect.Height);
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_0, Rect.X, Rect.Y, cuttedRect, GraphicsUnit.Pixel);
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
                    s.InvokeDestroy();
                    int creatorStars = s.Creator.Stars;
                    if(creatorStars >= 0 && creatorStars <= 2)
                    {
                        RectangleF oldRect = Rect;
                        switch(s.Direction)
                        {
                            case Direction.Up:
                                cuttedRect = new RectangleF(cuttedRect.X, cuttedRect.Y, cuttedRect.Width, cuttedRect.Height - 16.0f);
                                Rect = new RectangleF(Rect.X, Rect.Y, Rect.Width, Rect.Height - 16.0f);
                                break;
                            case Direction.Left:
                                cuttedRect = new RectangleF(cuttedRect.X, cuttedRect.Y, cuttedRect.Width - 16.0f, cuttedRect.Height);
                                Rect = new RectangleF(Rect.X, Rect.Y, Rect.Width - 16.0f, Rect.Height);
                                break;
                            case Direction.Down:
                                cuttedRect = new RectangleF(cuttedRect.X, cuttedRect.Y + 16.0f, cuttedRect.Width, cuttedRect.Height);
                                Rect = new RectangleF(Rect.X, Rect.Y + 16.0f, Rect.Width, Rect.Height);
                                break;
                            case Direction.Right:
                                cuttedRect = new RectangleF(cuttedRect.X + 16.0f, cuttedRect.Y, cuttedRect.Width, cuttedRect.Height);
                                Rect = new RectangleF(Rect.X + 16.0f, Rect.Y, Rect.Width, Rect.Height);
                                break;
                        }
                        if(Rect.IsEmpty || cuttedRect.X > 31.0f || cuttedRect.Y > 31.0f)
                        {
                            InvokeDestroy();
                        }
                        GUIForm.Invalidate(new Region(oldRect));
                    }
                    else if(creatorStars == 3)
                    {
                        InvokeDestroy();
                    }
                }
            }
        }
    }
}