using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace BattleCity
{
    public class FirstPlayerTank : PlayerTank
    {
        public FirstPlayerTank(GUIForm guiForm) : base(guiForm, new Rectangle())
        {
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.KeyDown += OnKeyDown;
        }

        public override void Unsubscribe()
        {
            GUIForm.Invalidate(new Rectangle(910, 585, 150, 41));
            GUIForm.Invalidate(new Rectangle(910, 626, 32, 32));
            GUIForm.Invalidate(new Rectangle(940, 626, 150, 41));
            base.Unsubscribe();
            GUIForm.KeyDown -= OnKeyDown;
        }

        public override int Lives
        {
            get { return base.Lives; }
            set
            {
                GUIForm.Invalidate(new Rectangle(910, 585, 150, 41));
                GUIForm.Invalidate(new Rectangle(910, 626, 32, 32));
                GUIForm.Invalidate(new Rectangle(940, 626, 150, 41));
                base.Lives = value;
            }
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(sender, e);
            if(Lives >= 0)
            {
                Graphics g = e.Graphics;
                g.DrawImageUnscaled(Properties.Resources.Player_Icon, 910, 626);
                string name = "";
                name += Properties.Settings.Default.P1Name[0];
                name += Properties.Settings.Default.P1Name[1];
                g.DrawString(name, MyFont.GetFont(22), Brushes.Black, 910, 585);
                g.DrawString(Lives.ToString(), MyFont.GetFont(22), Brushes.Black, 940, 626);
            }
        }

        protected override void OnMoveTimerTick(object sender, EventArgs e)
        {
            if(!RespawnTimer.Enabled)
            {
                if(Keyboard.IsKeyDown(Key.Up))
                {
                    Dx = 0;
                    Dy = -3;
                    Direction = Direction.Up;
                    IceTicks = 28;
                }
                else if(Keyboard.IsKeyDown(Key.Left))
                {
                    Dx = -3;
                    Dy = 0;
                    Direction = Direction.Left;
                    IceTicks = 28;
                }
                else if(Keyboard.IsKeyDown(Key.Down))
                {
                    Dx = 0;
                    Dy = 3;
                    Direction = Direction.Down;
                    IceTicks = 28;
                }
                else if(Keyboard.IsKeyDown(Key.Right))
                {
                    Dx = 3;
                    Dy = 0;
                    Direction = Direction.Right;
                    IceTicks = 28;
                }
                else
                {
                    Dx = 0;
                    Dy = 0;
                }
                base.OnMoveTimerTick(sender, e);
            }
        }

        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && Ammo > 0 && !ShootPressed && !ShotDelayTimer.Enabled && !RespawnTimer.Enabled)
            {
                Ammo--;
                Shell shell = new Shell(GUIForm, this);
                InvokeShoot(new ShellEventArgs(shell));
                ShootPressed = true;
                ShotDelayTimer.Start();
            }
        }

        public override void Respawn()
        {
            if(Lives >= 0)
            {
                Rect = new Rectangle(320, 832, 64, 64);
                base.Respawn();
            }
        }
    }
}