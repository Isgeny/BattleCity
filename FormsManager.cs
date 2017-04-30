namespace BattleCity
{
    public class FormsManager
    {
        private MainMenuForm MainMenu           { get; set; }
        public GameForm Game                    { get; private set; }
        private StageNumberForm StageNumber     { get; set; }
        private GameOverForm GameOver           { get; set; }
        private ConstructionForm Construction   { get; set; }
        private OptionsForm Options             { get; set; }
        public RecordsForm Records              { get; private set; }

        private AbstractForm _activeForm;
        public AbstractForm ActiveForm
        {
            get { return _activeForm; }
            set
            {
                _activeForm?.Unsubscribe();
                _activeForm = value;
                _activeForm.Subscribe();
            }
        }

        public FormsManager(GUIForm guiForm)
        {
            MainMenu        = new MainMenuForm(guiForm, this);
            Game            = new GameForm(guiForm, this);
            StageNumber     = new StageNumberForm(guiForm, this);
            GameOver        = new GameOverForm(guiForm, this);
            Construction    = new ConstructionForm(guiForm, this, Game.Field);
            Options         = new OptionsForm(guiForm, this);
            Records         = new RecordsForm(guiForm, this);

            ActiveForm = MainMenu;
            SetMainMenuForm();
        }

        public void SetMainMenuForm()
        {
            ActiveForm = MainMenu;
        }

        public void SetGameForm()
        {
            ActiveForm = Game; 
        }

        public void SetStageNumberForm()
        {
            ActiveForm = StageNumber;
        }

        public void SetGameOverForm()
        {
            ActiveForm = GameOver;
        }

        public void SetConstructionForm()
        {
            ActiveForm = Construction;
        }

        public void SetOptionsForm()
        {
            ActiveForm = Options;
        }

        public void SetRecordsForm()
        {
            ActiveForm = Records;
        }
    }
}