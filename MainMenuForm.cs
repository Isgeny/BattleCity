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
            GUIObjs.AddLast(new SelectButton(GUIForm, new RectangleF(360.0f, 440.0f, 0.0f, 0.0f), "1 PLAYER", true));
            GUIObjs.AddLast(new SelectButton(GUIForm, new RectangleF(360.0f, 520.0f, 0.0f, 0.0f), "2 PLAYER"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new RectangleF(360.0f, 600.0f, 0.0f, 0.0f), "CONSTRUCTION"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new RectangleF(360.0f, 680.0f, 0.0f, 0.0f), "OPTIONS"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new RectangleF(360.0f, 760.0f, 0.0f, 0.0f), "RECORDS"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new RectangleF(360.0f, 840.0f, 0.0f, 0.0f), "EXIT"));

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
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GUIForm.Size));
            g.DrawImageUnscaled(Properties.Resources.Main_Title, 144, 90);
            g.DrawString("MADE BY ISAEV EVGENY 8I52 2017", MyFont.GetFont(12), new SolidBrush(Color.Gray), 20.0f, 920.0f);
            Properties.Settings s = Properties.Settings.Default;
            g.DrawString(s.P1Name[0].ToString() + s.P1Name[1].ToString() + "-" + GameManager.Game.P1Tank.Points +
                         " HI-" + GameManager.Records.GetHighestRecord() + " " +
                         s.P2Name[0].ToString() + s.P2Name[1].ToString() + "-" + GameManager.Game.P2Tank.Points,
                         MyFont.GetFont(19), new SolidBrush(Color.White), 20.0f, 30.0f);
        }

        private void OnBtn1PlayerClick(object sender, EventArgs e)
        {
            GameManager.SetGameForm();
        }

        private void OnBtn2PlayerClick(object sender, EventArgs e)
        {
            GameManager.SetGameForm();
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