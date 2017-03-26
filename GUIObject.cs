using System;
using System.Drawing;

namespace BattleCity
{
    public abstract class GUIObject : Object
    {
        private string text;
        private bool selected;

        public event EventHandler Clicked;

        public GUIObject(GUIForm guiForm, RectangleF rect, string text, bool selected = false) : base(guiForm, rect)
        {
            this.text = text;
            this.selected = selected;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public virtual bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        protected void InvokeClicked(EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }
    }
}