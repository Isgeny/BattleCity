using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class OptionsForm : MenuForm
    {
        private GUIObject _btnFriendlyFire;
        private GUIObject _btnAIUseBonus;
        private GUIObject _nameBoxP1;
        private GUIObject _nameBoxP2;
        private GUIObject _btnMainMenu;

        public OptionsForm(GUIForm guiForm, FormsManager formsManager) : base(guiForm, formsManager)
        {
            InitializeComponents();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            foreach(var component in Components)
                component.Subscribe();

            _btnMainMenu.Clicked += OnBtnMainMenuClicked;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.Paint -= OnPaint;

            foreach(var component in Components)
                component.Unsubscribe();

            _btnMainMenu.Clicked -= OnBtnMainMenuClicked;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GUIForm.Size));
            g.DrawImageUnscaled(Properties.Resources.Options, 80, 30);
        }

        private void OnBtnMainMenuClicked(object sender, EventArgs e)
        {
            Properties.Settings.Default.FriendlyFire = ((OptionButton)_btnFriendlyFire).Enabled;
            Properties.Settings.Default.AIUseBonus = ((OptionButton)_btnAIUseBonus).Enabled;
            Properties.Settings.Default.P1Name = _nameBoxP1.Text;
            Properties.Settings.Default.P2Name = _nameBoxP2.Text;
            Properties.Settings.Default.Save();

            FormsManager.SetMainMenuForm();
        }

        private void InitializeComponents()
        {
            _btnFriendlyFire    = new OptionButton(GUIForm, new Point(260, 440), "FRIENDLY FIRE", Properties.Settings.Default.FriendlyFire, true);
            _btnAIUseBonus      = new OptionButton(GUIForm, new Point(260, 520), "AI USE BONUS", Properties.Settings.Default.AIUseBonus);
            _nameBoxP1          = new TextEdit(GUIForm, new Point(260, 600), Properties.Settings.Default.P1Name, "P1 NAME:");
            _nameBoxP2          = new TextEdit(GUIForm, new Point(260, 680), Properties.Settings.Default.P2Name, "P2 NAME:");
            _btnMainMenu        = new SelectButton(GUIForm, new Point(360, 840), "MAIN MENU");


            Components.AddLast(_btnFriendlyFire);
            Components.AddLast(_btnAIUseBonus);
            Components.AddLast(_nameBoxP1);
            Components.AddLast(_nameBoxP2);
            Components.AddLast(_btnMainMenu);

            CurrentComponent = Components.First;
        }
    }
}