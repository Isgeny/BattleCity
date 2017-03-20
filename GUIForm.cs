using System.Windows.Forms;
using System;
using System.Drawing;

namespace BattleCity
{
    public partial class GUIForm : Form
    {
        private Bitmap bitmap;
        private Timer mainTimer;

        public GUIForm()
        {
            InitializeComponent();
            
            DoubleBuffered = true;

            bitmap = new Bitmap(1024, 960);

            Paint += OnPaint;

            mainTimer = new Timer();
            mainTimer.Interval = 1000/60;
            mainTimer.Tick += OnMainTimerTick;
            mainTimer.Start();
        }

        public Bitmap Bitmap
        {
            get { return bitmap; }
        }

        private void OnMainTimerTick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(bitmap, 0, 0);
        }
    }
}