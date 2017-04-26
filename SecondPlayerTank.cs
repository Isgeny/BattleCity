using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace BattleCity
{
    public class SecondPlayerTank : PlayerTank
    {
        public SecondPlayerTank(GUIForm guiForm) : base(guiForm, new Rectangle())
        {
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.KeyDown += OnKeyDown;
        }

        public override void Unsubscribe()
        {
            GUIForm.Invalidate(new Rectangle(910, 690, 150, 41));
            GUIForm.Invalidate(new Rectangle(910, 732, 32, 32));
            GUIForm.Invalidate(new Rectangle(940, 732, 150, 41));
            base.Unsubscribe();
            GUIForm.KeyDown -= OnKeyDown;
        }

        public override int Lives
        {
            get { return base.Lives; }
            set
            {
                GUIForm.Invalidate(new Rectangle(910, 690, 150, 41));
                GUIForm.Invalidate(new Rectangle(910, 732, 32, 32));
                GUIForm.Invalidate(new Rectangle(940, 732, 150, 41));
                base.Lives = value;
            }
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(sender, e);
            if(Lives >= 0)
            {
                Graphics g = e.Graphics;
                e.Graphics.DrawImageUnscaled(Properties.Resources.Player_Icon, 910, 732);
                string name = "";
                name += Properties.Settings.Default.P2Name[0];
                name += Properties.Settings.Default.P2Name[1];
                g.DrawString(name, MyFont.GetFont(22), Brushes.Black, 910, 690);
                g.DrawString(Lives.ToString(), MyFont.GetFont(22), Brushes.Black, 940, 732);
            }
        }

        protected override void OnMoveTimerTick(object sender, EventArgs e)
        {
            if(!RespawnTimer.Enabled)
            {
                if(Keyboard.IsKeyDown(Key.W))
                {
                    Dx = 0;
                    Dy = -3;
                    Direction = Direction.Up;
                    IceTicks = 28;
                }
                else if(Keyboard.IsKeyDown(Key.A))
                {
                    Dx = -3;
                    Dy = 0;
                    Direction = Direction.Left;
                    IceTicks = 28;
                }
                else if(Keyboard.IsKeyDown(Key.S))
                {
                    Dx = 0;
                    Dy = 3;
                    Direction = Direction.Down;
                    IceTicks = 28;
                }
                else if(Keyboard.IsKeyDown(Key.D))
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
            if(e.KeyCode == Keys.Space && Ammo > 0 && !ShootPressed && !ShotDelayTimer.Enabled && !RespawnTimer.Enabled)
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
                Rect = new Rectangle(576, 832, 64, 64);
                base.Respawn();
            }
        }
    }
}