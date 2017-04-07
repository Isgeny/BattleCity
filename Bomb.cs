using System.Drawing;

namespace BattleCity
{
    public class Bomb : Bonus
    {
        public Bomb(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
            {
                if(sender is PlayerTank)
                    InvokePlayerTook();
                else if(sender is CompTank)
                    InvokeCompTook();
                Unsubscribe();
                GUIForm.Invalidate(new Region(Rect));
            }
        }
    }
}