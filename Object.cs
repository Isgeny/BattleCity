using System;
using System.Drawing;

namespace BattleCity
{
    public abstract class Object
    {
        private GUIForm _guiForm;
        private RectangleF _rect; 

        public Object(GUIForm guiForm, RectangleF rect)
        {
            _guiForm = guiForm;
            _rect = rect;
        }

        public RectangleF Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public GUIForm GUIForm
        {
            get { return _guiForm; }
            set { _guiForm = value; }
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();

        public event RectEventHandler CheckPosition;
        public event EventHandler Destroyed;

        protected void InvokeCheckPosition(RectEventArgs e)
        {
            CheckPosition?.Invoke(this, e);
        }

        public virtual void SubscribeToCheckPosition(Object obj)
        {
            obj.CheckPosition += OnCheckPosition;
        }

        public virtual void UnsubscribeFromCheckPosition(Object obj)
        {
            obj.CheckPosition -= OnCheckPosition;
        }

        protected virtual void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect) && sender is DynamicObject)
                ((DynamicObject)sender).StopMoving();
        }

        public void InvokeDestroyed()
        {
            Destroyed?.Invoke(this, new EventArgs());
        }

        public void SubscribeToDestroyed(Object obj)
        {
            obj.Destroyed += OnDestroyed;
        }

        public void UnsubscribeFromDestroyed(Object obj)
        {
            obj.Destroyed -= OnDestroyed;
        }

        protected virtual void OnDestroyed(object sender, EventArgs e)
        {
            GUIForm.Invalidate(new Region(Rect));
        }  
    }
}