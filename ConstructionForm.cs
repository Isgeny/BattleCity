using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class ConstructionForm : AbstractForm
    {
        private Object mouseObstacle;

        private System.Collections.Generic.List<BattleCity.GUIObject> blockBtns;

        public ConstructionForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            throw new System.NotImplementedException();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void Subscribe()
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnPushBtnClicicked(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private Point RoundMousePos(Point point)
        {
            throw new System.NotImplementedException();
        }

        private Point TransformMousePos(Point point)
        {
            throw new System.NotImplementedException();
        }
    }
}