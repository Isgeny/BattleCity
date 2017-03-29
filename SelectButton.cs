using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class SelectButton : GUIObject
    {
        public SelectButton(GUIForm guiForm, RectangleF rect, string text, bool selected = false) : base(guiForm, rect, text, selected)
        {
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(sender, e);
            Graphics g = e.Graphics;
            g.DrawString(Text, MyFont.GetFont(19), new SolidBrush(Color.White), new PointF(Rect.X, Rect.Y));
        }

        protected override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected && e.KeyCode == Keys.Enter)
            {
                InvokeClicked(new EventArgs());
            }
        }
    }
}