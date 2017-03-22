using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class Field : Object
    {
        private Obstacle[,] obstacles;
        private int stage;
        private const int SZ = 26;

        public event EventHandler HQDestroyed;

        public Field(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            stage = 39;
            obstacles = new Obstacle[SZ, SZ];
        }

        public int Stage
        {
            get { return stage; }
        }

        public Obstacle[,] Obstacles
        {
            get { return obstacles; }
        }

        public void LoadStage(int stage)
        {
            string s = Properties.Resources.ResourceManager.GetString("Stage_" + stage.ToString());
            int i = 0, j = 0;
            for(int c = 0; c < s.Length; c++)
            {
                char ch = s[c];
                switch(ch)
                {
                    case 'b':
                        obstacles[i, j] = new Brick(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f)));
                        break;
                    case 'c':
                        obstacles[i, j] = new Concrete(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))); 
                        break;
                    case 's':
                        obstacles[i, j] = new Bush(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f)));
                        break;
                    case 'w':
                        obstacles[i, j] = new Water(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f)));
                        break;
                    case 'i':
                        obstacles[i, j] = new Ice(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f)));
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

            Obstacle hq = new HQ(GUIForm, new RectangleF(FromMatrixPos(24, 12), new SizeF(64.0f, 64.0f)));
            obstacles[24, 12] = hq;
            obstacles[24, 13] = hq;
            obstacles[25, 12] = hq;
            obstacles[25, 13] = hq;
        }

        private PointF FromMatrixPos(int i, int j)
        {
            return new PointF(Rect.X + j*32.0f, Rect.Y + i*32.0f);
        }

        public void LoadNextStage()
        {
            throw new System.NotImplementedException();
        }

        private List<Obstacle> CollideableObstacles(System.Collections.Generic.List<System.Drawing.PointF> rectCorners)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            foreach(Obstacle obst in obstacles)
            {
                obst?.SubscribeToForm();
            }
        }

        public override void UnsubscribeFromForm()
        {
            foreach(Obstacle obst in obstacles)
            {
                obst.UnsubscribeFromForm();
            }
        }

        public override void SubscribeToObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnCheckPosition(object sender, RectEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private List<PointF> RectCorners()
        {
            throw new System.NotImplementedException();
        }

        public void CleanField()
        {
            throw new System.NotImplementedException();
        }

        public void PlaceObstacle(Obstacle obstacle, byte i, byte j)
        {
            throw new System.NotImplementedException();
        }

        public override void UnsubscribeFromObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}