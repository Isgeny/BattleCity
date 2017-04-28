using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class SelectButton : GUIObject
    {
        public SelectButton(GUIForm guiForm, Point point, string text, bool selected = false) : base(guiForm, point, text, selected)
        {
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
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected && e.KeyCode == Keys.Enter)
                InvokeClicked(new EventArgs());
        }
    }
}