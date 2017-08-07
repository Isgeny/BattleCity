namespace BattleCity
{
    public class Gun : Bonus
    {
        public Gun(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void TankCollision(Tank tank)
        {
            base.TankCollision(tank);
            if(Rect.IntersectsWith(tank.Rect) && (tank is PlayerTank || tank is CompTank && Properties.Settings.Default.AIUseBonus))
                tank.Gun = true;
        }
    }
}