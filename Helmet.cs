namespace BattleCity
{
    public class Helmet : Bonus
    {
        public Helmet(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            base.OnCheckPosition(sender, e);
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
                ((Tank)sender).Immortal = true;
        }
    }
}