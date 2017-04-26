using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class MainMenuForm : AbstractForm
    {
        public MainMenuForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            GUIObjs.AddLast(new SelectButton(GUIForm, new Rectangle(360, 440, 0, 0), "1 PLAYER", true));
            GUIObjs.AddLast(new SelectButton(GUIForm, new Rectangle(360, 520, 0, 0), "2 PLAYER"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new Rectangle(360, 600, 0, 0), "CONSTRUCTION"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new Rectangle(360, 680, 0, 0), "OPTIONS"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new Rectangle(360, 760, 0, 0), "RECORDS"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new Rectangle(360, 840, 0, 0), "EXIT"));

            CurrentGUIObj = GUIObjs.First;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            foreach(GUIObject obj in GUIObjs)
                obj.Subscribe();

            LinkedListNode<GUIObject> btn = GUIObjs.First;
            btn.Value.Clicked += OnBtn1PlayerClick;
            btn = btn.Next;
            btn.Value.Clicked += OnBtn2PlayerClick;
            btn = btn.Next;
            btn.Value.Clicked += OnBtnConstructionClicked;
            btn = btn.Next;
            btn.Value.Clicked += OnBtnOptionsClicked;
            btn = btn.Next;
            btn.Value.Clicked += OnBtnRecords;
            btn = btn.Next;
            btn.Value.Clicked += OnBtnExitClicked;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.Paint -= OnPaint;

            foreach(GUIObject obj in GUIObjs)
                obj.Unsubscribe();

            LinkedListNode<GUIObject> btn = GUIObjs.First;
            btn.Value.Clicked -= OnBtn1PlayerClick;
            btn = btn.Next;
            btn.Value.Clicked -= OnBtn2PlayerClick;
            btn = btn.Next;
            btn.Value.Clicked -= OnBtnConstructionClicked;
            btn = btn.Next;
            btn.Value.Clicked -= OnBtnOptionsClicked;
            btn = btn.Next;
            btn.Value.Clicked -= OnBtnRecords;
            btn = btn.Next;
            btn.Value.Clicked -= OnBtnExitClicked;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, GUIForm.DisplayRectangle);
            g.DrawImageUnscaled(Properties.Resources.Main_Title, 144, 90);
            g.DrawString("MADE BY ISAEV EVGENY 8I52 2017", MyFont.GetFont(12), Brushes.Gray, 20, 920);
            Properties.Settings s = Properties.Settings.Default;
            g.DrawString(s.P1Name.Substring(0, 2) + "-" + GameManager.Game.PlayerManager.P1Tank.Points, MyFont.GetFont(19), Brushes.White, 20,  30);
            g.DrawString("HI-" + GameManager.Records.GetHighestRecord(),                                MyFont.GetFont(19), Brushes.White, 350, 30);
            g.DrawString(s.P2Name.Substring(0, 2) + "-" + GameManager.Game.PlayerManager.P2Tank.Points, MyFont.GetFont(19), Brushes.White, 700, 30);
        }

        private void OnBtn1PlayerClick(object sender, EventArgs e)
        {
            GameManager.SetStageNumberForm();
            GameManager.Game.SetPlayers(1);
        }

        private void OnBtn2PlayerClick(object sender, EventArgs e)
        {
            GameManager.SetStageNumberForm();
            GameManager.Game.SetPlayers(2);
        }

        private void OnBtnConstructionClicked(object sender, EventArgs e)
        {
            GameManager.SetConstructionForm();
        }

        private void OnBtnOptionsClicked(object sender, EventArgs e)
        {
            GameManager.SetOptionsForm();
        }

        private void OnBtnRecords(object sender, EventArgs e)
        {
            GameManager.SetRecordsForm();
        }

        private void OnBtnExitClicked(object sender, EventArgs e)
        {
            GUIForm.Close();
        }
    }
}