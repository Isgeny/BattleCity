namespace BattleCity
{
    public abstract class AbstractForm
    {
        private GameManager gameManager;
        private GUIForm guiForm;

        public AbstractForm(GUIForm guiForm, GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.guiForm = guiForm;
        }

        public GameManager GameManager
        {
            get { return gameManager; }
        }

        public GUIForm GUIForm
        {
            get { return guiForm; }
        }

        public abstract void Subscribe();

        public abstract void Unsubscribe();
    }
}