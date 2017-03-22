using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class SecondPlayerTank : Tank
    {

        public SecondPlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, Direction.Up)
        {
            
        }

        protected override void Respawn()
        {
            RectangleF newRect = new RectangleF(576.0f, 832.0f, Rect.Width, Rect.Height);
            Rect = newRect;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            throw new System.NotImplementedException();
        }

        public override void UnsubscribeFromForm()
        {
            throw new System.NotImplementedException();
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