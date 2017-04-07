using System.Drawing;

namespace BattleCity
{
    public class Star : Bonus
    {
        public Star(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
            {
                ((Tank)sender).Stars++;
                Unsubscribe();
                GUIForm.Invalidate(new Region(Rect));
            }
        }
    }
}