using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Bonus : Object
    {
        private BonusType bonusType;

        public event EventHandler PlayersBomb;
        public event EventHandler PlayersWatch;
        public event EventHandler PlayersShowel;
        public event EventHandler CompsBomb;
        public event EventHandler CompsShowel;
        public event EventHandler CompsWatch;

        public Bonus(GUIForm guiForm, RectangleF rect, BonusType bonusType)
        {
            throw new System.NotImplementedException();
        }

        public BonusType BonusType
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Generate()
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

        private void OnBonusTankShoot(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeToBonusTank(CompTank tank)
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
    }
}