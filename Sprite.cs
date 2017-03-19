using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Sprite : Object
    {
        private Bitmap spriteImage;
        private Timer frameTimer;
        private float currentFrame;
        private float frameStep;
        private bool singleShoot;

        public Sprite(GUIForm guiForm, Bitmap spriteImage, RectangleF rect, int interval, float frameStep = 64.0f, bool singleShoot = false) : base(guiForm, rect)
        {
            currentFrame = 0;
            this.frameStep = frameStep;
            this.singleShoot = singleShoot;
            this.spriteImage = spriteImage;

            frameTimer = new Timer();
            frameTimer.Interval = interval;
            frameTimer.Tick += NextFrame;
        }

        private void NextFrame(object sender, EventArgs e)
        {
            if(currentFrame < spriteImage.Width - spriteImage.Height)
            {
                currentFrame += frameStep;
            }
            else
            {
                if(singleShoot)
                {
                    frameTimer.Stop();
                    UnsubscribeFromForm();
                }
                currentFrame = 0.0f;
            }
            GUIForm.Invalidate(new Region(Rect));
        }

        public void StartAnimation()
        {
            frameTimer.Enabled = true;
            GUIForm.Invalidate(new Region(Rect));
        }

        public void StopAnimation()
        {
            frameTimer.Enabled = false;
            GUIForm.Invalidate(new Region(Rect));
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if(frameTimer.Enabled)
            {
                Graphics g = e.Graphics;
                g.DrawImage(spriteImage, Rect, new RectangleF(currentFrame, 0.0f, spriteImage.Height, spriteImage.Height), GraphicsUnit.Pixel);
            }
        }

        public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;
        }
    }
}