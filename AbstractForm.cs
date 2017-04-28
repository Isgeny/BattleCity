namespace BattleCity
{
    public abstract class AbstractForm : Object
    {
        public FormsManager FormsManager { get; private set; }

        public AbstractForm(GUIForm guiForm, FormsManager formsManager) : base(guiForm)
        {
            FormsManager = formsManager;
        }

        public override void Subscribe()
        {
            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            GUIForm.Invalidate();
        }
    }
}