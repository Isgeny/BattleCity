using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class Field : Object
    {
        private BattleCity.Obstacle[][] obstacles;
        private int stage;
        private const int SZ = 26;

        public event EventHandler HQDestroyed;

        public Field(GUIForm guiForm, RectangleF rect)
        {
            throw new System.NotImplementedException();
        }

        public int Stage
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Obstacle[][] Obstacles
        {
            get => default(Obstacle[][]);
        }

        public void loadStage(int stage)
        {
            throw new System.NotImplementedException();
        }

        public void loadNextStage()
        {
            throw new System.NotImplementedException();
        }

        private List<Obstacle> CollideableObstacles(System.Collections.Generic.List<System.Drawing.PointF> rectCorners)
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToForm()
        {
            throw new System.NotImplementedException();
        }

        public override void SubscribeToObjectPosition(Object obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnCheckPosition(object sender, PositionEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private List<PointF> RectCorners(Direction direction)
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

        public override void UnsubscribeFromForm()
        {
            throw new System.NotImplementedException();
        }
    }
}