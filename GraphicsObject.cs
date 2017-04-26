using System;
using System.Drawing;
using System.Collections.Generic;

namespace BattleCity
{
    public abstract class GraphicsObject : Object
    {
        private Rectangle _rect;

        public event EventHandler Destroyed;
        public event RectEventHandler CheckPosition;

        public GraphicsObject(GUIForm guiForm, Rectangle rect) : base(guiForm)
        {
            _rect = rect;
        }

        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }  

        public EventHandler GetDestroyedListener()
        {
            return OnDestroyed;
        }

        public RectEventHandler GetCheckPositionListener()
        {
            return OnCheckPosition;
        }

        protected virtual void OnDestroyed(object sender, EventArgs e)
        {
            GUIForm.Invalidate(_rect);
        }

        protected virtual void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(_rect.IntersectsWith(e.Rect) && sender is DynamicObject)
                ((DynamicObject)sender).StopMoving();
        }

        public void InvokeDestroyed()
        {
            Destroyed?.Invoke(this, new EventArgs());
        }

        protected void InvokeCheckPosition(RectEventArgs e)
        {
            CheckPosition?.Invoke(this, e);
        }

        //
        public void SubscribeToDestroyed(GraphicsObject obj)
        {
            obj.Destroyed += OnDestroyed;
        }

        public void UnsubscribeFromDestroyed(GraphicsObject obj)
        {
            obj.Destroyed -= OnDestroyed;
        }

        public virtual void SubscribeToCheckPosition(GraphicsObject obj)
        {
            obj.CheckPosition += OnCheckPosition;
        }

        public virtual void UnsubscribeFromCheckPosition(GraphicsObject obj)
        {
            obj.CheckPosition -= OnCheckPosition;
        }
    }
}


