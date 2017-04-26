using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class OptionButton : GUIObject
    {
        private bool _enabled;

        public OptionButton(GUIForm guiForm, Rectangle rect, string text, bool enabled, bool selected = false) : base(guiForm, rect, text, selected)
        {
            _enabled = enabled;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                GUIForm.Invalidate(new Rectangle(Rect.Right + 50, Rect.Y, 150, 41));
            }
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.KeyDown += OnKeyDown;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.KeyDown -= OnKeyDown;
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(sender, e);
            Graphics g = e.Graphics;
            g.DrawString(Text, MyFont.GetFont(19), Brushes.White, Rect.X, Rect.Y);
            string str = (_enabled) ? "YES" : "NO";
            g.DrawString(str, MyFont.GetFont(19), Brushes.Gray, Rect.Right + 50, Rect.Y);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected && e.KeyCode == Keys.Enter)
                Enabled = !Enabled;
        }
    }
}