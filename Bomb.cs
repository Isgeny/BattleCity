namespace BattleCity
{
    public class Bomb : Bonus
    {
        public Bomb(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            base.OnCheckPosition(sender, e);
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is PlayerTank)
                    InvokePlayerTook();
                else if(sender is CompTank)
                    InvokeCompTook();
            }
        }
    }
}