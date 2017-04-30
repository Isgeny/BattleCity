using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class MainMenuForm : MenuForm
    {
        private GUIObject _btn1Player;
        private GUIObject _btn2Player;
        private GUIObject _btnConstruction;
        private GUIObject _btnOptions;
        private GUIObject _btnRecords;
        private GUIObject _btnExit;

        public MainMenuForm(GUIForm guiForm, FormsManager formsManager) : base(guiForm, formsManager)
        {
            InitializeComponents();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            foreach(var component in Components)
                component.Subscribe();

            _btn1Player.Clicked         += OnBtn1PlayerClick;
            _btn2Player.Clicked         += OnBtn2PlayerClick;
            _btnConstruction.Clicked    += OnBtnConstructionClicked;
            _btnOptions.Clicked         += OnBtnOptionsClicked;
            _btnRecords.Clicked         += OnBtnRecords;
            _btnExit.Clicked            += OnBtnExitClicked;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.Paint -= OnPaint;

            foreach(var component in Components)
                component.Unsubscribe();

            _btn1Player.Clicked         -= OnBtn1PlayerClick;
            _btn2Player.Clicked         -= OnBtn2PlayerClick;
            _btnConstruction.Clicked    -= OnBtnConstructionClicked;
            _btnOptions.Clicked         -= OnBtnOptionsClicked;
            _btnRecords.Clicked         -= OnBtnRecords;
            _btnExit.Clicked            -= OnBtnExitClicked;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(Brushes.Black, GUIForm.DisplayRectangle);
            g.DrawImageUnscaled(Properties.Resources.Main_Title, 144, 90);
            g.DrawString("MADE BY ISAEV EVGENY 8I52 2017", MyFont.GetFont(12), Brushes.Gray, 20, 920);
            Properties.Settings s = Properties.Settings.Default;
            g.DrawString(s.P1Name.Substring(0, 2) + "-" + FormsManager.Game.Field.PlayersManager.P1Tank.Points, MyFont.GetFont(19), Brushes.White, 20, 30);
            g.DrawString("HI-" + FormsManager.Records.GetHighestRecord(), MyFont.GetFont(19), Brushes.White, 350, 30);
            g.DrawString(s.P2Name.Substring(0, 2) + "-" + FormsManager.Game.Field.PlayersManager.P2Tank.Points, MyFont.GetFont(19), Brushes.White, 700, 30);
        }

        private void OnBtn1PlayerClick(object sender, EventArgs e)
        {
            FormsManager.SetStageNumberForm();
            FormsManager.Game.SetPlayers(1);
        }

        private void OnBtn2PlayerClick(object sender, EventArgs e)
        {
            FormsManager.SetStageNumberForm();
            FormsManager.Game.SetPlayers(2);
        }

        private void OnBtnConstructionClicked(object sender, EventArgs e)
        {
            FormsManager.SetConstructionForm();
        }

        private void OnBtnOptionsClicked(object sender, EventArgs e)
        {
            FormsManager.SetOptionsForm();
        }

        private void OnBtnRecords(object sender, EventArgs e)
        {
            FormsManager.SetRecordsForm();
        }

        private void OnBtnExitClicked(object sender, EventArgs e)
        {
            GUIForm.Close();
        }

        private void InitializeComponents()
        {
            _btn1Player         = new SelectButton(GUIForm, new Point(360, 440), "1 PLAYER", true);
            _btn2Player         = new SelectButton(GUIForm, new Point(360, 520), "2 PLAYER");
            _btnConstruction    = new SelectButton(GUIForm, new Point(360, 600), "CONSTRUCTION");
            _btnOptions         = new SelectButton(GUIForm, new Point(360, 680), "OPTIONS");
            _btnRecords         = new SelectButton(GUIForm, new Point(360, 760), "RECORDS");
            _btnExit            = new SelectButton(GUIForm, new Point(360, 840), "EXIT");

            Components.AddLast(_btn1Player);
            Components.AddLast(_btn2Player);
            Components.AddLast(_btnConstruction);
            Components.AddLast(_btnOptions);
            Components.AddLast(_btnRecords);
            Components.AddLast(_btnExit);

            CurrentComponent = Components.First;
        }
    }
}