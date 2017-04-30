using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameForm : AbstractForm
    {
        private Field _field;

        public GameForm(GUIForm guiForm, FormsManager gameManager) : base(guiForm, gameManager)
        {
            _field = new Field(guiForm, new Rectangle(64, 64, 832, 832), this);
        }

        public Field Field
        {
            get { return _field; }
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            _field.Subscribe();
            _field.GameOver     += OnGameOver;
            _field.NextStage    += OnNextStage;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.Paint -= OnPaint;

            _field.Unsubscribe();
            _field.GameOver     -= OnGameOver;
            _field.NextStage    -= OnNextStage;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0, 1024, 960);
            g.FillRectangle(Brushes.Black, 64, 64, 832, 832);
        }

        private void OnNextStage(object sender, EventArgs e)
        {
            FormsManager.SetStageNumberForm();
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            FormsManager.SetGameOverForm();
        }

        public void SetPlayers(int players)
        {
            _field.SetPlayers(players);            
        }
    }
}