using System;
using System.Collections.Generic;

namespace BattleCity
{
    public class BonusManager : Object
    {
        private Field _field;
        private List<Bonus> _bonuses;
        private Bonus _currentBonus;

        public event EventHandler PlayerTookBomb;
        public event EventHandler CompTookBomb;
        public event EventHandler PlayerTookWatch;
        public event EventHandler CompTookWatch;
        public event EventHandler PlayerTookShowel;
        public event EventHandler CompTookShowel;

        public BonusManager(GUIForm guiForm, Field field) : base(guiForm)
        {
            _field = field;
            _bonuses = new List<Bonus>();

            _bonuses.Add(new Life(guiForm));
            _bonuses.Add(new Star(guiForm));
            _bonuses.Add(new Helmet(guiForm));
            _bonuses.Add(new Ship(guiForm));
            _bonuses.Add(new Gun(guiForm));

            Bonus bonus = new Bomb(guiForm);
            bonus.PlayerTook    += OnPlayerTookBomb;
            bonus.CompTook      += OnCompTookBomb;
            _bonuses.Add(bonus);

            bonus = new Watch(guiForm);
            bonus.PlayerTook += OnPlayerTookWatch;
            bonus.CompTook += OnCompTookWatch;
            _bonuses.Add(bonus);

            bonus = new Showel(guiForm);
            bonus.PlayerTook += OnPlayerTookShowel;
            bonus.CompTook += OnCompTookShowel;
            _bonuses.Add(bonus);

            foreach(Bonus b in _bonuses)
                b.TankTook += OnTankTookBonus;
        }

        public override void Subscribe()
        {
            _currentBonus.Subscribe();
            _currentBonus.TimeUp            += OnBonusTimeUp;
            foreach(Tank playerTank in _field.PlayersManager.Tanks)
                playerTank.CheckPosition    += _currentBonus.GetCheckPositionListener();
            foreach(Tank compTank in _field.CompsManager.Tanks)
                compTank.CheckPosition      += _currentBonus.GetCheckPositionListener();
        }

        public override void Unsubscribe()
        {
            _currentBonus.Unsubscribe();
            _currentBonus.TimeUp            -= OnBonusTimeUp;
            foreach(Tank playerTank in _field.PlayersManager.Tanks)
                playerTank.CheckPosition    -= _currentBonus.GetCheckPositionListener();
            foreach(Tank compTank in _field.CompsManager.Tanks)
                compTank.CheckPosition      -= _currentBonus.GetCheckPositionListener();
        }

        private void OnBonusTankShooted(object sender, EventArgs e)
        {
            if(_currentBonus != null)
                Unsubscribe();
            GenerateRandomBonus();
            Subscribe();
        }

        public EventHandler GetBonusTankShootedListener()
        {
            return OnBonusTankShooted;
        }

        private void OnBonusTimeUp(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        private void OnTankTookBonus(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        private void OnPlayerTookBomb(object sender, EventArgs e)
        {
            PlayerTookBomb?.Invoke(this, new EventArgs());
        }

        private void OnCompTookBomb(object sender, EventArgs e)
        {
            CompTookBomb?.Invoke(this, new EventArgs());
        }

        private void OnPlayerTookWatch(object sender, EventArgs e)
        {
            PlayerTookWatch?.Invoke(this, new EventArgs());
        }

        private void OnCompTookWatch(object sender, EventArgs e)
        {
            CompTookWatch?.Invoke(this, new EventArgs());
        }

        private void OnPlayerTookShowel(object sender, EventArgs e)
        {
            PlayerTookShowel?.Invoke(this, new EventArgs());
        }

        private void OnCompTookShowel(object sender, EventArgs e)
        {
            CompTookShowel?.Invoke(this, new EventArgs());
        }     

        private void GenerateRandomBonus()
        {
            _currentBonus = _bonuses[GameRandom.RandNumber(0, _bonuses.Count - 1)];
            //_currentBonus = _bonuses[7]; //DEBUG
        }
    }
}