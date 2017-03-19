using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    public class CompTanksManager : TanksManager
    {
        private int aliveTanks;

        public CompTanksManager(GUIForm guiForm)
        {
            throw new System.NotImplementedException();
        }

        public override void UnsubscribeFromForm(GUIForm form)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SubscribeToForm(GUIForm gameForm)
        {
            throw new System.NotImplementedException();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnTankDestroy(object sender, EventHandler e)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeToBonus(Bonus bonus)
        {
            throw new System.NotImplementedException();
        }

        private void OnBonusTaked()
        {
            throw new System.NotImplementedException();
        }
    }
}