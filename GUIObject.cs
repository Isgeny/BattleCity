using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class GUIObject : Object
    {
        private string text;
        private bool selected;
        private Timer spriteTimer;

        public event EventHandler Clicked;

        public GUIObject(GUIForm guiForm, RectangleF rect, string text, bool selected = false) : base(guiForm, rect)
        {
            this.text = text;
            this.selected = selected;

            spriteTimer = new Timer();
            spriteTimer.Interval = 24;
            spriteTimer.Tick += OnSpriteTimer;
            Selected = selected;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public virtual bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
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

        protected void InvokeClicked(EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
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
    }
}