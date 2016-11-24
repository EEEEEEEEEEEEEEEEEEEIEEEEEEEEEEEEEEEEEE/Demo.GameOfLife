using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Livies
{
    internal class WorldTaskComparer : IComparer<WorldTask>
    {
        public int Compare(WorldTask x, WorldTask y)
        {
            int dtresult = DateTime.Compare(x.ExecuteTime, y.ExecuteTime);
            if (dtresult != 0) return dtresult;

            return x.ID.CompareTo(y.ID);
        }
    }
}
