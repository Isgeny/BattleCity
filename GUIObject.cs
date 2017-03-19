using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class GUIObject : Object
    {
        private bool selected;

        private string text;

        public event EventHandler Clicked;

        public GUIObject(GUIForm guiForm, RectangleF rect, string text, bool selected)
        {
            throw new System.NotImplementedException();
        }

        public bool Selected
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public string Text
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        protected void OnClicked(EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}