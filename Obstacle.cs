using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Obstacle : Object
    {
        public Obstacle(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
        }

        /*public override void SubscribeToForm()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void UnsubscribeFromForm()
        {
            GUIForm.Paint -= OnPaint;
        }*/
    }
}