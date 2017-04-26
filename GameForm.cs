using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class GameForm : AbstractForm
    {
        private Field _field;
        private PlayersManager _playersManager;
        private CompsManager _compsManager;
        private List<Shell> _shells;
        private BonusManager _bonusManager;

        public GameForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            _bonusManager = new BonusManager(guiForm, this);
            _field          = new Field(guiForm, new Rectangle(64, 64, 832, 832), this, _bonusManager);
            _playersManager = new PlayersManager(guiForm, this, _bonusManager);
            _compsManager   = new CompsManager(guiForm, this, _bonusManager);
            _shells         = new List<Shell>();
        }

        public Field Field
        {
            get { return _field; }
        }

        public PlayersManager PlayerManager
        {
            get { return _playersManager; }
        }

        public CompsManager CompsManager
        {
            get { return _compsManager; }
        }

        public List<Shell> Shells
        {
            get { return _shells; }
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            _field.Subscribe();
            _field.HQDestroyed += OnHQDestroyed;

            _playersManager.Subscribe();
            _playersManager.TankShoot += OnShoot;
            _playersManager.TanksDestroyed += OnPlayerTanksDestroyed;

            _compsManager.Subscribe();
            _compsManager.TankShoot += OnShoot;
            _compsManager.TanksDestroyed += OnCompTanksDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            _field.Unsubscribe();
            _field.HQDestroyed -= OnHQDestroyed;

            _playersManager.Unsubscribe();
            _playersManager.TankShoot -= OnShoot;
            _playersManager.TanksDestroyed -= OnPlayerTanksDestroyed;

            _compsManager.Unsubscribe();
            _compsManager.TankShoot -= OnShoot;
            _compsManager.TanksDestroyed -= OnCompTanksDestroyed;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0, 1024, 960);
            g.FillRectangle(Brushes.Black, 64, 64, 832, 832);
        }

        private void OnShoot(object sender, ShellEventArgs e)
        {
            Shell s = e.Shell;
            s.Subscribe();
            s.Destroyed     += OnShellDestroyed;
            s.CheckPosition += _field.GetCheckPositionListener();
            foreach(GraphicsObject obstacle in _field.Obstacles)
                s.CheckPosition += obstacle.GetCheckPositionListener();
            foreach(Shell shell in _shells)
                shell.CheckPosition += s.GetCheckPositionListener();
            _shells.Add(s);
        }

        private void OnShellDestroyed(object sender, EventArgs e)
        {
            Shell s = sender as Shell;
            s.Destroyed -= OnShellDestroyed;
            s.CheckPosition -= _field.GetCheckPositionListener();
            foreach(GraphicsObject obstacle in _field.Obstacles)
                s.CheckPosition -= obstacle.GetCheckPositionListener();
            foreach(Shell shell in _shells)
                shell.CheckPosition -= s.GetCheckPositionListener();
            _shells.Remove(s);
        }

        private void OnHQDestroyed(object sender, EventArgs e)
        {
            Timer gameOverTimer = new Timer();
            gameOverTimer.Interval = 5000;
            gameOverTimer.Tick += OnGameOverTimer;
            gameOverTimer.Start();
        }

        private void OnPlayerTanksDestroyed(object sender, EventArgs e)
        {
            Timer gameOverTimer = new Timer();
            gameOverTimer.Interval = 5000;
            gameOverTimer.Tick += OnGameOverTimer;
            gameOverTimer.Start();
        }
        private void OnCompTanksDestroyed(object sender, EventArgs e)
        {
            GameManager.SetStageNumberForm();
        }

        public void Initialize()
        {
            _field.InitializeField();
            _playersManager.InitializeTanks();
            _compsManager.InitializeTanks();
        }

        public void SetPlayers(int players)
        {
            _playersManager.SetPlayers(players);
            Initialize();
        }

        private void OnGameOverTimer(object sender, EventArgs e)
        {
            Timer gameOverTimer = sender as Timer;
            gameOverTimer.Stop();
            gameOverTimer.Tick -= OnGameOverTimer;
            GameManager.SetGameOverForm();
        }
    }
}