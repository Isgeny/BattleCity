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
            var playerTank = tank as PlayerTank;
            playerTank.OnIce = true;
        }
    }
}