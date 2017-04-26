using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class StageNumberForm : AbstractForm
    {
        public StageNumberForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            Timer gameFormTimer = new Timer();
            gameFormTimer.Interval = 3000;
            gameFormTimer.Tick += OnStageNumberFormTimer;
            gameFormTimer.Start();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0, 1024, 960);
            g.DrawString("STAGE " + GameManager.Game.Field.Stage.ToString(), MyFont.GetFont(18), Brushes.Black, 400, 450);
        }

        private void OnStageNumberFormTimer(object sender, EventArgs e)
        {
            Timer gameFormTimer = sender as Timer;
            gameFormTimer.Stop();
            gameFormTimer.Tick -= OnStageNumberFormTimer;
            GameManager.SetGameForm();
        }
    }
}