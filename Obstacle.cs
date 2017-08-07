using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Obstacle : GameObject
    {
        public event EventHandler Destroyed;

        public Obstacle(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
        {
        }

        public override void Subscribe()
        {
            GUIForm.Paint   += OnPaint;
            Destroyed       += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint   -= OnPaint;
            Destroyed       -= OnDestroyed;
        }

        protected virtual void OnPaint(object sender, PaintEventArgs e)
        {
            if(Rect.IntersectsWith(e.ClipRectangle))
            {
                var g = e.Graphics;
                var rm = Properties.Resources.ResourceManager;
                var filename = "Tile_" + GetType().Name;
                var bmp = rm.GetObject(filename);
                g.DrawImageUnscaled(bmp as Bitmap, Rect);
            }
        }

        protected virtual void OnDestroyed(object sender, EventArgs e)
        {
            GUIForm.Invalidate(Rect);
        }

        public void InvokeDestroyed()
        {
            Destroyed?.Invoke(this, new EventArgs());
        }

        public void RemoveDestroyedListeners()
        {
            Destroyed = null;
        }
    }
}