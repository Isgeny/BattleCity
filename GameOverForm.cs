using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameOverForm : AbstractForm
    {
        public GameOverForm(GUIForm guiForm, FormsManager formsManager) : base(guiForm, formsManager)
        {
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            var gameFormTimer = new Timer();
            gameFormTimer.Interval = 3000;
            gameFormTimer.Tick += OnGameFormTimer;
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
            g.FillRectangle(Brushes.Black, GUIForm.ClientRectangle);
            g.DrawImageUnscaled(Properties.Resources.Game_Over, 272, 319);
        }

        private void OnGameFormTimer(object sender, EventArgs e)
        {
            var gameFormTimer = sender as Timer;
            gameFormTimer.Stop();
            gameFormTimer.Tick -= OnGameFormTimer;
            FormsManager.SetMainMenuForm();
        }
    }
}