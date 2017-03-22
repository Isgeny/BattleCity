using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

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

        /*private void OnRedraw(object sender, EventArgs e)
        {
            Graphics g = GUIForm.Painter.Graphics;
            g.DrawImage(Properties.Resources.P1_1_0, Rect, new RectangleF(0.0f, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
        }*/

        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            /*switch(e.KeyCode)
            {
                case Keys.Up:
                    Dx = 0.0f;
                    Dy = -2.25f;
                    break;
                case Keys.Left:
                    Dx = -2.25f;
                    Dy = 0.0f;
                    break;
                case Keys.Down:
                    Dx = 0.0f;
                    Dy = 2.25f;
                    break;
                case Keys.Right:
                    Dx = 2.25f;
                    Dy = 0.0f;
                    break;
                default:
                    break;
            }*/
        }

        private void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            /*Dx = 0.0f;
            Dy = 0.0f;*/
        }

        public override void SubscribeToForm()
        {
            /*GUIForm.Redraw += OnRedraw;
            GUIForm.KeyDown += OnKeyDown;
            GUIForm.KeyUp += OnKeyUp;

            MoveTimer.Tick += OnMoveTimerTick;
            MoveTimer.Start();*/
        }

        public override void UnsubscribeFromForm()
        {
            /*GUIForm.Redraw -= OnRedraw;
            GUIForm.KeyDown -= OnKeyDown;
            GUIForm.KeyUp -= OnKeyUp;
            MoveTimer.Tick -= OnMoveTimerTick;*/
        }

        private void OnMoveTimerTick(object sender, EventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.Up))
            {
                Dx = 0.0f;
                Dy = -2.25f;
            }
            else if(Keyboard.IsKeyDown(Key.Left))
            {
                Dx = -2.25f;
                Dy = 0.0f;
            }
            else if(Keyboard.IsKeyDown(Key.Down))
            {
                Dx = 0.0f;
                Dy = 2.25f;
            }
            else if(Keyboard.IsKeyDown(Key.Right))
            {
                Dx = 2.25f;
                Dy = 0.0f;
            }
            Move();
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