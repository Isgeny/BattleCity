using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class NameBox : GUIObject
    {
        private string hintText;

        public NameBox(GUIForm guiForm, RectangleF rect, string text, string hintText, bool selected = false) : base(guiForm, rect, text, selected)
        {
            this.hintText = hintText;
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(sender, e);
            Graphics g = e.Graphics;
            g.DrawString(hintText, new Font(MyFont.GetFont(28), FontStyle.Regular), new SolidBrush(Color.White), new PointF(Rect.X, Rect.Y));
            g.DrawString(Text, new Font(MyFont.GetFont(28), FontStyle.Regular), new SolidBrush(Color.White), new PointF(Rect.Right + 30.0f, Rect.Y));
        }

        protected override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected)
            {
                if(Text.Length < 8 && Char.IsLetterOrDigit((char)e.KeyCode))
                {
                    Text += Char.ToUpper((char)e.KeyCode);
                }
                else if(Text.Length > 0 && e.KeyCode == Keys.Back)
                {
                    Text = Text.Remove(Text.Length - 1);
                }
                GUIForm.Invalidate(new Region(new RectangleF(Rect.X, Rect.Y, 800.0f, 41.0f)));
            }
        }
    }
}