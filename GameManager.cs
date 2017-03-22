using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameManager
    {
        private AbstractForm activeForm;
        private MainMenuForm mainMenu;
        private GameForm game;
        private ConstructionForm construction;
        private OptionsForm options;
        private RecordsForm records;      

        public GameManager(GUIForm guiForm)
        {
            mainMenu    = new MainMenuForm(guiForm, this);
            game        = new GameForm(guiForm, this, 1);

            ActiveForm = mainMenu;
        }

        public AbstractForm ActiveForm
        {
            get { return activeForm; }
            set
            {
                activeForm = value;
                activeForm.Subscribe();
            }
        }

        public MainMenuForm MainMenu
        {
            get { return mainMenu; }
        }

        public GameForm Game
        {
            get { return game; }
        }

        public ConstructionForm Construction
        {
            get { return construction; }
        }

        public OptionsForm Options
        {
            get { return options; }
        }

        public RecordsForm Records
        {
            get { return records; }
        }
    }
}