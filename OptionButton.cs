using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class OptionButton : GUIObject
    {
        private bool enabled;

        public OptionButton(GUIForm guiForm, RectangleF rect, string text, bool enabled, bool selected = false) : base(guiForm, rect, text, selected)
        {
            this.enabled = enabled;
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                GUIForm.Invalidate(new Region(new RectangleF(Rect.Right + 50.0f, Rect.Y, 150.0f, 41.0f)));
            }
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(sender, e);
            Graphics g = e.Graphics;
            g.DrawString(Text, new Font(MyFont.GetFont(28), FontStyle.Regular), new SolidBrush(Color.White), new PointF(Rect.X, Rect.Y));
            string str = (enabled) ? "YES" : "NO";
            g.DrawString(str, new Font(MyFont.GetFont(28), FontStyle.Regular), new SolidBrush(Color.White), new PointF(Rect.Right + 50.0f, Rect.Y));
        }

        protected override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected && e.KeyCode == Keys.Enter)
            {
                Enabled = !Enabled;
            }
        }
    }
}