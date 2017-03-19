using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Object
    {
        private RectangleF rect;
        private GUIForm guiForm;

        public Object(GUIForm guiForm, RectangleF rect)
        {
            throw new System.NotImplementedException();
        }

        public RectangleF Rect
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public int GUIForm
        {
            get => default(int);
        }

        public abstract void SubscribeToForm();

        public virtual void SubscribeToObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        public abstract void UnsubscribeFromForm();

        public virtual void UnsubscribeFromObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        protected void OnCheckPosition(RectEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
}