using System.Drawing;

namespace BattleCity
{
    public class Gun : Bonus
    {
        public Gun(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
            {
                ((Tank)sender).Gun = true;
                Unsubscribe();
                GUIForm.Invalidate(new Region(Rect));
            }
        }
    }
}