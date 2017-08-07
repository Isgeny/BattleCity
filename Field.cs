using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class Field : GameObject
    {
        private int _stage;
        public int Stage
        {
            get { return _stage; }
            set
            {
                _stage = value;    //70 уровней по кругу

                if(!(_stage == 1 && Obstacles.Count != 0))
                {
                    //Очистка старого уровня
                    foreach(var obstacle in Obstacles)
                    {
                        foreach(var playerTank in PlayersManager.Tanks)
                            playerTank.CheckPosition -= obstacle.GetCheckPositionListener();

                        foreach(var compTank in CompsManager.Tanks)
                            compTank.CheckPosition -= obstacle.GetCheckPositionListener();
                    }

                    foreach(var shell in Shells)
                    {
                        shell.InvokeDestroyed();
                        shell.RemoveCheckPositionListeners();
                        shell.RemoveDestroyedListeners();
                    }

                    Obstacles.Clear();
                    _showelDelayTimer.Stop();
                    _watchDelayTimer.Stop();

                    //Загрузка нового уровня
                    LoadStage();
                }

                //Инициалиалиация полей танков
                PlayersManager.SetNewStageTanksParameters();
                CompsManager.SetNewStageTanksParameters();

                foreach(var obstacle in Obstacles)
                {
                    foreach(var playerTank in PlayersManager.Tanks)
                        playerTank.CheckPosition += obstacle.GetCheckPositionListener();

                    foreach(var compTank in CompsManager.Tanks)
                        if(!(obstacle is Ice))
                            compTank.CheckPosition += obstacle.GetCheckPositionListener();
                }
            }
        }

        private GameForm _gameForm;
        public List<Obstacle> Obstacles { get; set; }
        public PlayersManager PlayersManager { get; private set; }
        public CompsManager CompsManager { get; private set; }
        public BonusManager BonusManager { get; private set; }
        public List<Shell> Shells { get; set; }

        public event EventHandler GameOver;
        public event EventHandler NextStage;

        private Timer _showelDelayTimer;
        private Timer _watchDelayTimer;

        public Field(GUIForm guiForm, Rectangle rect, GameForm gameForm) : base(guiForm, rect)
        {
            _gameForm = gameForm;
            Obstacles = new List<Obstacle>();
            PlayersManager = new PlayersManager(guiForm, this);
            CompsManager = new CompsManager(guiForm, this);
            BonusManager = new BonusManager(guiForm, this);
            Shells = new List<Shell>();

            _showelDelayTimer = new Timer();
            _showelDelayTimer.Interval = 20 * 1000;
            _showelDelayTimer.Tick += OnShowelDelayTimerTick;

            _watchDelayTimer = new Timer();
            _watchDelayTimer.Interval = 10 * 1000;
            _watchDelayTimer.Tick += OnWatchDelayTimerTick;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            SubscribeObstacles(Obstacles);

            PlayersManager.Subscribe();
            PlayersManager.TankShot += OnShot;
            PlayersManager.TanksDestroyed += OnPlayerTanksOrHQDestroyed;

            CompsManager.Subscribe();
            CompsManager.TankShot += OnShot;
            CompsManager.TanksDestroyed += OnCompTanksDestroyed;

            BonusManager.Subscribe();
            BonusManager.PlayerTookBomb += OnPlayerTookBomb;
            BonusManager.CompTookBomb += OnCompTookBomb;
            BonusManager.PlayerTookWatch += OnPlayerTookWatch;
            BonusManager.CompTookWatch += OnCompTookWatch;
            BonusManager.PlayerTookShowel += OnPlayerTookShowel;
            BonusManager.CompTookShowel += OnCompTookShowel;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            UnsubscribeObstacles(Obstacles);

            PlayersManager.Unsubscribe();
            PlayersManager.TankShot -= OnShot;
            PlayersManager.TanksDestroyed -= OnPlayerTanksOrHQDestroyed;

            CompsManager.Unsubscribe();
            CompsManager.TankShot -= OnShot;
            CompsManager.TanksDestroyed -= OnCompTanksDestroyed;

            BonusManager.Unsubscribe();
            BonusManager.PlayerTookBomb -= OnPlayerTookBomb;
            BonusManager.CompTookBomb -= OnCompTookBomb;
            BonusManager.PlayerTookWatch -= OnPlayerTookWatch;
            BonusManager.CompTookWatch -= OnCompTookWatch;
            BonusManager.PlayerTookShowel -= OnPlayerTookShowel;
            BonusManager.CompTookShowel -= OnCompTookShowel;
        }

        private void SubscribeObstacles(List<Obstacle> obstacles)
        {
            foreach(var obstacle in obstacles)
            {
                obstacle.Subscribe();
                if(obstacle is HQ)
                    obstacle.Destroyed += OnPlayerTanksOrHQDestroyed;
                else
                    obstacle.Destroyed += OnObstacleDestroyed;
            }
        }

        private void UnsubscribeObstacles(List<Obstacle> obstacles)
        {
            foreach(var obstacle in obstacles)
            {
                obstacle.Unsubscribe();
                obstacle.RemoveDestroyedListeners();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            //Рисование флажка
            if(new Rectangle(910, 420, 64, 60).IntersectsWith(e.ClipRectangle))
                g.DrawImageUnscaled(Properties.Resources.Flag, 910, 420);

            //Рисование номера уровня
            if(new Rectangle(930, 485, 50, 200).IntersectsWith(e.ClipRectangle))
                g.DrawString(Stage.ToString(), MyFont.GetFont(22), Brushes.Black, 930, 485);    
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(!Rect.Contains(e.NewRect))
                if(sender is Tank)
                    TankCollision(sender as Tank);
                else if(sender is Shell)
                    ShellCollision(sender as Shell);
        }

        private void OnObstacleDestroyed(object sender, EventArgs e)
        {
            var destroyedObstacle = sender as Obstacle;

            foreach(var playerTank in PlayersManager.Tanks)
                playerTank.CheckPosition -= destroyedObstacle.GetCheckPositionListener();

            foreach(var compTank in CompsManager.Tanks)
                compTank.CheckPosition -= destroyedObstacle.GetCheckPositionListener();

            foreach(var shell in Shells)
                shell.CheckPosition -= destroyedObstacle.GetCheckPositionListener();
            destroyedObstacle.RemoveDestroyedListeners();
            destroyedObstacle.Unsubscribe();
            Obstacles.Remove(destroyedObstacle);
        }

        private void OnShot(object sender, ShellEventArgs e)
        {
            var shell = e.Shell;
            shell.Destroyed += OnShellDestroyed;
            shell.CheckPosition += OnCheckPosition;
            foreach(var obstacle in Obstacles)
                if(!(obstacle is Water) && !(obstacle is Ice))
                    shell.CheckPosition += obstacle.GetCheckPositionListener();
            foreach(var listShell in Shells)
                listShell.CheckPosition += shell.GetCheckPositionListener();

            foreach(var playerTank in PlayersManager.Tanks)
                shell.CheckPosition += playerTank.GetCheckPositionListener();

            foreach(var compTankOnField in CompsManager.TanksOnField)
                shell.CheckPosition += compTankOnField.GetCheckPositionListener();

            Shells.Add(shell);
        }

        private void OnShellDestroyed(object sender, EventArgs e)
        {
            var shell = sender as Shell;
            shell.RemoveDestroyedListeners();
            shell.RemoveCheckPositionListeners();
            foreach(var listShell in Shells)
                listShell.CheckPosition -= shell.GetCheckPositionListener();
            Shells.Remove(shell);
        }

        private void OnCompTanksDestroyed(object sender, EventArgs e)
        {
            NextStage.Invoke(this, new EventArgs());
        }

        private void OnPlayerTanksOrHQDestroyed(object sender, EventArgs e)
        {
            int points1 = PlayersManager.P1Tank.Points;
            int points2 = PlayersManager.P2Tank.Points;
            if(points1 > 0)
                _gameForm.FormsManager.Records.AddRecord(Properties.Settings.Default.P1Name, PlayersManager.P1Tank.Points);
            if(points2 > 0)
                _gameForm.FormsManager.Records.AddRecord(Properties.Settings.Default.P2Name, PlayersManager.P2Tank.Points);

            //Очистка уровня
            foreach(var obstacle in Obstacles)
            {
                foreach(var playerTank in PlayersManager.Tanks)
                    playerTank.CheckPosition -= obstacle.GetCheckPositionListener();

                foreach(var compTank in CompsManager.Tanks)
                    compTank.CheckPosition -= obstacle.GetCheckPositionListener();
            }

            PlayersManager.Unsubscribe();
            CompsManager.Unsubscribe();

            Obstacles.Clear();
            _showelDelayTimer.Stop();
            _watchDelayTimer.Stop();

            GameOver?.Invoke(sender, e);
        }

        /*БОНУСЫ*/

        private void OnPlayerTookBomb(object sender, EventArgs e)
        {
            var tank = sender as Tank;
            foreach(var compTank in CompsManager.TanksOnField)
            {
                tank.Points += compTank.Points;
                compTank.HP = 0;
            }
        }

        private void OnCompTookBomb(object sender, EventArgs e)
        {
            foreach(var playerTank in PlayersManager.Tanks)
                playerTank.HP = 0;
        }

        private void OnPlayerTookShowel(object sender, EventArgs e)
        {
            DestroyHQFence();

            var concreteFence = CreateConcreteHQFence();
            SubscribeObstacles(concreteFence);

            foreach(var obstacle in concreteFence)
            {
                foreach(var playerTank in PlayersManager.Tanks)
                    playerTank.CheckPosition += obstacle.GetCheckPositionListener();

                foreach(var compTank in CompsManager.Tanks)
                    compTank.CheckPosition += obstacle.GetCheckPositionListener();
            }

            Obstacles.AddRange(concreteFence);

            _showelDelayTimer.Stop();
            _showelDelayTimer.Start();
        }

        private void OnCompTookShowel(object sender, EventArgs e)
        {
            DestroyHQFence();
        }

        private void OnShowelDelayTimerTick(object sender, EventArgs e)
        {
            _showelDelayTimer.Stop();

            DestroyHQFence();

            var brickFence = CreateBrickHQFence();
            SubscribeObstacles(brickFence);

            foreach(var obstacle in brickFence)
            {
                foreach(var playerTank in PlayersManager.Tanks)
                    playerTank.CheckPosition += obstacle.GetCheckPositionListener();

                foreach(var compTank in CompsManager.Tanks)
                    compTank.CheckPosition += obstacle.GetCheckPositionListener();
            }

            Obstacles.AddRange(brickFence);
        }

        private void OnPlayerTookWatch(object sender, EventArgs e)
        {
            _watchDelayTimer.Stop();
            _watchDelayTimer.Start();

            foreach(var compTank in CompsManager.Tanks)
                compTank.MoveTimer.Stop();

            foreach(var compTank in CompsManager.Tanks)
                compTank.MoveTimer.Stop();
        }

        private void OnCompTookWatch(object sender, EventArgs e)
        {
            _watchDelayTimer.Stop();
            _watchDelayTimer.Start();
            foreach(var playerTank in PlayersManager.Tanks)
                playerTank.MoveTimer.Stop();
        }

        private void OnWatchDelayTimerTick(object sender, EventArgs e)
        {
            _watchDelayTimer.Stop();
            
            foreach(var playerTank in PlayersManager.Tanks)
                playerTank.MoveTimer.Start();

            foreach(var compTank in CompsManager.Tanks)
                compTank.MoveTimer.Start();
        }

        /*/БОНУСЫ*/

        private void DestroyHQFence()
        {
            List<Rectangle> rects = new List<Rectangle>()
            {
                new Rectangle(416, 864, 32, 32),
                new Rectangle(416, 832, 32, 32),
                new Rectangle(416, 800, 32, 32),
                new Rectangle(448, 800, 32, 32),
                new Rectangle(480, 800, 32, 32),
                new Rectangle(512, 800, 32, 32),
                new Rectangle(512, 832, 32, 32),
                new Rectangle(512, 864, 32, 32),
            };

            var destroyObstacles = new List<Obstacle>();
            foreach(var obstacle in Obstacles)
                foreach(var rect in rects)
                    if(obstacle.Rect.IntersectsWith(rect))
                        destroyObstacles.Add(obstacle);

            foreach(Obstacle obstacle in destroyObstacles)
                obstacle.InvokeDestroyed();
        }

        private List<Obstacle> CreateConcreteHQFence()
        {
            return new List<Obstacle>()
            {
                new Concrete(GUIForm, new Rectangle(416, 864, 32, 32)),
                new Concrete(GUIForm, new Rectangle(416, 832, 32, 32)),
                new Concrete(GUIForm, new Rectangle(416, 800, 32, 32)),
                new Concrete(GUIForm, new Rectangle(448, 800, 32, 32)),
                new Concrete(GUIForm, new Rectangle(480, 800, 32, 32)),
                new Concrete(GUIForm, new Rectangle(512, 800, 32, 32)),
                new Concrete(GUIForm, new Rectangle(512, 832, 32, 32)),
                new Concrete(GUIForm, new Rectangle(512, 864, 32, 32))
            };
        }

        private List<Obstacle> CreateBrickHQFence()
        {
            return new List<Obstacle>()
            {
                new Brick(GUIForm, new Rectangle(416, 864, 32, 32)),
                new Brick(GUIForm, new Rectangle(416, 832, 32, 32)),
                new Brick(GUIForm, new Rectangle(416, 800, 32, 32)),
                new Brick(GUIForm, new Rectangle(448, 800, 32, 32)),
                new Brick(GUIForm, new Rectangle(480, 800, 32, 32)),
                new Brick(GUIForm, new Rectangle(512, 800, 32, 32)),
                new Brick(GUIForm, new Rectangle(512, 832, 32, 32)),
                new Brick(GUIForm, new Rectangle(512, 864, 32, 32))
            };
        }

        private void LoadStage()
        {
            string s = Properties.Resources.ResourceManager.GetString("Stage_" + (Stage % 71).ToString());
            int i = 0, j = 0;
            for(int c = 0; c < s.Length; c++)
            {
                char ch = s[c];
                switch(ch)
                {
                    case 'b':
                        Obstacles.Add(new Brick(GUIForm, new Rectangle(FromMatrixPos(i, j), new Size(32, 32))));
                        break;
                    case 'c':
                        Obstacles.Add(new Concrete(GUIForm, new Rectangle(FromMatrixPos(i, j), new Size(32, 32))));
                        break;
                    case 's':
                        Obstacles.Add(new Bush(GUIForm, new Rectangle(FromMatrixPos(i, j), new Size(32, 32))));
                        break;
                    case 'w':
                        Obstacles.Add(new Water(GUIForm, new Rectangle(FromMatrixPos(i, j), new Size(32, 32))));
                        break;
                    case 'i':
                        Obstacles.Add(new Ice(GUIForm, new Rectangle(FromMatrixPos(i, j), new Size(32, 32))));
                        break;
                    default:
                        break;
                }
                j++;
                if(j > 25)
                {
                    i++;
                    j = 0;
                }
            }
            Obstacles.Add(new HQ(GUIForm, new Rectangle(FromMatrixPos(24, 12), new Size(64, 64))));
        }

        private Point FromMatrixPos(int i, int j)
        {
            return new Point(Rect.X + j * 32, Rect.Y + i * 32);
        }

        public void StartNewGame(int players)
        {
            _stage = 0;
            PlayersManager.SetNewGameTanksParameters(players);
            CompsManager.SetNewGameTanksParameters(players);
        }
    }
}