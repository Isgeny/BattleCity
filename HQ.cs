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
            _explosionTimer.Interval = 100;
            _explosionTimer.Tick += OnExplosionTimer;
            _explosionFrame = 0;
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            var clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                var g = e.Graphics;
                if(_explosionTimer.Enabled)
                    g.DrawImage(Properties.Resources.Tank_Death, new Rectangle(Rect.X - 32, Rect.Y - 32, 128, 128), new Rectangle(_explosionFrame, 0, 128, 128), GraphicsUnit.Pixel);
                else
                {
                    int currentFrame = (!_destroyed) ? 0 : 64;
                    g.DrawImage(Properties.Resources.Tile_HQ, Rect, new Rectangle(currentFrame, 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
                }
            }
        }

        protected override void OnDestroyed(object sender, EventArgs e)
        {
            base.OnDestroyed(sender, e);
            _explosionTimer.Start();
        }

        private void OnExplosionTimer(object sender, EventArgs e)
        {
            _explosionFrame = _explosionFrame + 128;
            if(_explosionFrame % 1152 == 0)
            {
                _explosionTimer.Stop();
                _explosionTimer.Tick -= OnExplosionTimer;
            }
            GUIForm.Invalidate(Rectangle.Inflate(Rect, 64, 64));
        }

        protected override void ShellCollision(Shell shell)
        {
            base.ShellCollision(shell);
            _destroyed = true;
            InvokeDestroyed();
        }
    }
}