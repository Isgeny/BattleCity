using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Bonus : GameObject
    {
        private Timer _bonusTimer;
        private Timer _pointsTimer;
        private Timer _flickerTimer;

        public event EventHandler TankTook;
        public event EventHandler PlayerTook;
        public event EventHandler CompTook;
        public event EventHandler TimeUp;

        public Bonus(GUIForm guiForm) : base(guiForm)
        {
            _bonusTimer = new Timer();
            _bonusTimer.Interval = 60 * 1000;
            _bonusTimer.Tick += OnBonusTimer;

            _pointsTimer = new Timer();
            _pointsTimer.Interval = 300;
            _pointsTimer.Tick += OnPointsTimerTick;

            _flickerTimer = new Timer();
            _flickerTimer.Interval = 150;
            _flickerTimer.Tick += OnFlickerTimerTick;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            MoveToNewPosition();
            _bonusTimer.Start();
            _flickerTimer.Start();
        }

        public override void Unsubscribe()
        {
            _bonusTimer.Stop();
            _pointsTimer.Stop();
            _flickerTimer.Stop();
            GUIForm.Invalidate(Rect);
            GUIForm.Paint -= OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if(Rect.IntersectsWith(e.ClipRectangle))
            {
                var g = e.Graphics;
                if(_bonusTimer.Enabled && _flickFlag)
                {
                    var rm = Properties.Resources.ResourceManager;
                    var filename = "Bonus_" + GetType().Name;
                    var bmp = (Bitmap)rm.GetObject(filename);
                    g.DrawImageUnscaled(bmp, Rect);
                }
                else if(_pointsTimer.Enabled)
                    g.DrawString("500", MyFont.GetFont(12), Brushes.White, Rect.X, Rect.Y + 20);
            }
        }

        protected override void TankCollision(Tank tank)
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.Paint += OnPaint;
            if(Rect.IntersectsWith(tank.Rect) && (tank is PlayerTank || tank is CompTank && Properties.Settings.Default.AIUseBonus))
            {
                TankTook.Invoke(this, new EventArgs());

                if(tank is PlayerTank)
                {
                    tank.Points += 500;
                    _bonusTimer.Stop();
                    _flickerTimer.Stop();
                    InvokePlayerTook(tank);
                    _pointsTimer.Start();
                    GUIForm.Invalidate(Rect);
                }
                else if(tank is CompTank)
                    InvokeCompTook();
            }
        }

        private void OnBonusTimer(object sender, EventArgs e)
        {
            Unsubscribe();
            TimeUp.Invoke(this, new EventArgs());
        }

        private void OnPointsTimerTick(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        private bool _flickFlag;
        private void OnFlickerTimerTick(object sender, EventArgs e)
        {
            _flickFlag = !_flickFlag;
            GUIForm.Invalidate(Rect);
        }

        private void MoveToNewPosition()
        {
            Rect = new Rectangle(64 + GameRandom.RandNumber(0, 24) * 32, 64 + GameRandom.RandNumber(0, 24) * 32, 64, 64);
        }

        protected void InvokePlayerTook(object tank)
        {
            PlayerTook?.Invoke(tank, new EventArgs());
        }

        protected void InvokeCompTook()
        {
            CompTook?.Invoke(this, new EventArgs());
        }
    }
}