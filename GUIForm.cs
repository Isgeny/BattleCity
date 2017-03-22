using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public partial class GUIForm : Form
    {
        private Timer mainTimer;
        private int fps;
        private System.Timers.Timer fpstimer;

        public GUIForm()
        {
            InitializeComponent();
         
            DoubleBuffered = true;

            fpstimer = new System.Timers.Timer(1000);
            fpstimer.Elapsed += (sender1, args) =>
            {
                Update(delegate
                {
                    Text = "FPS: " + fps;
                    fps = 0;
                });
            };
            fpstimer.Start();

            mainTimer = new Timer();
            mainTimer.Interval = 15;
            mainTimer.Tick += OnMainTimerTick;
            mainTimer.Start();
        }

        public void Update(MethodInvoker callback)
        {
            if(IsDisposed || Disposing)
                return;

            try
            {
                if(InvokeRequired)
                    Invoke(callback);
                else
                    callback();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void OnMainTimerTick(object sender, EventArgs e)
        {
            fps++;
        }
    }
}