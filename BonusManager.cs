using System;
using System.Collections.Generic;
using System.Drawing;

namespace BattleCity
{
    public class BonusManager : Object
    {
        private List<Bonus> _bonuses;
        private Bonus _currentBonus;

        public BonusManager(GUIForm guiForm) : base(guiForm, new RectangleF())
        {
            _bonuses = new List<Bonus>();
            _bonuses.Add(new Life(guiForm));
            _bonuses.Add(new Star(guiForm));
            _bonuses.Add(new Bomb(guiForm));
            _bonuses.Add(new Watch(guiForm));
            _bonuses.Add(new Showel(guiForm));
            _bonuses.Add(new Helmet(guiForm));
            _bonuses.Add(new Ship(guiForm));
            _bonuses.Add(new Gun(guiForm));
        }

        public override void Subscribe()
        {
            Random rand = new Random();
            _currentBonus = _bonuses[rand.Next(_bonuses.Count - 1)];
            _currentBonus.Subscribe();
        }

        public override void Unsubscribe()
        {
            _currentBonus.Unsubscribe();
        }

        public override void SubscribeToCheckPosition(Object obj)
        {
            _currentBonus.SubscribeToCheckPosition(obj);
        }

        public override void UnsubscribeFromCheckPosition(Object obj)
        {
            _currentBonus.UnsubscribeFromCheckPosition(obj);
        }

        private void OnBonusTankShooted(object sender, EventArgs e)
        {

        }
    }
}