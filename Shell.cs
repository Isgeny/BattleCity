using System;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;

namespace BattleCity
{
    public class Shell : DynamicObject
    {
        private Tank _creator;

        public Shell(GUIForm guiForm, Tank creator) : base(guiForm, new RectangleF(), creator.Direction)
        {
            _creator = creator;
            MoveTimer.Tick += OnMoveTimer;
            MoveTimer.Start();

            float speed = 7.0f;
            if(creator is PlayerTank && creator.Stars >= 1)
            {
                speed = 10.0f;
            }

            switch(Direction)
            {
                case Direction.Up:
                    Dx = 0.0f;
                    Dy = -speed;
                    Rect = new RectangleF(creator.Rect.X + 22.0f, creator.Rect.Y - 6.0f, 16.0f, 16.0f);
                    break;
                case Direction.Left:
                    Dx = -speed;
                    Dy = 0.0f;
                    Rect = new RectangleF(creator.Rect.X - 6.0f, creator.Rect.Y + 22.0f, 16.0f, 16.0f);
                    break;
                case Direction.Down:
                    Dx = 0.0f;
                    Dy = speed;
                    Rect = new RectangleF(creator.Rect.X + 22.0f, creator.Rect.Y + 50.0f, 16.0f, 16.0f);
                    break;
                case Direction.Right:
                    Dx = speed;
                    Dy = 0.0f;
                    Rect = new RectangleF(creator.Rect.X + 50.0f, creator.Rect.Y + 22.0f, 16.0f, 16.0f);
                    break;
            }
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            Destroyed += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            Destroyed -= OnDestroyed;
        }

        public Tank Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                ResourceManager rm = Properties.Resources.ResourceManager;
                string filename = "Shell_" + (int)Direction;
                Bitmap bmp = (Bitmap)rm.GetObject(filename);
                g.DrawImage(bmp, Rect, new RectangleF(0.0f, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Tank)
                {
                    InvokeDestroyed();
                }
                else if(sender is Shell)
                {
                    InvokeDestroyed();
                    ((Shell)sender).InvokeDestroyed();
                }
            }
        }

        private void OnMoveTimer(object sender, EventArgs e)
        {
            InvokeCheckPosition(new RectEventArgs(new RectangleF(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            Move();
        }

        protected override void OnDestroyed(object sender, EventArgs e)
        {
            base.OnDestroyed(sender, e);
            if(MoveTimer.Enabled)
            {
                Unsubscribe();
                MoveTimer.Stop();
                _creator.Ammo++;
            }
        }

        public override void SubscribeToCheckPosition(Object obj)
        {
            if(obj is Tank && _creator == obj)
                return;
            base.SubscribeToCheckPosition(obj);
        }

        public override void UnsubscribeFromCheckPosition(Object obj)
        {
            if(obj is Tank && _creator == obj)
                return;
            base.UnsubscribeFromCheckPosition(obj);
        }
    }
}