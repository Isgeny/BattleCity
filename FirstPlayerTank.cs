using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using System.Resources;

namespace BattleCity
{
    public class FirstPlayerTank : Tank
    {
        public FirstPlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, Direction.Up)
        {
            MoveTimer = new Timer();
            MoveTimer.Interval = 1000/60;
        }

        protected override void Respawn()
        {
            RectangleF newRect = new RectangleF(320.0f, 832.0f, Rect.Width, Rect.Height);
            Rect = newRect;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                float currentFrame = ((Rect.X + Rect.Y) % 8 < 4) ? 0.0f : 64.0f;
                Bitmap bmp = GetCurrentSprite();
                g.DrawImage(bmp, Rect, new RectangleF(currentFrame, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
            }            
        }

        public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;

            MoveTimer.Tick += OnMoveTimerTick;
            MoveTimer.Start();        
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;

            MoveTimer.Tick -= OnMoveTimerTick;
            MoveTimer.Stop();
        }

        private void OnMoveTimerTick(object sender, EventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.Up))
            {
                Dx = 0.0f;
                Dy = -2.75f;
                Direction = Direction.Up;
            }
            else if(Keyboard.IsKeyDown(Key.Left))
            {
                Dx = -2.75f;
                Dy = 0.0f;
                Direction = Direction.Left;
            }
            else if(Keyboard.IsKeyDown(Key.Down))
            {
                Dx = 0.0f;
                Dy = 2.75f;
                Direction = Direction.Down;
            }
            else if(Keyboard.IsKeyDown(Key.Right))
            {
                Dx = 2.75f;
                Dy = 0.0f;
                Direction = Direction.Right;
            }
            else
            {
                Dx = 0.0f;
                Dy = 0.0f;
            }
            OnCheckPosition(new RectEventArgs(new RectangleF(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            Move();
            Turn(Direction);
        }

        private Bitmap GetCurrentSprite()
        {
            ResourceManager rm = Properties.Resources.ResourceManager;
            string filename = "P1_" + Stars + "_" + (int)Direction;
            Bitmap bmp = (Bitmap)rm.GetObject(filename);
            return bmp;
        }

        private void OnCheckPosition(object sender, RectEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        public override void UnsubscribeFromObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}