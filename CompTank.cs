using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class CompTank : Tank
    {
        private bool bonus;
        private int period;

        public event EventHandler BonusTankShooted;

        public CompTank(GUIForm guiForm, RectangleF rect)
        {
            throw new System.NotImplementedException();
        }

        public bool Bonus
        {
            get => default(int);
            set
            {
            }
        }

        public Period Period
        {
            get => default(Period);
            set
            {
            }
        }

        protected override void Respawn()
        {
            throw new System.NotImplementedException();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnCheckPosition(object sender, PositionEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        public override void UnsubscribeFromForm()
        {
            throw new System.NotImplementedException();
        }

        public override void UnsubscribeFromObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        private static int TankRespawnTime(int stage, int playerCount)
        {
            throw new System.NotImplementedException();
        }

        private void ChangeDirection()
        {
            throw new System.NotImplementedException();
        }

        private void RandomDirection()
        {
            throw new System.NotImplementedException();
        }

        private void MoveToFirstPlayerTank()
        {
            throw new System.NotImplementedException();
        }

        private void MoveToSecondPlayerTank()
        {
            throw new System.NotImplementedException();
        }

        private void MoveToHQ()
        {
            throw new System.NotImplementedException();
        }

        private void InvertDirection()
        {
            throw new System.NotImplementedException();
        }

        private void RotateClockwise()
        {
            throw new System.NotImplementedException();
        }

        private void RotateAntiClockwise()
        {
            throw new System.NotImplementedException();
        }
    }
}