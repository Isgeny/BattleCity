using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class GUIObject : Object
    {
        private string _text;
        private bool _selected;
        private Timer _spriteTimer;

        public event EventHandler Clicked;

        public GUIObject(GUIForm guiForm, RectangleF rect, string text, bool selected = false) : base(guiForm, rect)
        {
            _text = text;

            _spriteTimer = new Timer();
            _spriteTimer.Interval = 24;
            _spriteTimer.Tick += OnSpriteTimer;
            Selected = selected;
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public virtual bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if(Selected)
                    _spriteTimer.Start();
                else
                    _spriteTimer.Stop();
                GUIForm.Invalidate(new Region(new RectangleF(Rect.X - 100.0f, Rect.Y - 18.0f, 64.0f, 64.0f)));
            }
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
        }

        protected void InvokeClicked(EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        protected virtual void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if(Selected)
            {
                float currentFrame = (DateTime.Now.Millisecond % 24 < 13) ? 0.0f : 64.0f;
                g.DrawImage(Properties.Resources.Tank_Selecting, new RectangleF(Rect.X - 100.0f, Rect.Y - 18.0f, 64.0f, 64.0f), new RectangleF(currentFrame, 0.0f, 64.0f, 64.0f), GraphicsUnit.Pixel);
            }
        }

        private void OnSpriteTimer(object sender, EventArgs e)
        {
            GUIForm.Invalidate(new Region(new RectangleF(Rect.X - 100.0f, Rect.Y - 18.0f, 64.0f, 64.0f)));
        }
    }
}