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
        private bool changeable;

        public Sprite(GUIForm guiForm, Bitmap spriteImage, RectangleF rect, int interval, float frameStep = 64.0f, bool changeable = false) : base(guiForm, rect)
        {
            currentFrame = 0;
            this.frameStep = frameStep;
            this.changeable = changeable;
            this.spriteImage = spriteImage;

            frameTimer = new Timer();
            frameTimer.Interval = interval;
            frameTimer.Tick += OnFrameTimer;
        }

        private void OnFrameTimer(object sender, EventArgs e)
        {
            if(currentFrame < spriteImage.Width - spriteImage.Height)
            {
                currentFrame += frameStep;
            }
            else
            {
                currentFrame = 0.0f;
            }
            GUIForm.Invalidate(new Region(Rect));
        }

        public void NextFrame()
        {
            if(currentFrame < spriteImage.Width - spriteImage.Height)
            {
                currentFrame += frameStep;
            }
            else
            {
                currentFrame = 0.0f;
            }
        }

        public void StartAnimation()
        {
            frameTimer.Enabled = true;
        }

        public void StopAnimation()
        {
            frameTimer.Enabled = false;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if(frameTimer.Enabled || changeable)
            {
                Bitmap bmap = GUIForm.Bitmap;
                Graphics g = Graphics.FromImage(bmap);
                g.DrawImage(spriteImage, Rect, new RectangleF(currentFrame, 0.0f, spriteImage.Height, spriteImage.Height), GraphicsUnit.Pixel);
            }
        }

        public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;
            GUIForm.Invalidate(new Region(Rect));
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.Invalidate(new Region(Rect));
        }
    }
}