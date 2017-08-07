using System.Drawing;
using System.Windows.Input;
using System.Windows.Forms;

namespace BattleCity
{
    public class SecondPlayerTank : PlayerTank
    {
        public override int Lives
        {
            get { return base.Lives; }
            set
            {
                base.Lives = value;
                GUIForm.Invalidate(new Rectangle(910, 690, 150, 41));
                GUIForm.Invalidate(new Rectangle(910, 732, 32, 32));
                GUIForm.Invalidate(new Rectangle(940, 732, 150, 41));
            }
        }

        public SecondPlayerTank(GUIForm guiForm) : base(guiForm)
        {
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(sender, e);
            var g = e.Graphics;
            e.Graphics.DrawImageUnscaled(Properties.Resources.Player_Icon, 910, 732);
            string name = "";
            name += Properties.Settings.Default.P2Name[0];
            name += Properties.Settings.Default.P2Name[1];
            g.DrawString(name, MyFont.GetFont(22), Brushes.Black, 910, 690);
            g.DrawString(Lives.ToString(), MyFont.GetFont(22), Brushes.Black, 940, 732);
        }

        protected override void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
                base.OnKeyDown(sender, e);
        }

        protected override bool UpPressed()
        {
            return Keyboard.IsKeyDown(Key.W);
        }

        protected override bool LeftPressed()
        {
            return Keyboard.IsKeyDown(Key.A);
        }

        protected override bool DownPressed()
        {
            return Keyboard.IsKeyDown(Key.S);
        }

        protected override bool RightPressed()
        {
            return Keyboard.IsKeyDown(Key.D);
        }

        protected override void MoveToStartPosition()
        {
            Rect = new Rectangle(576, 832, 64, 64);
        }
    }
}