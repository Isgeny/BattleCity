namespace BattleCity
{
    public class Life : Bonus
    {
        public Life(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void TankCollision(Tank tank)
        {
            base.TankCollision(tank);
            if(Rect.IntersectsWith(tank.Rect))
                if(tank is PlayerTank)
                    tank.Lives++;
                else if(tank is CompTank && Properties.Settings.Default.AIUseBonus)
                    tank.HP++;
        }
    }
}