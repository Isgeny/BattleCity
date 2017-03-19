﻿using System.Drawing;

namespace BattleCity
{
    public abstract class Object
    {
        private RectangleF rect;
        private GUIForm guiForm;

        public Object(GUIForm guiForm, RectangleF rect)
        {
            this.guiForm = guiForm;
            this.rect = rect;
        }

        public RectangleF Rect
        {
            get { return rect; }
        }

        public GUIForm GUIForm
        {
            get { return guiForm; }
        }

        public abstract void SubscribeToForm();

        public abstract void UnsubscribeFromForm();

        public virtual void SubscribeToObjectPosition(Object obj) { }

        public virtual void UnsubscribeFromObjectPosition(Object obj) { }
    }
}