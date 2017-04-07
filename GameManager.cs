﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameManager
    {
        private GUIForm _guiForm;
        private AbstractForm _activeForm;
        private MainMenuForm _mainMenu;
        private GameForm _game;
        private ConstructionForm _construction;
        private OptionsForm _options;
        private RecordsForm _records;      

        public GameManager(GUIForm guiForm)
        {
            _guiForm = guiForm;
            _mainMenu        = new MainMenuForm(guiForm, this);
            _game            = new GameForm(guiForm, this);
            _construction    = new ConstructionForm(guiForm, this);
            _options         = new OptionsForm(guiForm, this);
            _records         = new RecordsForm(guiForm, this);

            _activeForm = _mainMenu;
            SetMainMenuForm();
        }

        public AbstractForm ActiveForm
        {
            get { return _activeForm; }
            set
            {
                _activeForm.Unsubscribe();
                _activeForm = value;
                _activeForm.Subscribe();
                _guiForm.Invalidate();
            }
        }

        public void SetMainMenuForm()
        {
            ActiveForm = _mainMenu;
        }

        public void SetGameForm()
        {
            ActiveForm = _game;
        }

        public void SetConstructionForm()
        {
            ActiveForm = _construction;
        }

        public void SetOptionsForm()
        {
            ActiveForm = _options;
        }

        public void SetRecordsForm()
        {
            ActiveForm = _records;
        }

        public GameForm Game
        {
            get { return _game; }
        }

        public RecordsForm Records
        {
            get { return _records; }
        }
    }
}