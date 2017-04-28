namespace BattleCity
{
    public abstract class Object
    {
        protected GUIForm GUIForm { get; set; }

        public Object(GUIForm guiForm)
        {
            GUIForm = guiForm;
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();
    }
}