using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class Concrete : GraphicsObject
    {
        public Concrete(GUIForm guiForm, Rectangle rect) : base(guiForm, rect)
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
            Rectangle clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                Graphics g = e.Graphics;
                g.DrawImage(Properties.Resources.Tile_1, Rect.X, Rect.Y);
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(Rect.IntersectsWith(e.Rect))
            {
                if(sender is PlayerTank)
                    ((PlayerTank)sender).StopMoving();
                else if(sender is CompTank)
                    ((CompTank)sender).StopMoving();
                else if(sender is Shell)
                {
                    Shell s = sender as Shell;
                    s.InvokeDestroyed();
                    Tank creator = s.Creator;
                    if(creator is PlayerTank && creator.Stars == 3 || creator is CompTank && creator.Gun)
                        InvokeDestroyed();
                }
            }
        }
    }
}