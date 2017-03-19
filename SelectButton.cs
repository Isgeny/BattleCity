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

        public override bool Selected
        {
            get { return base.Selected; }
            set
            {
                base.Selected = value;
                GUIForm.Invalidate(new Region(new RectangleF(Rect.X - 10, Rect.Y, 64.0f, 64.0f)));
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(Text, new Font("Arial", 14), new SolidBrush(Color.Black), new PointF(Rect.X, Rect.Y));
            g.DrawImage(Properties.Resources.Tank_Selecting, Rect.X - 10, Rect.Y);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(Selected && e.KeyCode == Keys.Enter)
            {
                OnClicked(new EventArgs());
            }
        }

        public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;
            GUIForm.KeyDown += OnKeyDown;
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.KeyDown -= OnKeyDown;
        }
    }
}