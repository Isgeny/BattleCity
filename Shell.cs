﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;

namespace BattleCity
{
    public class Shell : DynamicObject
    {
        private int power;
        private Tank creator;

        public Shell(GUIForm guiForm, Tank creator) : base(guiForm, new RectangleF(), creator.Direction)
        {
            this.creator = creator;
            MoveTimer.Tick += OnMoveTimer;
            MoveTimer.Start();

            float speed = 10.0f;
            if(creator is PlayerTank && creator.Stars >= 1)
            {
                speed = 20.0f;
            }

            switch(Direction)
            {
                case Direction.Up:
                    Dx = 0.0f;
                    Dy = -speed;
                    Rect = new RectangleF(creator.Rect.X + 22.0f, creator.Rect.Y - 8.0f, 16.0f, 16.0f);
                    break;
                case Direction.Left:
                    Dx = -speed;
                    Dy = 0.0f;
                    Rect = new RectangleF(creator.Rect.X - 8.0f, creator.Rect.Y + 22.0f, 16.0f, 16.0f);
                    break;
                case Direction.Down:
                    Dx = 0.0f;
                    Dy = speed;
                    Rect = new RectangleF(creator.Rect.X + 22.0f, creator.Rect.Y + 55.0f, 16.0f, 16.0f);
                    break;
                case Direction.Right:
                    Dx = speed;
                    Dy = 0.0f;
                    Rect = new RectangleF(creator.Rect.X + 55.0f, creator.Rect.Y + 22.0f, 16.0f, 16.0f);
                    break;
            }
        }

        public int Power
        {
            get { return power; }
        }

        public Tank Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                ResourceManager rm = Properties.Resources.ResourceManager;
                string filename = "Shell_" + (int)Direction;
                Bitmap bmp = (Bitmap)rm.GetObject(filename);
                g.DrawImage(bmp, Rect, new RectangleF(0.0f, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMoveTimer(object sender, EventArgs e)
        {
            InvokeCheckPosition(new RectEventArgs(new RectangleF(Rect.X + Dx, Rect.Y + Dy, Rect.Width, Rect.Height)));
            Move();
        }
    }
}