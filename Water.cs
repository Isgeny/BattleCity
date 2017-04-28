using System.Drawing;

namespace BattleCity
{
    public class Water : Obstacle
    {
        public Water(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
        {
        }

        protected override void TankCollision(Tank tank)
        {
            if(!tank.Amphibian)
                tank.StopMoving();
        }
    }
}