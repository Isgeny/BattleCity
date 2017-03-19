using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Water : Obstacle
    {
        public Water(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 0, false)
        {
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.Tile_2, Rect.X, Rect.Y);
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