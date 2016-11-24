using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Livies
{
    public class Grass : Life
    {
        public override string DisplayText
        {
            get { return "※"; }
        }

        protected override IEnumerable<Life> FindNeighbors()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<TimeSpan> WholeLife()
        {
            while (true) yield return TimeSpan.FromSeconds(5);
            yield break;
        }
    }
}
