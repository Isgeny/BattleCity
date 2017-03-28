using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class OptionsForm : AbstractForm
    {
        private int position;
        private List<GUIObject> optionsObjs;

        public OptionsForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            position = 0;

            optionsObjs = new List<GUIObject>();
            optionsObjs.Add(new OptionButton(GUIForm, new RectangleF(160.0f, 390.0f, 620.0f, 28.0f), "FRIENDLY FIRE", Properties.Settings.Default.FriendlyFire, true));
            optionsObjs.Add(new OptionButton(GUIForm, new RectangleF(160.0f, 470.0f, 620.0f, 28.0f), "AI USE BONUS", Properties.Settings.Default.AIUseBonus));
            optionsObjs.Add(new NameBox(GUIForm, new RectangleF(160.0f, 550.0f, 360.0f, 41.0f), Properties.Settings.Default.P1Name, "P1 NAME:"));
            optionsObjs.Add(new NameBox(GUIForm, new RectangleF(160.0f, 630.0f, 360.0f, 41.0f), Properties.Settings.Default.P2Name, "P2 NAME:"));
            optionsObjs.Add(new SelectButton(GUIForm, new RectangleF(310.0f, 790.0f, 0.0f, 0.0f), "MAIN MENU"));
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            GUIForm.KeyDown += OnKeyDown;

            foreach(GUIObject obj in optionsObjs)
            {
                obj.SubscribeToPaint();
                obj.SubscribeToKeyDown();
            }

            optionsObjs[4].Clicked += OnBtnMainMenuClicked;

            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.KeyDown -= OnKeyDown;

            foreach(GUIObject obj in optionsObjs)
            {
                obj.UnsubscribeFromPaint();
                obj.UnsubscribeFromKeyDown();
            }

            optionsObjs[4].Clicked -= OnBtnMainMenuClicked;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GUIForm.Size));
            g.DrawImage(Properties.Resources.Options, 80, 30);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                optionsObjs[position].Selected = false;
                position = ++position % 5;
                optionsObjs[position].Selected = true;
            }
            else if(e.KeyCode == Keys.Up)
            {
                optionsObjs[position].Selected = false;
                position = (position - 1 < 0) ? 4 : --position;
                optionsObjs[position].Selected = true;
            }
        }

        private void OnBtnMainMenuClicked(object sender, EventArgs e)
        {
            Unsubscribe();
            Properties.Settings.Default.FriendlyFire = ((OptionButton)optionsObjs[0]).Enabled;
            Properties.Settings.Default.AIUseBonus = ((OptionButton)optionsObjs[1]).Enabled;
            Properties.Settings.Default.P1Name = optionsObjs[2].Text;
            Properties.Settings.Default.P2Name = optionsObjs[3].Text;
            Properties.Settings.Default.Save();
            GameManager.ActiveForm = GameManager.MainMenu;
        }
    }
}