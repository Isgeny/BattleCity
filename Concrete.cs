﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Concrete : Obstacle
    {
        public Concrete(GUIForm guiForm, RectangleF rect) : base(guiForm, rect, 3, false)
        {
            throw new System.NotImplementedException();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void ShellCollision(Shell shell)
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
    }
}