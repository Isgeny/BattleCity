using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class AbstractForm
    {
        private GameManager gameManager;

        private GUIForm guiForm;

        public AbstractForm(GUIForm guiForm, GameManager gameManager)
        {
            throw new System.NotImplementedException();
        }

        public GameManager GameManager
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public GUIForm GUIForm
        {
            get => default(int);
        }

        public abstract void Subscribe();

        public abstract void Unsubscribe();
    }
}