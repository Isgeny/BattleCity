namespace BattleCity
{
    public class Life : Bonus
    {
        public Life(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            base.OnCheckPosition(sender, e);
            if(Rect.IntersectsWith(e.Rect))
                if(sender is PlayerTank)
                    ((Tank)sender).Lives++;
                else if(sender is CompTank)
                    ((Tank)sender).HP++;
        }
    }
}