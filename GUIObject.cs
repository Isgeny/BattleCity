using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class GUIObject : GraphicsObject
    {
        private string _text;
        private bool _selected;
        private Timer _spriteTimer;

        public event EventHandler Clicked;

        public GUIObject(GUIForm guiForm, Rectangle rect, string text, bool selected = false) : base(guiForm, rect)
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
                GUIForm.Invalidate(new Rectangle(Rect.X - 100, Rect.Y - 18, 64, 64));
                
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
                int currentFrame = (DateTime.Now.Millisecond % 24 < 13) ? 0 : 64;
                g.DrawImage(Properties.Resources.Tank_Selecting, new Rectangle(Rect.X - 100, Rect.Y - 18, 64, 64), new Rectangle(currentFrame, 0, 64, 64), GraphicsUnit.Pixel);
            }
        }

        private void OnSpriteTimer(object sender, EventArgs e)
        {
            GUIForm.Invalidate(new Rectangle(Rect.X - 100, Rect.Y - 18, 64, 64));
        }
    }
}