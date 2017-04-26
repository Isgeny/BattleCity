namespace BattleCity
{
    public class Gun : Bonus
    {
        public Gun(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            base.OnCheckPosition(sender, e);
            if(Rect.IntersectsWith(e.Rect) && sender is Tank)
                ((Tank)sender).Gun = true;
        }
    }
}