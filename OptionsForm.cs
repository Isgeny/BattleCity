using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class OptionsForm : AbstractForm
    {
        public OptionsForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            GUIObjs.AddLast(new OptionButton(GUIForm, new Rectangle(260, 440, 400, 28), "FRIENDLY FIRE", Properties.Settings.Default.FriendlyFire, true));
            GUIObjs.AddLast(new OptionButton(GUIForm, new Rectangle(260, 520, 400, 28), "AI USE BONUS", Properties.Settings.Default.AIUseBonus));
            GUIObjs.AddLast(new NameBox(GUIForm, new Rectangle(260, 600, 250, 41), Properties.Settings.Default.P1Name, "P1 NAME:"));
            GUIObjs.AddLast(new NameBox(GUIForm, new Rectangle(260, 680, 250, 41), Properties.Settings.Default.P2Name, "P2 NAME:"));
            GUIObjs.AddLast(new SelectButton(GUIForm, new Rectangle(360, 840, 0, 0), "MAIN MENU"));

            CurrentGUIObj = GUIObjs.First;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            foreach(GUIObject option in GUIObjs)
                option.Subscribe();

            GUIObjs.Last.Value.Clicked += OnBtnMainMenuClicked;

            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.Paint -= OnPaint;

            foreach(GUIObject option in GUIObjs)
                option.Unsubscribe();

            GUIObjs.Last.Value.Clicked -= OnBtnMainMenuClicked;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GUIForm.Size));
            g.DrawImageUnscaled(Properties.Resources.Options, 80, 30);
        }

        private void OnBtnMainMenuClicked(object sender, EventArgs e)
        {
            LinkedListNode<GUIObject> option = GUIObjs.First;
            Properties.Settings.Default.FriendlyFire = ((OptionButton)option.Value).Enabled;
            option = option.Next;
            Properties.Settings.Default.AIUseBonus = ((OptionButton)option.Value).Enabled;
            option = option.Next;
            Properties.Settings.Default.P1Name = option.Value.Text;
            option = option.Next;
            Properties.Settings.Default.P2Name = option.Value.Text;
            Properties.Settings.Default.Save();
            GameManager.SetMainMenuForm();
        }
    }
}