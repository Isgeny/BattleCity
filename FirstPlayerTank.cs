using System.Drawing;
using System.Windows.Input;
using System.Windows.Forms;

namespace BattleCity
{
    public class FirstPlayerTank : PlayerTank
    {
        public override int Lives
        {
            get { return base.Lives; }
            set
            {
                base.Lives = value;
                GUIForm.Invalidate(new Rectangle(910, 585, 150, 41));
                GUIForm.Invalidate(new Rectangle(910, 626, 32, 32));
                GUIForm.Invalidate(new Rectangle(940, 626, 150, 41));
            }
        }

        public FirstPlayerTank(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(sender, e);
            var g = e.Graphics;
            g.DrawImageUnscaled(Properties.Resources.Player_Icon, 910, 626);
            string name = "";
            name += Properties.Settings.Default.P1Name[0];
            name += Properties.Settings.Default.P1Name[1];
            g.DrawString(name, MyFont.GetFont(22), Brushes.Black, 910, 585);
            g.DrawString(Lives.ToString(), MyFont.GetFont(22), Brushes.Black, 940, 626);
        }

        protected override void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                base.OnKeyDown(sender, e);

            //DEBUG
            if(e.KeyCode == Keys.F1)
            {
                Stars++;
            }
            else if(e.KeyCode == Keys.F2)
            {
                Amphibian = !Amphibian;
            }
            else if(e.KeyCode == Keys.F3)
            {
                Gun = !Gun;
            }
            else if(e.KeyCode == Keys.F4)
            {
                HP--;
            }
            else if(e.KeyCode == Keys.F5)
            {
                Immortal = !Immortal;
            }
        }

        protected override bool UpPressed()
        {
            return Keyboard.IsKeyDown(Key.Up);
        }

        protected override bool LeftPressed()
        {
            return Keyboard.IsKeyDown(Key.Left);
        }

        protected override bool DownPressed()
        {
            return Keyboard.IsKeyDown(Key.Down);
        }

        protected override bool RightPressed()
        {
            return Keyboard.IsKeyDown(Key.Right);
        }

        protected override void MoveToStartPosition()
        {
            Rect = new Rectangle(320, 832, 64, 64);
        }
    }
}