using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Bush : Object
    {
        public Bush(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            Destroyed += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            Destroyed -= OnDestroyed;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_3, Rect.X, Rect.Y);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is Shell)
                {
                    Shell s = sender as Shell;
                    if(s.Creator.Gun)
                    {
                        InvokeDestroyed();
                    }
                    else
                    {
                        GUIForm.Paint -= OnPaint;
                        GUIForm.Paint += OnPaint;
                    }
                }
                else if(sender is Tank)
                {
                    GUIForm.Paint -= OnPaint;
                    GUIForm.Paint += OnPaint;
                }
            }
        }
    }
}