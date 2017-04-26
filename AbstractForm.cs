using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public abstract class AbstractForm : Object
    {
        private GameManager _gameManager;
        private LinkedList<GUIObject> _GUIObjs;
        private LinkedListNode<GUIObject> _currentGUIObj;

        public AbstractForm(GUIForm guiForm, GameManager gameManager) : base(guiForm)
        {
            _gameManager = gameManager;
            _GUIObjs = new LinkedList<GUIObject>();
        }

        public GameManager GameManager
        {
            get { return _gameManager; }
        }

        public LinkedListNode<GUIObject> CurrentGUIObj
        {
            get { return _currentGUIObj; }
            set { _currentGUIObj = value; }
        }

        public LinkedList<GUIObject> GUIObjs
        {
            get { return _GUIObjs; }
            set { _GUIObjs = value; }
        }

        public override void Subscribe()
        {
            GUIForm.KeyDown += OnKeyDown;
        }

        public override void Unsubscribe()
        {
            GUIForm.KeyDown -= OnKeyDown;
        }

        protected virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                CurrentGUIObj.Value.Selected = false;
                if(e.KeyCode == Keys.Down)
                    CurrentGUIObj = CurrentGUIObj.NextOrFirst();
                else if(e.KeyCode == Keys.Up)
                    CurrentGUIObj = CurrentGUIObj.PreviousOrLast();
                CurrentGUIObj.Value.Selected = true;
            }
        }
    }
}