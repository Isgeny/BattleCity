using System;
using System.Drawing;

namespace BattleCity
{
    public abstract class GUIObject : Object
    {
        private string text;
        private bool selected;

        public event EventHandler Clicked;

        public GUIObject(GUIForm guiForm, RectangleF rect, string text, bool selected) : base(guiForm, rect)
        {
            this.text = text;
            this.selected = selected;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        protected void OnClicked(EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }
    }
}