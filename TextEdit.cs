using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class TextEdit : GUIObject
    {
        private string _hintText;

        public TextEdit(GUIForm guiForm, Point point, string text, string hintText, bool selected = false) : base(guiForm, point, text, selected)
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
            var g = e.Graphics;
            g.DrawString(_hintText, MyFont.GetFont(19), Brushes.White, Point);
            g.DrawString(Text, MyFont.GetFont(19), Brushes.Gray, Point.X + 280, Point.Y);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected)
            {
                if(Text.Length < 8 && Char.IsLetterOrDigit((char)e.KeyCode))
                    Text += Char.ToUpper((char)e.KeyCode);
                else if(Text.Length > 0 && e.KeyCode == Keys.Back)
                    Text = Text.Remove(Text.Length - 1);
                GUIForm.Invalidate(new Rectangle(Point.X, Point.Y, 800, 41));
            }
        }
    }
}