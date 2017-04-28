using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Obstacle : GraphicsObject
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
            var clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                var g = e.Graphics;
                var rm = Properties.Resources.ResourceManager;
                var filename = "Tile_" + GetType().Name;
                var bmp = rm.GetObject(filename);
                g.DrawImageUnscaled(bmp as Bitmap, Rect);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
                if(sender is Tank)
                    TankCollision(sender as Tank);
                else if(sender is Shell)
                    ShellCollision(sender as Shell);
        }

        protected virtual void TankCollision(Tank tank)
        {
            tank.StopMoving();
        }

        protected virtual void ShellCollision(Shell shell)
        {
            shell.InvokeDestroyed();
        }

        protected virtual void OnDestroyed(object sender, EventArgs e)
        {
            GUIForm.Invalidate(Rect);
        }

        public void InvokeDestroyed()
        {
            Destroyed?.Invoke(this, new EventArgs());
        }
    }
}