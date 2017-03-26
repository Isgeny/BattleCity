using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class PushButton : GUIObject
    {
        public PushButton(GUIForm guiForm, RectangleF rect, string text, bool selected) : base(guiForm, rect, text, selected)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnMouseClick(object sender, MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}