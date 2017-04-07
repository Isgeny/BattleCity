﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace BattleCity
{
    public class SecondPlayerTank : PlayerTank
    {
        public SecondPlayerTank(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            Points = 777;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.KeyDown += OnKeyDown;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.KeyDown -= OnKeyDown;
        }

        protected override void OnMoveTimerTick(object sender, EventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.W))
            {
                Dx = 0.0f;
                Dy = -2.75f;
                Direction = Direction.Up;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.A))
            {
                Dx = -2.75f;
                Dy = 0.0f;
                Direction = Direction.Left;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.S))
            {
                Dx = 0.0f;
                Dy = 2.75f;
                Direction = Direction.Down;
                IceTicks = 28;
            }
            else if(Keyboard.IsKeyDown(Key.D))
            {
                Dx = 2.75f;
                Dy = 0.0f;
                Direction = Direction.Right;
                IceTicks = 28;
            }
            else
            {
                Dx = 0.0f;
                Dy = 0.0f;
            }
            base.OnMoveTimerTick(sender, e);
        }

        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space && Ammo > 0 && !ShootPressed)
            {
                Ammo--;
                Shell shell = new Shell(GUIForm, this);
                InvokeShoot(new ShellEventArgs(shell));
                ShootPressed = true;
            }
        }

        protected override void Respawn()
        {
            RectangleF newRect = new RectangleF(576.0f, 832.0f, Rect.Width, Rect.Height);
            Rect = newRect;
        }
    }
}