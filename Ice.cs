using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Ice : Obstacle
    {
        public Ice(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 0, true)
        {
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.Tile_4, Rect.X, Rect.Y);
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