using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BattleCity
{
    public class Brick : Obstacle
    {
        public Brick(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 2, false)
        {
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Bitmap bmap = GUIForm.Bitmap;
            Graphics g = Graphics.FromImage(bmap);
            g.DrawImage(Properties.Resources.Tile_0, Rect.X, Rect.Y);
        }

        public override void ShellCollision(Shell shell)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint += OnPaint;
        }
    }
}