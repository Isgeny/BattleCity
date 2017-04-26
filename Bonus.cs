using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Bonus : GraphicsObject
    {
        private Timer _bonusTimer;

        public event EventHandler TankTook;
        public event EventHandler PlayerTook;
        public event EventHandler CompTook;
        public event EventHandler TimeUp;

        public Bonus(GUIForm guiForm) : base(guiForm, new Rectangle())
        {
            _bonusTimer = new Timer();
            _bonusTimer.Interval = 60 * 1000;
        }

        public override void Subscribe()
        {
            GUIForm.Paint       += OnPaint;
            _bonusTimer.Tick    += OnBonusTimer;
            MoveToNewPosition();
        }

        public override void Unsubscribe()
        {
            GUIForm.Invalidate(Rectangle.Inflate(Rect, 8, 8));
            GUIForm.Paint       -= OnPaint;
            _bonusTimer.Tick    -= OnBonusTimer;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Rectangle clipRect = e.ClipRectangle;
            if(Rectangle.Inflate(Rect, 8, 8).IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                ResourceManager rm = Properties.Resources.ResourceManager;
                string filename = "Bonus_" + GetType().Name;
                Bitmap bmp = (Bitmap)rm.GetObject(filename);
                g.DrawImageUnscaled(bmp, Rect.Location);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
                TankTook.Invoke(this, new EventArgs());
        }

        private void OnBonusTimer(object sender, EventArgs e)
        {
            Unsubscribe();
            TimeUp.Invoke(this, new EventArgs());
        }

        private void MoveToNewPosition()
        {
            Rect = new Rectangle(64 + GameRandom.RandNumber(0, 24) * 32 + 4, 64 + GameRandom.RandNumber(0, 24) * 32 + 4, 56, 56);
            GUIForm.Invalidate(Rectangle.Inflate(Rect, 8, 8));
        }

        protected void InvokePlayerTook()
        {
            PlayerTook.Invoke(this, new EventArgs());
        }

        protected void InvokeCompTook()
        {
            CompTook.Invoke(this, new EventArgs());
        }
    }
}