using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class BonusManager : Object
    {
        private Field _field;
        private List<Bonus> _bonuses;

        private Bonus _currentBonus;
        private Bonus CurrentBonus
        {
            get { return _currentBonus; }
            set
            {
                _currentBonus?.Unsubscribe();

                foreach(var playerTank in _field.PlayersManager.Tanks)
                    playerTank.CheckPosition -= _currentBonus?.GetCheckPositionListener();

                foreach(var compTank in _field.CompsManager.Tanks)
                    compTank.CheckPosition -= _currentBonus?.GetCheckPositionListener();

                _currentBonus = value;

                _currentBonus.Subscribe();

                foreach(var playerTank in _field.PlayersManager.Tanks)
                    playerTank.CheckPosition += _currentBonus?.GetCheckPositionListener();

                foreach(var compTank in _field.CompsManager.Tanks)
                    compTank.CheckPosition += _currentBonus?.GetCheckPositionListener();
            }
        }

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

            var bonusBomb = new Bomb(guiForm);
            bonusBomb.PlayerTook    += OnPlayerTookBomb;
            bonusBomb.CompTook      += OnCompTookBomb;
            _bonuses.Add(bonusBomb);

            var bonusWatch = new Watch(guiForm);
            bonusWatch.PlayerTook   += OnPlayerTookWatch;
            bonusWatch.CompTook     += OnCompTookWatch;
            _bonuses.Add(bonusWatch);

            var bonusShowel = new Showel(guiForm);
            bonusShowel.PlayerTook  += OnPlayerTookShowel;
            bonusShowel.CompTook    += OnCompTookShowel;
            _bonuses.Add(bonusShowel);

            foreach(Bonus bonus in _bonuses)
            {
                bonus.TankTook  += OnTankTookBonusOrBonusTimeUp;
                bonus.TimeUp    += OnTankTookBonusOrBonusTimeUp;
            }
        }

        public override void Subscribe()
        {
            GUIForm.KeyDown += OnKeyDown;   //DEBUG

            _field.CompsManager.BonusTankShoot += OnBonusTankShoot;
        }

        public override void Unsubscribe()
        {
            GUIForm.KeyDown -= OnKeyDown;   //DEBUG

            _field.CompsManager.BonusTankShoot -= OnBonusTankShoot;
        }

        private void GenerateRandomBonus()
        {
            CurrentBonus = _bonuses[GameRandom.RandNumber(0, _bonuses.Count - 1)];
        }

        //DEBUG
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.NumPad0)
                CurrentBonus = _bonuses[0];
            else if(e.KeyCode == Keys.NumPad1)
                CurrentBonus = _bonuses[1];
            else if(e.KeyCode == Keys.NumPad2)
                CurrentBonus = _bonuses[2];
            else if(e.KeyCode == Keys.NumPad3)
                CurrentBonus = _bonuses[3];
            else if(e.KeyCode == Keys.NumPad4)
                CurrentBonus = _bonuses[4];
            else if(e.KeyCode == Keys.NumPad5)
                CurrentBonus = _bonuses[5];
            else if(e.KeyCode == Keys.NumPad6)
                CurrentBonus = _bonuses[6];
            else if(e.KeyCode == Keys.NumPad7)
                CurrentBonus = _bonuses[7];
        }

        private void OnBonusTankShoot(object sender, EventArgs e)
        {
            GenerateRandomBonus();
        }

        private void OnTankTookBonusOrBonusTimeUp(object sender, EventArgs e)
        {
            foreach(var playerTank in _field.PlayersManager.Tanks)
                playerTank.CheckPosition -= _currentBonus?.GetCheckPositionListener();

            foreach(var compTank in _field.CompsManager.Tanks)
                compTank.CheckPosition -= _currentBonus?.GetCheckPositionListener();

            _currentBonus = null;
        }

        private void OnPlayerTookBomb(object sender, EventArgs e)
        {
            PlayerTookBomb?.Invoke(sender, new EventArgs());
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
    }
}