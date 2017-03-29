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
            this.guiForm = guiForm;
            this.rect = rect;
            Destroy += OnDestroy;
        }

        public RectangleF Rect
        {
            get { return rect; }
            set { rect = value; }
        }

        public GUIForm GUIForm
        {
            get { return guiForm; }
            set { guiForm = value; }
        }

        public event RectEventHandler CheckPosition;

        protected void InvokeCheckPosition(RectEventArgs e)
        {
            CheckPosition?.Invoke(this, e);
        }

        protected virtual void OnCheckPosition(object sender, RectEventArgs e) { }

        public virtual void SubscribeToCheckPosition(Object obj)
        {
            obj.CheckPosition += OnCheckPosition;
        }

        public virtual void UnsubscribeFromCheckPosition(Object obj)
        {
            obj.CheckPosition -= OnCheckPosition;
        }
        
        protected virtual void OnPaint(object sender, PaintEventArgs e) { }

        public virtual void SubscribeToPaint()
        {
            guiForm.Paint += OnPaint;
        }

        public virtual void UnsubscribeFromPaint()
        {
            guiForm.Paint -= OnPaint;
        }

        protected virtual void OnKeyDown(object sender, KeyEventArgs e) { }

        public virtual void SubscribeToKeyDown()
        {
            guiForm.KeyDown += OnKeyDown;
        }

        public virtual void UnsubscribeFromKeyDown()
        {
            guiForm.KeyDown -= OnKeyDown;
        }

        protected virtual void OnKeyUp(object sender, KeyEventArgs e) { }

        public virtual void SubscribeToKeyUp()
        {
            guiForm.KeyUp += OnKeyUp;
        }

        public virtual void UnsubscribeFromKeyUp()
        {
            guiForm.KeyUp -= OnKeyUp;
        }

        protected virtual void OnMouseClick(object sender, MouseEventArgs e) { }

        public virtual void SubscribeToMouseClick()
        {
            guiForm.MouseClick += OnMouseClick;
        }

        public virtual void UnsubscribeFromMouseClick()
        {
            guiForm.MouseClick -= OnMouseClick;
        }

        protected virtual void OnMouseMove(object sender, MouseEventArgs e) { }

        public virtual void SubscribeToMouseMove()
        {
            guiForm.MouseMove += OnMouseMove;
        }

        public virtual void UnsubscribeFromMouseMove()
        {
            guiForm.MouseMove -= OnMouseMove;
        }

        protected virtual void OnMouseWheel(object sender, MouseEventArgs e) { }

        public virtual void SubscribeToMouseWheel()
        {
            guiForm.MouseWheel += OnMouseWheel;
        }

        public virtual void UnsubscribeFromMouseWheel()
        {
            guiForm.MouseWheel -= OnMouseWheel;
        }

        public event EventHandler Destroy;

        public void InvokeDestroy()
        {
            Destroy?.Invoke(this, new EventArgs());
        }

        protected virtual void OnDestroy(object sender, EventArgs e)
        {
            UnsubscribeFromPaint();
            GUIForm.Invalidate(new Region(Rect));
        }

        public void SubscribeToDestroy()
        {
            Destroy += OnDestroy;
        }

        public void UnsubscribeFromDestroy()
        {
            Destroy -= OnDestroy;
        }
    }
}