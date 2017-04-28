using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class MenuForm : AbstractForm
    {
        public LinkedListNode<GUIObject> CurrentComponent { get; set; }
        public LinkedList<GUIObject> Components { get; private set; }

        public MenuForm(GUIForm guiForm, FormsManager formManager) : base(guiForm, formManager)
        {
            Components = new LinkedList<GUIObject>();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.KeyDown += OnKeyDown;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                CurrentComponent.Value.Selected = false;
                if(e.KeyCode == Keys.Down)
                    CurrentComponent = CurrentComponent.NextOrFirst();
                else if(e.KeyCode == Keys.Up)
                    CurrentComponent = CurrentComponent.PreviousOrLast();
                CurrentComponent.Value.Selected = true;
            }
        }
    }
}