using System.Drawing;

namespace BattleCity
{
    public class Ship : Bonus
    {
        public Ship(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
            {
                ((Tank)sender).Amphibian = true;
                Unsubscribe();
                GUIForm.Invalidate(new Region(Rect));
            }
        }
    }
}