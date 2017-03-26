using System.Drawing;

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
            set { rect = value; }
        }

        public GUIForm GUIForm
        {
            get { return guiForm; }
        }

        public event RectEventHandler CheckPosition;

        protected void OnCheckPosition(RectEventArgs e)
        {
            CheckPosition?.Invoke(this, e);
        }

        public abstract void SubscribeToForm();

        public abstract void UnsubscribeFromForm();

        public virtual void SubscribeToObjectPosition(Object obj) { }

        public virtual void UnsubscribeFromObjectPosition(Object obj) { }
    }
}