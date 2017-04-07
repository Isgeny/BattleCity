using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BattleCity
{
    public class CompTanksManager : Object
    {
        private List<CompTank> tanks;
        private int aliveTanks;

        public CompTanksManager(GUIForm guiForm) : base(guiForm, new RectangleF())
        {
        }

        public override void Subscribe()
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnTankDestroyed(object sender, EventHandler e)
        {
            throw new System.NotImplementedException();
        }

        private void OnBonusTaked()
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeToBonus(/*Bonus bonus*/)
        {
            throw new System.NotImplementedException();
        }      
    }
}