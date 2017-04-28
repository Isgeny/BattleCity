using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class StageNumberForm : AbstractForm
    {
        public StageNumberForm(GUIForm guiForm, FormsManager formsManager) : base(guiForm, formsManager)
        {
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            var gameFormTimer = new Timer();
            gameFormTimer.Interval = 3000;
            gameFormTimer.Tick += OnStageNumberFormTimer;
            gameFormTimer.Start();
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.Paint -= OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0, 1024, 960);
            g.DrawString("STAGE " + FormsManager.Game.Field.Stage.ToString(), MyFont.GetFont(18), Brushes.Black, 400, 450);
        }

        private void OnStageNumberFormTimer(object sender, EventArgs e)
        {
            var gameFormTimer = sender as Timer;
            gameFormTimer.Stop();
            gameFormTimer.Tick -= OnStageNumberFormTimer;
            FormsManager.SetGameForm();
        }
    }
}