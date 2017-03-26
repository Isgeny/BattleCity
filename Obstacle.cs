using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Obstacle : Object
    {
        public Obstacle(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
        }

        public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;
        }

        public override void SubscribeToObjectPosition(Object obj)
        {
            obj.CheckPosition += OnCheckPosition;
        }

        public override void UnsubscribeFromObjectPosition(Object obj)
        {
            obj.CheckPosition -= OnCheckPosition;
        }

        protected abstract void OnPaint(object sender, PaintEventArgs e);

        protected abstract void OnCheckPosition(object sender, RectEventArgs e);
    }
}