using System;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;

namespace BattleCity
{
    public abstract class Bonus : Object
    {
        public event EventHandler PlayerTook;
        public event EventHandler CompTook;

        public Bonus(GUIForm guiForm) : base(guiForm, new RectangleF())
        {
            MoveToNewPosition();
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                ResourceManager rm = Properties.Resources.ResourceManager;
                string filename = "Bonus_" + GetType().Name;
                Bitmap bmp = (Bitmap)rm.GetObject(filename);
                g.DrawImage(bmp, Rect, new RectangleF(0.0f, 0.0f, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
            }
        }

        private void MoveToNewPosition()
        {
            Random rand = new Random();
            Rect = new RectangleF(rand.Next(26)*32.0f, rand.Next(26)*32.0f, 64.0f, 64.0f);
        }

        protected void InvokePlayerTook()
        {
            PlayerTook.Invoke(this, new EventArgs());
        }

        protected void InvokeCompTook()
        {
            CompTook.Invoke(this, new EventArgs());
        }  
    }
}