using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class Field : GraphicsObject
    {
        private BonusManager _bonusManager;
        private GameForm _gameForm;
        private Timer _showelDelayTimer;
        private int _stage;

        public List<GraphicsObject> Obstacles { get; private set; }

        public int Stage
        {
            get { return _stage; }
            private set
            {
                _stage = value % 71;
            }
        }

        public event EventHandler HQDestroyed;

        public Field(GUIForm guiForm, Rectangle rect, GameForm gameForm, BonusManager bonusManager) : base(guiForm, rect)
        {
            _bonusManager = bonusManager;
            _gameForm = gameForm;
            _showelDelayTimer = new Timer();
            _showelDelayTimer.Interval = 20 * 1000;
            _showelDelayTimer.Tick += OnShowelDelayTimerTick;

            Obstacles = new List<GraphicsObject>();
            Stage = 1;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            SubscribeObstacles(Obstacles);
            SubscribeToPlayerTanks(Obstacles);
            SubscribeToCompTanks(Obstacles);
            _gameForm.PlayerManager.TanksDestroyed  += OnPlayerTanksDestroyed;
            _gameForm.CompsManager.TanksDestroyed   += OnCompTanksDestroyed;
            _bonusManager.PlayerTookShowel          += OnPlayerTookShowel;
            _bonusManager.CompTookShowel            += OnCompTookShowel;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            UnsubscribeObstacles(Obstacles);
            UnsubscribeFromPlayerTanks(Obstacles);
            UnsubscribeFromCompTanks(Obstacles);
            _gameForm.PlayerManager.TanksDestroyed  -= OnPlayerTanksDestroyed;
            _gameForm.CompsManager.TanksDestroyed   -= OnCompTanksDestroyed;
            _bonusManager.PlayerTookShowel          -= OnPlayerTookShowel;
            _bonusManager.CompTookShowel            -= OnCompTookShowel;
        }

        private void SubscribeObstacles(List<GraphicsObject> obstacles)
        {
            foreach(GraphicsObject obstacle in obstacles)
            {
                obstacle.Subscribe();
                obstacle.Destroyed += OnObstacleDestroyed;
            }
        }

        private void UnsubscribeObstacles(List<GraphicsObject> obstacles)
        {
            foreach(GraphicsObject obstacle in obstacles)
            {
                obstacle.Unsubscribe();
                obstacle.Destroyed -= OnObstacleDestroyed;
            }
        }

        private void SubscribeToPlayerTanks(List<GraphicsObject> obstacles)
        {
            foreach(Tank playerTank in _gameForm.PlayerManager.Tanks)
            {
                playerTank.CheckPosition            += OnCheckPosition;
                playerTank.Destroyed                += OnPlayerTankDestroyed;
                foreach(GraphicsObject obstacle in obstacles)
                    playerTank.CheckPosition        += obstacle.GetCheckPositionListener();
            }
        }

        private void UnsubscribeFromPlayerTanks(List<GraphicsObject> obstacles)
        {
            foreach(Tank playerTank in _gameForm.PlayerManager.Tanks)
            {
                playerTank.CheckPosition            -= OnCheckPosition;
                playerTank.Destroyed                -= OnPlayerTankDestroyed;
                foreach(GraphicsObject obstacle in obstacles)
                    playerTank.CheckPosition        -= obstacle.GetCheckPositionListener();
            }
        }

        private void SubscribeToCompTanks(List<GraphicsObject> obstacles)
        {
            foreach(Tank compTank in _gameForm.CompsManager.Tanks)
            {
                compTank.CheckPosition              += OnCheckPosition;
                compTank.Destroyed                  += OnCompTankDestroyed;
                foreach(GraphicsObject obstacle in obstacles)
                    if(!(obstacle is Ice))
                        compTank.CheckPosition      += obstacle.GetCheckPositionListener();
            }
        }

        private void UnsubscribeFromCompTanks(List<GraphicsObject> obstacles)
        {
            foreach(Tank compTank in _gameForm.CompsManager.Tanks)
            {
                compTank.CheckPosition              -= OnCheckPosition;
                compTank.Destroyed                  -= OnCompTankDestroyed;
                foreach(GraphicsObject obstacle in obstacles)
                    if(!(obstacle is Ice))
                        compTank.CheckPosition      -= obstacle.GetCheckPositionListener();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Rectangle clipRect = e.ClipRectangle;
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.Flag, 910, 420);
            g.DrawString(Stage.ToString(), MyFont.GetFont(22), new SolidBrush(Color.Black), new Point(930, 485));
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(!Rect.Contains(e.Rect))
                if(sender is Tank)
                    ((Tank)sender).StopMoving();
                else if(sender is Shell)
                    ((Shell)sender).InvokeDestroyed();
        }

        private void OnObstacleDestroyed(object sender, EventArgs e)
        {
            GraphicsObject destroyedObstacle = sender as GraphicsObject;
            foreach(Tank tank in _gameForm.PlayerManager.Tanks)
                tank.CheckPosition -= destroyedObstacle.GetCheckPositionListener();
            foreach(Tank tank in _gameForm.CompsManager.Tanks)
                tank.CheckPosition -= destroyedObstacle.GetCheckPositionListener();
            foreach(Shell shell in _gameForm.Shells)
                shell.CheckPosition -= destroyedObstacle.GetCheckPositionListener();
            Obstacles.Remove(destroyedObstacle);
            if(sender is HQ)
            {
                HQDestroyed?.Invoke(sender, e);
                return;
            }
            destroyedObstacle.Unsubscribe();   
        }

        private void OnPlayerTankDestroyed(object sender, EventArgs e)
        {
            Tank playerTank = sender as Tank;
            playerTank.CheckPosition        -= OnCheckPosition;
            playerTank.Destroyed            -= OnPlayerTankDestroyed;
            foreach(GraphicsObject obstacle in Obstacles)
                playerTank.CheckPosition    -= obstacle.GetCheckPositionListener();
        }

        private void OnCompTankDestroyed(object sender, EventArgs e)
        {
            Tank compTank = sender as Tank;
            compTank.CheckPosition          -= OnCheckPosition;
            compTank.Destroyed              -= OnCompTankDestroyed;
            foreach(GraphicsObject obstacle in Obstacles)
                if(!(obstacle is Ice))
                    compTank.CheckPosition  -= obstacle.GetCheckPositionListener();
        }

        private void OnPlayerTanksDestroyed(object sender, EventArgs e)
        {
            Stage = 1;
        }        

        private void OnCompTanksDestroyed(object sender, EventArgs e)
        {
            UnsubscribeObstacles(Obstacles);
            UnsubscribeFromPlayerTanks(Obstacles);
            UnsubscribeFromCompTanks(Obstacles);

            Stage++;
            LoadStage();

            SubscribeObstacles(Obstacles);
            SubscribeToPlayerTanks(Obstacles);
            SubscribeToCompTanks(Obstacles);
        }

        private void OnPlayerTookShowel(object sender, EventArgs e)
        {
            DestroyHQFence();

            List<GraphicsObject> ConcreteFence = CreateConcreteHQFence();
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
            
            List<GraphicsObject> BrickFence = CreateBrickHQFence();
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

            List<GraphicsObject> destroyObstacles = new List<GraphicsObject>();
            foreach(GraphicsObject obstacle in Obstacles)
                foreach(Rectangle rect in rects)
                    if(obstacle.Rect.IntersectsWith(rect))
                        destroyObstacles.Add(obstacle);

            foreach(GraphicsObject obstacle in destroyObstacles)
                obstacle.InvokeDestroyed();
        }

        private List<GraphicsObject> CreateConcreteHQFence()
        {
            return new List<GraphicsObject>()
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

        private List<GraphicsObject> CreateBrickHQFence()
        {
            return new List<GraphicsObject>()
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
            GUIForm.Invalidate(new Rectangle(0, 0, 1024, 960));
        }

        /// <summary>
        /// Перевод позиции матрицы в декартовы координаты
        /// </summary>
        /// <param name="i">позиция строки</param>
        /// <param name="j">позиция столбца</param>
        /// <returns></returns>
        private Point FromMatrixPos(int i, int j)
        {
            return new Point(Rect.X + j * 32, Rect.Y + i * 32);
        }

        /*public EventHandler GetCompsTanksDestroyedListener()
        {
            return OnCompTanksDestroyed;
        }*/

        public void InitializeField()
        {
            LoadStage();
        }
    }
}