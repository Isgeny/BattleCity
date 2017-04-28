using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class OptionButton : GUIObject
    {
        private bool _enabled;

        public OptionButton(GUIForm guiForm, Point point, string text, bool enabled, bool selected = false) : base(guiForm, point, text, selected)
        {
            _enabled = enabled;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                GUIForm.Invalidate(new Rectangle(Point.X + 450 ,Point.Y, 150, 41));
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
            var g = e.Graphics;
            g.DrawString(Text, MyFont.GetFont(19), Brushes.White, Point);
            string str = (_enabled) ? "YES" : "NO";
            g.DrawString(str, MyFont.GetFont(19), Brushes.Gray, Point.X + 450, Point.Y);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected && e.KeyCode == Keys.Enter)
                Enabled = !Enabled;
        }
    }
}