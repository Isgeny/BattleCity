using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    public delegate void RectEventHandler(System.Drawing.RectangleF rect);

    public class RectEventArgs
    {
        private RectangleF rect;

        public RectangleF Rect
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}