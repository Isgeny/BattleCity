using System.Drawing;

namespace BattleCity
{
    public class Ice : Obstacle
    {
        public Ice(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
        {
        }

        protected override void TankCollision(Tank tank)
        {
            if(tank is PlayerTank)
            {
                var playerTank = tank as PlayerTank;
                playerTank.OnIce = true;
            }
        }
    }
}