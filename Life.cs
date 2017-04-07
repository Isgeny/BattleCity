using System.Drawing;

namespace BattleCity
{
    public class Life : Bonus
    {
        public Life(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
            {
                ((Tank)sender).Lives++;
                Unsubscribe();
                GUIForm.Invalidate(new Region(Rect));
            }
        }
    }
}