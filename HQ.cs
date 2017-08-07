using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class HQ : Obstacle
    {
        private bool _destroyed;
        private Timer _explosionTimer;
        private int _explosionFrame;

        public HQ(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
        {
            _destroyed = false;
            _explosionTimer = new Timer();
            _explosionTimer.Interval = 50;
            _explosionTimer.Tick += OnExplosionTimer;
            _explosionFrame = 0;
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            if(Rect.IntersectsWith(e.ClipRectangle))
            {
                var g = e.Graphics;
                if(_explosionTimer.Enabled)
                    g.DrawImage(Properties.Resources.Tank_Death, new Rectangle(Rect.X - 32, Rect.Y - 32, 128, 128), new Rectangle(_explosionFrame, 0, 128, 128), GraphicsUnit.Pixel);
                else
                {
                    int currentFrame = (_destroyed) ? 64 : 0;
                    g.DrawImage(Properties.Resources.Tile_HQ, Rect, new Rectangle(currentFrame, 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
                }
            }
        }

        private void OnExplosionTimer(object sender, EventArgs e)
        {
            _explosionFrame += 128;
            if(_explosionFrame == 1152)
            {
                _explosionFrame = 0;
                _explosionTimer.Stop();
                _explosionTimer.Tick -= OnExplosionTimer;
            }
            GUIForm.Invalidate(Rectangle.Inflate(Rect, 64, 64));
        }

        protected override void ShellCollision(Shell shell)
        {
            base.ShellCollision(shell);
            if(!_destroyed)
            {
                _destroyed = true;
                _explosionTimer.Start();

                var hqDestroyedtimer = new Timer();
                hqDestroyedtimer.Interval = 5000;
                hqDestroyedtimer.Start();
                hqDestroyedtimer.Tick += OnHQDestroyedTimer;
            }
        }

        private void OnHQDestroyedTimer(object sender, EventArgs e)
        {
            var hqDestroyedtimer = sender as Timer;
            hqDestroyedtimer.Stop();
            hqDestroyedtimer.Tick -= OnHQDestroyedTimer;
            InvokeDestroyed();
        }
    }
}