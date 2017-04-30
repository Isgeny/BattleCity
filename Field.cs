using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class Field : GraphicsObject
    {
        private int _stage;
        private Timer _showelDelayTimer;

        public List<Obstacle> Obstacles { get; set; }
        public PlayersManager PlayersManager { get; private set; }
        public CompsManager CompsManager { get; private set; }
        public BonusManager BonusManager { get; private set; }
        public List<Shell> Shells { get; private set; }
        
        public event EventHandler GameOver;
        public event EventHandler NextStage;

        public Field(GUIForm guiForm, Rectangle rect, GameForm gameForm) : base(guiForm, rect)
        {
            Obstacles = new List<Obstacle>();
            Stage = 1;
            
            _showelDelayTimer = new Timer();
            _showelDelayTimer.Interval = 20 * 1000;
            _showelDelayTimer.Tick += OnShowelDelayTimerTick;

            BonusManager = new BonusManager(guiForm, this);
            PlayersManager = new PlayersManager(guiForm, this);
            CompsManager = new CompsManager(guiForm, this);
            Shells = new List<Shell>();
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            SubscribeObstacles(Obstacles);
            SubscribeToPlayerTanks(Obstacles);
            SubscribeToCompTanks(Obstacles);

            PlayersManager.Subscribe();
            PlayersManager.TankShoot       += OnShoot;
            PlayersManager.TanksDestroyed  += OnPlayerTanksOrHQDestroyed;

            CompsManager.Subscribe();
            CompsManager.TankShoot         += OnShoot;
            CompsManager.TanksDestroyed    += OnCompTanksDestroyed;

            BonusManager.PlayerTookShowel  += OnPlayerTookShowel;
            BonusManager.CompTookShowel    += OnCompTookShowel; 
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            UnsubscribeObstacles(Obstacles);
            UnsubscribeFromPlayerTanks(Obstacles);
            UnsubscribeFromCompTanks(Obstacles);

            PlayersManager.Unsubscribe();
            PlayersManager.TankShoot       -= OnShoot;
            PlayersManager.TanksDestroyed  -= OnPlayerTanksOrHQDestroyed;

            CompsManager.Unsubscribe();
            CompsManager.TankShoot         -= OnShoot;
            CompsManager.TanksDestroyed    -= OnCompTanksDestroyed;

            BonusManager.PlayerTookShowel  -= OnPlayerTookShowel;
            BonusManager.CompTookShowel    -= OnCompTookShowel;
        }

        public int Stage
        {
            get { return _stage; }
            private set
            {
                _stage = value % 71;
                LoadStage();
            }
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
                if(obstacle is HQ)
                    obstacle.Destroyed -= OnPlayerTanksOrHQDestroyed;
                else
                    obstacle.Destroyed -= OnObstacleDestroyed;
            }
        }

        private void SubscribeToPlayerTanks(List<Obstacle> obstacles)
        {
            foreach(var playerTank in PlayersManager.Tanks)
            {
                playerTank.CheckPosition            += OnCheckPosition;
                playerTank.Destroyed                += OnPlayerTankDestroyed;
                foreach(var obstacle in obstacles)
                    playerTank.CheckPosition        += obstacle.GetCheckPositionListener();
            }
        }

        private void UnsubscribeFromPlayerTanks(List<Obstacle> obstacles)
        {
            foreach(var playerTank in PlayersManager.Tanks)
            {
                playerTank.CheckPosition            -= OnCheckPosition;
                playerTank.Destroyed                -= OnPlayerTankDestroyed;
                foreach(var obstacle in obstacles)
                    playerTank.CheckPosition        -= obstacle.GetCheckPositionListener();
            }
        }

        private void SubscribeToCompTanks(List<Obstacle> obstacles)
        {
            foreach(var compTank in CompsManager.Tanks)
            {
                compTank.CheckPosition              += OnCheckPosition;
                compTank.Destroyed                  += OnCompTankDestroyed;
                foreach(var obstacle in obstacles)
                    if(!(obstacle is Ice))
                        compTank.CheckPosition      += obstacle.GetCheckPositionListener();
            }
        }

        private void UnsubscribeFromCompTanks(List<Obstacle> obstacles)
        {
            foreach(var compTank in CompsManager.Tanks)
            {
                compTank.CheckPosition              -= OnCheckPosition;
                compTank.Destroyed                  -= OnCompTankDestroyed;
                foreach(var obstacle in obstacles)
                    if(!(obstacle is Ice))
                        compTank.CheckPosition      -= obstacle.GetCheckPositionListener();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawImageUnscaled(Properties.Resources.Flag, 910, 420);
            g.DrawString(Stage.ToString(), MyFont.GetFont(22), Brushes.Black, 930, 485);
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(!Rect.Contains(e.Rect))
                if(sender is Tank)
                    TankCollision(sender as Tank);
                else if(sender is Shell)
                    ShellCollision(sender as Shell);
        }

        private void OnObstacleDestroyed(object sender, EventArgs e)
        {
            var destroyedObstacle = sender as Obstacle;
            foreach(Tank tank in PlayersManager.Tanks)
                tank.CheckPosition -= destroyedObstacle.GetCheckPositionListener();
            foreach(Tank tank in CompsManager.Tanks)
                tank.CheckPosition -= destroyedObstacle.GetCheckPositionListener();
            foreach(Shell shell in Shells)
                shell.CheckPosition -= destroyedObstacle.GetCheckPositionListener();
            Obstacles.Remove(destroyedObstacle);
            destroyedObstacle.Unsubscribe();   
        }

        private void OnPlayerTankDestroyed(object sender, EventArgs e)
        {
            var playerTank = sender as Tank;
            playerTank.CheckPosition        -= OnCheckPosition;
            playerTank.Destroyed            -= OnPlayerTankDestroyed;
            foreach(var obstacle in Obstacles)
                playerTank.CheckPosition    -= obstacle.GetCheckPositionListener();
        }

        private void OnCompTankDestroyed(object sender, EventArgs e)
        {
            var compTank = sender as Tank;
            compTank.CheckPosition          -= OnCheckPosition;
            compTank.Destroyed              -= OnCompTankDestroyed;
            foreach(var obstacle in Obstacles)
                if(!(obstacle is Ice))
                    compTank.CheckPosition  -= obstacle.GetCheckPositionListener();
        }

        private void OnCompTanksDestroyed(object sender, EventArgs e)
        {
            UnsubscribeObstacles(Obstacles);
            UnsubscribeFromPlayerTanks(Obstacles);
            UnsubscribeFromCompTanks(Obstacles);

            Stage++;

            NextStage.Invoke(this, new EventArgs());
        }

        private void OnPlayerTanksOrHQDestroyed(object sender, EventArgs e)
        {
            UnsubscribeObstacles(Obstacles);
            UnsubscribeFromPlayerTanks(Obstacles);
            UnsubscribeFromCompTanks(Obstacles);
            Stage = 1;

            GameOver?.Invoke(sender, e);
        }

        private void OnPlayerTookShowel(object sender, EventArgs e)
        {
            DestroyHQFence();

            List<Obstacle> ConcreteFence = CreateConcreteHQFence();
            SubscribeObstacles(ConcreteFence);
            SubscribeToPlayerTanks(ConcreteFence);
            SubscribeToCompTanks(ConcreteFence);
            Obstacles.AddRange(ConcreteFence);

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
            
            List<Obstacle> BrickFence = CreateBrickHQFence();
            SubscribeObstacles(BrickFence);
            SubscribeToPlayerTanks(BrickFence);
            SubscribeToCompTanks(BrickFence);
            Obstacles.AddRange(BrickFence);
        }

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
            Obstacles.Clear();
            string s = Properties.Resources.ResourceManager.GetString("Stage_" + Stage.ToString());
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

        private void OnShoot(object sender, ShellEventArgs e)
        {
            var s = e.Shell;
            s.Subscribe();
            s.Destroyed     += OnShellDestroyed;
            s.CheckPosition += OnCheckPosition;
            foreach(var obstacle in Obstacles)
                if(!(obstacle is Water) && !(obstacle is Ice))
                    s.CheckPosition += obstacle.GetCheckPositionListener();
            foreach(var shell in Shells)
                shell.CheckPosition += s.GetCheckPositionListener();
            Shells.Add(s);
        }

        private void OnShellDestroyed(object sender, EventArgs e)
        {
            var s = sender as Shell;
            s.Destroyed     -= OnShellDestroyed;
            s.CheckPosition -= OnCheckPosition;
            foreach(var obstacle in Obstacles)
                if(!(obstacle is Water) && !(obstacle is Ice))
                    s.CheckPosition -= obstacle.GetCheckPositionListener();
            foreach(var shell in Shells)
                shell.CheckPosition -= s.GetCheckPositionListener();
            Shells.Remove(s);
        }

        public void SetPlayers(int players)
        {
            PlayersManager.SetPlayers(players);
            PlayersManager.InitializeTanks();
            CompsManager.InitializeTanks();
        }
    }
}