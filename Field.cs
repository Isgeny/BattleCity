using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class Field : Object
    {
        private List<Object> obstacles;
        private int stage;

        public event EventHandler HQDestroyed;
        public event EventHandler ObstacleDestroyed;

        public Field(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            stage = 39;
            obstacles = new List<Object>();
        }

        public int Stage
        {
            get { return stage; }
        }

        public List<Object> Obstacles
        {
            get { return obstacles; }
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.Flag, 910.0f, 420.0f);
            g.DrawString(stage.ToString(), MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(930.0f, 485.0f));
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
                        obstacles.Add(new Brick(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
                        break;
                    case 'c':
                        obstacles.Add(new Concrete(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f)))); 
                        break;
                    case 's':
                        obstacles.Add(new Bush(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
                        break;
                    case 'w':
                        obstacles.Add(new Water(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
                        break;
                    case 'i':
                        obstacles.Add(new Ice(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
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
            obstacles.Add(new HQ(GUIForm, new RectangleF(FromMatrixPos(24, 12), new SizeF(64.0f, 64.0f))));
        }

        private PointF FromMatrixPos(int i, int j)
        {
            return new PointF(Rect.X + j*32.0f, Rect.Y + i*32.0f);
        }

        private void LoadNextStage()
        {
            LoadStage(++stage);
        }

        public override void SubscribeToPaint()
        {
            GUIForm.Paint += OnPaint;
            foreach(Object obst in obstacles)
            {
                obst?.SubscribeToPaint();
            }
        }

        public override void UnsubscribeFromPaint()
        {
            GUIForm.Paint -= OnPaint;
            foreach(Object obst in obstacles)
            {
                obst?.UnsubscribeFromPaint();
            }
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(!Rect.Contains(e.Rect))
            {
                ((DynamicObject)sender).StopMoving();
                if(sender is Shell)
                {
                    ((Shell)sender).InvokeDestroy();
                }
            }
        }

        public override void SubscribeToCheckPosition(Object obj)
        {
            obj.CheckPosition += OnCheckPosition;
            foreach(Object obst in Obstacles)
            {
                if(obj is Tank || (obj is Shell && !(obst is Ice || obst is Water)))
                {
                    obst?.SubscribeToCheckPosition(obj);
                }
            }
        }

        public override void UnsubscribeFromCheckPosition(Object obj)
        {
            obj.CheckPosition -= OnCheckPosition;
            foreach(Object obst in Obstacles)
            {
                obst?.UnsubscribeFromCheckPosition(obj);
            }
        }

        private void OnObstacleDestroy(object sender, EventArgs e)
        {
            obstacles.Remove((Object)sender);
            ObstacleDestroyed.Invoke(sender, e); 
        }

        public void SubscribeToObstaclesDestroy()
        {
            foreach(Object obst in Obstacles)
            {
                obst.Destroy += OnObstacleDestroy;
            }
        }

        public void UnsubscribeFromObstaclesDestroy()
        {
            foreach(Object obst in Obstacles)
            {
                obst.Destroy -= OnObstacleDestroy;
            }
        }

        public void CleanField()
        {
            throw new System.NotImplementedException();
        }

        public void PlaceObstacle(Object obstacle, byte i, byte j)
        {
            throw new System.NotImplementedException();
        }
    }
}