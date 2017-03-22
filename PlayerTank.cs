using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class PlayerTank
    {
        public PlayerTank(GUIForm guiForm, RectangleF rect)/* : base(guiForm, rect, Direction.Up)*/
        {

        }

        /*public override void Move()
        {
            throw new System.NotImplementedException();
        }*/

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