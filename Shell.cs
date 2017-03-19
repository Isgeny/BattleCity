using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BattleCity
{
    public class Shell : DynamicObject
    {
        private int power;
        private Tank creator;

        public Shell(GUIForm guiForm, RectangleF rect, Tank creator) : base(guiForm, rect, 0.0f, Direction.Up)
        {
            throw new System.NotImplementedException();
        }

        public int Power
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Tank Creator
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnCheckPosition(object sender, RectEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMoveTimerTick(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void UnsubscribeFromObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        public override void UnsubscribeFromForm()
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            throw new System.NotImplementedException();
        }
    }
}