using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public abstract class PlayerTank : Tank
    {
        private Dictionary<int, bool> keys;

        public PlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 0.0f, Direction.Up)
        {
            throw new System.NotImplementedException();
        }

        public override void Move()
        {
            throw new System.NotImplementedException();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnMoveTimerTick()
        {
            throw new System.NotImplementedException();
        }

        private Direction GetCurrentDirection()
        {
            throw new System.NotImplementedException();
        }
    }
}