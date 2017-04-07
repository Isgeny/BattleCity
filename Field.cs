using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class Field : Object
    {
        private List<Object> _obstacles;
        private int _stage;

        public event EventHandler HQDestroyed;
        public event EventHandler ObstacleDestroyed;

        public Field(GUIForm guiForm, RectangleF rect) : base(guiForm, rect)
        {
            _stage = 1;
            _obstacles = new List<Object>();
        }

        public int Stage
        {
            get { return _stage; }
        }

        public List<Object> Obstacles
        {
            get { return _obstacles; }
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            foreach(Object obst in _obstacles)
                obst?.Subscribe();

            SubscribeToObstaclesDestroyed();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            foreach(Object obst in _obstacles)
                obst?.Unsubscribe();

            UnsubscribeFromObstaclesDestroyed();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RectangleF clipRect = e.ClipRectangle;
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.Flag, 910.0f, 420.0f);
            g.DrawString(_stage.ToString(), MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(930.0f, 485.0f));
        }

        public void LoadStage(int stage)
        {
            _stage = stage;
            string s = Properties.Resources.ResourceManager.GetString("Stage_" + stage.ToString());
            int i = 0, j = 0;
            for(int c = 0; c < s.Length; c++)
            {
                char ch = s[c];
                switch(ch)
                {
                    case 'b':
                        _obstacles.Add(new Brick(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
                        break;
                    case 'c':
                        _obstacles.Add(new Concrete(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
                        break;
                    case 's':
                        _obstacles.Add(new Bush(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
                        break;
                    case 'w':
                        _obstacles.Add(new Water(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
                        break;
                    case 'i':
                        _obstacles.Add(new Ice(GUIForm, new RectangleF(FromMatrixPos(i, j), new SizeF(32.0f, 32.0f))));
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
            _obstacles.Add(new HQ(GUIForm, new RectangleF(FromMatrixPos(24, 12), new SizeF(64.0f, 64.0f))));
        }

        private PointF FromMatrixPos(int i, int j)
        {
            return new PointF(Rect.X + j * 32.0f, Rect.Y + i * 32.0f);
        }

        private void LoadNextStage()
        {
            LoadStage(++_stage);
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(!Rect.Contains(e.Rect))
            {
                ((DynamicObject)sender).StopMoving();
                if(sender is Shell)
                    ((Shell)sender).InvokeDestroyed();
            }
        }

        public override void SubscribeToCheckPosition(Object obj)
        {
            base.SubscribeToCheckPosition(obj);
            foreach(Object obst in Obstacles)
                if(obj is Tank || (obj is Shell && !(obst is Ice || obst is Water)))
                    obst?.SubscribeToCheckPosition(obj);
        }

        public override void UnsubscribeFromCheckPosition(Object obj)
        {
            base.UnsubscribeFromCheckPosition(obj);
            foreach(Object obst in Obstacles)
                obst?.UnsubscribeFromCheckPosition(obj);
        }

        public void SubscribeToObstaclesDestroyed()
        {
            foreach(Object obst in Obstacles)
                obst.Destroyed += OnObstacleDestroyed;
        }

        public void UnsubscribeFromObstaclesDestroyed()
        {
            foreach(Object obst in Obstacles)
                obst.Destroyed -= OnObstacleDestroyed;
        }

        private void OnObstacleDestroyed(object sender, EventArgs e)
        {
            _obstacles.Remove((Object)sender);
            ObstacleDestroyed.Invoke(sender, e);
        }
    }
}