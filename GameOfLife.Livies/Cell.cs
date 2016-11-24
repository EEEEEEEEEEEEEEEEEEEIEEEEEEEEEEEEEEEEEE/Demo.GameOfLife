using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameOfLife.Livies
{
    public class Cell : Life //: Life
    {
        public Cell()
            : base()
        {
            this.IsAlive = //(_rnd.NextDouble() < InitAliveProbability);
                this.InProbability(InitAliveProbability);
        }

        private bool InProbability(int percentage)
        {
            return (_rnd.Next(0, 100) <= percentage);
        }

        private static Random _rnd = new Random();

        public override string DisplayText
        {
            get
            {
                if (this.IsAlive == true) return "●";
                else if (this.IsInfected == true) return "◎";
                else return "○";
            }
        }

        protected override IEnumerable<Life> FindNeighbors()
        {
            if (this.CurrentWorld == null) yield break;

            foreach (Life item in new Life[] {
                this.CurrentWorld.GetLife(this.PosX -1, this.PosY-1),
                this.CurrentWorld.GetLife(this.PosX, this.PosY-1),
                this.CurrentWorld.GetLife(this.PosX+1, this.PosY-1),
                this.CurrentWorld.GetLife(this.PosX-1, this.PosY),
                this.CurrentWorld.GetLife(this.PosX+1, this.PosY),
                this.CurrentWorld.GetLife(this.PosX-1, this.PosY+1),
                this.CurrentWorld.GetLife(this.PosX, this.PosY+1),
                this.CurrentWorld.GetLife(this.PosX+1, this.PosY+1)})
            {
                if (item != null) yield return item;
            }
            yield break;
        }
        
        
        private static bool?[,] _table = new bool?[2, 9] {
            //  0       1      2      3      4      5      6      7      8
            {null,  null,  null,  true,  null,  null,  null,  null,  null},  // from dead state
            {false, false,  true,  true,  false, false, false, false, false}   // from alive state
        };

        //private const double InitAliveProbability = 0.2D;
        private const int InitAliveProbability = 20;    // 20%


        public bool IsAlive { get; private set; }

        public bool IsInfected
        {
            get { return this.InfectedCount > 0; }
        }
        private int InfectedCount = 0;



        protected override IEnumerable<TimeSpan> WholeLife()
        {
            yield return TimeSpan.FromMilliseconds(_rnd.Next(800, 1200));

            for (int index = 0; index < int.MaxValue; index++)
            {
                int livesCount = 0;
                int infectsCount = 0;
                foreach (Cell item in this.FindNeighbors())
                {
                    if (item.IsAlive == true) livesCount++;
                    if (item.IsInfected == true) infectsCount++;
                }

                bool? value = _table[this.IsAlive ? 1 : 0, livesCount];
                if (value.HasValue == true)
                {
                    this.IsAlive = value.Value;
                }

                if (this.IsInfected == true)
                {
                    this.InfectedCount--;
                    if (this.InProbability(10) == true) this.IsAlive = false;
                }
                else
                {
                    if (this.InProbability(1 + infectsCount * 5) == true) this.InfectedCount = 3;
                }

                yield return TimeSpan.FromMilliseconds(_rnd.Next(800, 1200));
            }

            this.Dispose();
            yield break;
        }
    }
}
