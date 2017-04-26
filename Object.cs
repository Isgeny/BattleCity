namespace BattleCity
{
    public abstract class Object
    {
        private GUIForm _guiForm;

        public Object(GUIForm guiForm)
        {
            _guiForm = guiForm;
        }

        public GUIForm GUIForm
        {
            get { return _guiForm; }
            set { _guiForm = value; }
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();
    }
}