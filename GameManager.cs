using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    public class GameManager
    {
        private AbstractForm activeForm;
        private MainMenuForm mainMenu;
        private GameForm game;
        private ConstructionForm construction;
        private BattleCity.OptionsForm options;
        private RecordsForm records;

        public GameManager(GUIForm guiForm)
        {
            throw new System.NotImplementedException();
        }

        public AbstractForm ActiveForm
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public RecordsForm Records
        {
            get => default(int);
        }

        public GameForm Game
        {
            get => default(int);
        }
    }
}