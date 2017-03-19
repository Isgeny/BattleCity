using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class SelectButton : GUIObject
    {
        private Sprite sprite;

        public SelectButton(GUIForm guiForm, RectangleF rect, string text, bool selected = false) : base(guiForm, rect, text, selected)
        {
            sprite = new Sprite(guiForm, Properties.Resources.Tank_Selecting, new RectangleF(Rect.X - 100.0f, Rect.Y - 12.0f, 64.0f, 64.0f), 75);
            if(Selected)
            {
                sprite.StartAnimation();
            }
        }

        public override bool Selected
        {
            get { return base.Selected; }
            set
            {
                base.Selected = value;
                if(Selected)
                {
                    sprite.StartAnimation();
                }
                else
                {
                    sprite.StopAnimation();
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(Text, new Font(MyFont.GetFont(28), FontStyle.Regular), new SolidBrush(Color.White), new PointF(Rect.X, Rect.Y));
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
            sprite.SubscribeToForm();
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.KeyDown -= OnKeyDown;
            sprite.UnsubscribeFromForm();
        }
    }
}