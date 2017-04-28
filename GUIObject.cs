using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class GUIObject : Object
    {
        protected Point Point { get; private set; }
        public string Text { get; set; }
        private bool _selected;
        private Timer _spriteTimer;

        public event EventHandler Clicked;

        public GUIObject(GUIForm guiForm, Point point, string text, bool selected = false) : base(guiForm)
        {
            Point = point;
            Text = text;

            _spriteTimer = new Timer();
            _spriteTimer.Interval = 24;
            _spriteTimer.Tick += OnSpriteTimer;
            Selected = selected;
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
                GUIForm.Invalidate(new Rectangle(Point.X - 100, Point.Y - 18, 64, 64));
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
            var g = e.Graphics;
            if(Selected)
            {
                int currentFrame = (DateTime.Now.Millisecond % 24 < 13) ? 0 : 64;
                g.DrawImage(Properties.Resources.Tank_Selecting, new Rectangle(Point.X - 100, Point.Y - 18, 64, 64), new Rectangle(currentFrame, 0, 64, 64), GraphicsUnit.Pixel);
            }
        }

        private void OnSpriteTimer(object sender, EventArgs e)
        {
            GUIForm.Invalidate(new Rectangle(Point.X - 100, Point.Y - 18, 64, 64));
        }
    }
}