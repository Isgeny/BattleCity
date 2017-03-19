using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameForm : AbstractForm
    {
        private Bonus bonus;

        private CompTanksManager cTanksManager;

        private Field field;

        private PlayerTanksManager pTanksManager;

        public GameForm(GUIForm guiForm, GameManager gameManager, int players) : base(guiForm, gameManager)
        {
            throw new System.NotImplementedException();
        }

        public Field Field
        {
            get => default(Field);
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
    }
}