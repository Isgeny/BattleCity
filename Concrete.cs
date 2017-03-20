using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Concrete : Obstacle
    {
        public Concrete(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 3, false)
        {
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Bitmap bmap = GUIForm.Bitmap;
            Graphics g = Graphics.FromImage(bmap);
            g.DrawImage(Properties.Resources.Tile_1, Rect.X, Rect.Y);
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
            GUIForm.Paint -= OnPaint;
        }
    }
}