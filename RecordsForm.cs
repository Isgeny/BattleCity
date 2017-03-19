using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class RecordsForm : AbstractForm
    {
        private GUIObject btnMainMenu;

        private SortedDictionary<int, string> records;

        public RecordsForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            throw new System.NotImplementedException();
        }

        ~RecordsForm()
        {
            throw new System.NotImplementedException();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void Subscribe()
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        private void OnBtnMainMenuClicked(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void AddRecord(string name, int points)
        {
            throw new System.NotImplementedException();
        }

        private void LoadRecordsFromFile()
        {
            throw new System.NotImplementedException();
        }

        private void WriteRecordsToFile()
        {
            throw new System.NotImplementedException();
        }
    }
}