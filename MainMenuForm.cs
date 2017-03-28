using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class MainMenuForm : AbstractForm
    {
        private int position;
        private List<GUIObject> btns;
        private GUIObject btn1Player;
        private GUIObject btn2Player;
        private GUIObject btnConstruction;
        private GUIObject btnOptions;
        private GUIObject btnRecords;
        private GUIObject btnExit;

        public MainMenuForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            position = 0;

            btn1Player        = new SelectButton(GUIForm, new RectangleF(270.0f, 390.0f, 0.0f, 0.0f), "1 PLAYER", true);
            btn2Player        = new SelectButton(GUIForm, new RectangleF(270.0f, 470.0f, 0.0f, 0.0f), "2 PLAYER");
            btnConstruction   = new SelectButton(GUIForm, new RectangleF(270.0f, 550.0f, 0.0f, 0.0f), "CONSTRUCTION");
            btnOptions        = new SelectButton(GUIForm, new RectangleF(270.0f, 630.0f, 0.0f, 0.0f), "OPTIONS");
            btnRecords        = new SelectButton(GUIForm, new RectangleF(270.0f, 710.0f, 0.0f, 0.0f), "RECORDS");
            btnExit           = new SelectButton(GUIForm, new RectangleF(270.0f, 790.0f, 0.0f, 0.0f), "EXIT");

            btns = new List<GUIObject>();
            btns.Add(btn1Player);
            btns.Add(btn2Player);
            btns.Add(btnConstruction);
            btns.Add(btnOptions);
            btns.Add(btnRecords);
            btns.Add(btnExit);

            guiForm.Invalidate();
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            GUIForm.KeyDown += OnKeyDown;

            foreach(GUIObject btn in btns)
            {
                btn.SubscribeToPaint();
                btn.SubscribeToKeyDown();
            }

            btn1Player.Clicked      += OnBtn1PlayerClick;
            btn2Player.Clicked      += OnBtn2PlayerClick;
            btnConstruction.Clicked += OnBtnConstructionClicked;
            btnOptions.Clicked      += OnBtnOptionsClicked;
            btnRecords.Clicked      += OnBtnRecords;
            btnExit.Clicked         += OnBtnExitClicked;

            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.KeyDown -= OnKeyDown;

            foreach(GUIObject btn in btns)
            {
                btn.UnsubscribeFromPaint();
                btn.UnsubscribeFromKeyDown();
            }

            btn1Player.Clicked      -= OnBtn1PlayerClick;
            btn2Player.Clicked      -= OnBtn2PlayerClick;
            btnConstruction.Clicked -= OnBtnConstructionClicked;
            btnOptions.Clicked      -= OnBtnOptionsClicked;
            btnRecords.Clicked      -= OnBtnRecords;
            btnExit.Clicked         -= OnBtnExitClicked;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GUIForm.Size));
            g.DrawImage(Properties.Resources.Main_Title, 144, 30);
            g.DrawString("MADE BY ISAEV EVGENY 8I52 2017", MyFont.GetFont(12), new SolidBrush(Color.White), 20.0f, 920.0f);
        }

        private void OnBtn1PlayerClick(object sender, EventArgs e)
        {
            Unsubscribe();
            GameManager.ActiveForm = GameManager.Game;
        }

        private void OnBtn2PlayerClick(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnBtnConstructionClicked(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnBtnOptionsClicked(object sender, EventArgs e)
        {
            Unsubscribe();
            GameManager.ActiveForm = GameManager.Options;
        }

        private void OnBtnRecords(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnBtnExitClicked(object sender, EventArgs e)
        {
            GUIForm.Close();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                btns[position].Selected = false;
                position = ++position % 6;
                btns[position].Selected = true;
            }
            else if(e.KeyCode == Keys.Up)
            {
                btns[position].Selected = false;
                position = (position - 1 < 0) ? 5 : --position;
                btns[position].Selected = true;
            }
        }   
    }
}