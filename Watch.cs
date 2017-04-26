namespace BattleCity
{
    public class Watch : Bonus
    {
        public Watch(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            base.OnCheckPosition(sender, e);
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
            {
                if(sender is PlayerTank)
                    InvokePlayerTook();
                else if(sender is CompTank)
                    InvokeCompTook();
            }
        }
    }
}