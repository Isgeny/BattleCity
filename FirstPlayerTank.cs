﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class FirstPlayerTank : PlayerTank
    {

        public FirstPlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            throw new System.NotImplementedException();
        }

        protected override void Respawn()
        {
            throw new System.NotImplementedException();
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