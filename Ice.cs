using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Ice : Object
    {
        public Ice(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_4, Rect.X, Rect.Y);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Tank)
                {
                    if(sender is PlayerTank)
                    {
                        PlayerTank t = sender as PlayerTank;
                        t.OnIce = true;
                    }
                }
            }
        }
    }
}