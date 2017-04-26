using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class NameBox : GUIObject
    {
        private string _hintText;

        public NameBox(GUIForm guiForm, Rectangle rect, string text, string hintText, bool selected = false) : base(guiForm, rect, text, selected)
        {
            _hintText = hintText;
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
            g.DrawString(_hintText, MyFont.GetFont(19), Brushes.White, Rect.X, Rect.Y);
            g.DrawString(Text, MyFont.GetFont(19), Brushes.Gray, Rect.Right + 30, Rect.Y);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected)
            {
                if(Text.Length < 8 && Char.IsLetterOrDigit((char)e.KeyCode))
                    Text += Char.ToUpper((char)e.KeyCode);
                else if(Text.Length > 0 && e.KeyCode == Keys.Back)
                    Text = Text.Remove(Text.Length - 1);
                GUIForm.Invalidate(new Rectangle(Rect.X, Rect.Y, 800, 41));
            }
        }
    }
}