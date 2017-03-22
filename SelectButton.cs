using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class SelectButton : GUIObject
    {
        private Timer spriteTimer;

        public SelectButton(GUIForm guiForm, RectangleF rect, string text, bool selected = false) : base(guiForm, rect, text, selected)
        {
            spriteTimer = new Timer();
            spriteTimer.Interval = 24;
            spriteTimer.Tick += OnSpriteTimer;
            Selected = selected;
        }

        public override bool Selected
        {
            get { return base.Selected; }
            set
            {
                base.Selected = value;
                if(Selected)
                {
                    spriteTimer.Start();
                }
                else
                {
                    spriteTimer.Stop();
                }
                GUIForm.Invalidate(new Region(new RectangleF(Rect.X - 100.0f, Rect.Y - 12.0f, 64.0f, 64.0f)));
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(Text, new Font(MyFont.GetFont(28), FontStyle.Regular), new SolidBrush(Color.White), new PointF(Rect.X, Rect.Y));
            if(Selected)
            {
                float currentFrame = (DateTime.Now.Millisecond % 24 < 13) ? 0.0f : 64.0f;
                g.DrawImage(Properties.Resources.Tank_Selecting, new RectangleF(Rect.X - 100.0f, Rect.Y - 12.0f, 64.0f, 64.0f), new RectangleF(currentFrame, 0.0f, 64.0f, 64.0f), GraphicsUnit.Pixel);
            }
        }

        private void OnSpriteTimer(object sender, EventArgs e)
        {
            GUIForm.Invalidate(new Region(new RectangleF(Rect.X - 100.0f, Rect.Y - 12.0f, 64.0f, 64.0f)));
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