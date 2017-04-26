using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameOverForm : AbstractForm
    {
        public GameOverForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            Timer gameFormTimer = new Timer();
            gameFormTimer.Interval = 4000;
            gameFormTimer.Tick += OnGameFormTimer;
            gameFormTimer.Start();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, GUIForm.ClientRectangle);
            g.DrawImageUnscaled(Properties.Resources.Game_Over, 272, 319);
        }

        private void OnGameFormTimer(object sender, EventArgs e)
        {
            Timer gameFormTimer = sender as Timer;
            gameFormTimer.Stop();
            gameFormTimer.Tick -= OnGameFormTimer;
            GameManager.SetMainMenuForm();
        }
    }
}