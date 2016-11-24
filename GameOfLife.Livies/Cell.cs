using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameOfLife.Livies
{
    public class Cell : IDisposable //: Life
    {
        private bool _Disposed = false;
        public World CurrentWorld { get; internal set; }
        public int PosX { get; internal set; }
        public int PosY { get; internal set; }
        private IEnumerator<TimeSpan> _enum = null;

        private static Random _rnd = new Random();

        public virtual string DisplayText
        {
            get
            {
                return this.IsAlive ? "●" : "○";
            }
        }

        protected virtual IEnumerable<Cell> FindNeighbors()
        {
            if (this.CurrentWorld == null) yield break;

            foreach (Cell item in new Cell[] {
                this.CurrentWorld.GetCell(this.PosX -1, this.PosY-1),
                this.CurrentWorld.GetCell(this.PosX, this.PosY-1),
                this.CurrentWorld.GetCell(this.PosX+1, this.PosY-1),
                this.CurrentWorld.GetCell(this.PosX-1, this.PosY),
                this.CurrentWorld.GetCell(this.PosX+1, this.PosY),
                this.CurrentWorld.GetCell(this.PosX-1, this.PosY+1),
                this.CurrentWorld.GetCell(this.PosX, this.PosY+1),
                this.CurrentWorld.GetCell(this.PosX+1, this.PosY+1)})
            {
                if (item != null) yield return item;
            }
            yield break;
        }

        internal WorldTask GetNextWorldTask()
        {
            if (this.CurrentWorld == null) return null;

            if (this._enum.MoveNext() == true)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Cell({0},{1}): {2}", this.PosX, this.PosY, this._enum.Current));
                return new WorldTask(
                    DateTime.Now.Add(this._enum.Current),
                    delegate()
                    {
                        WorldTask task = this.GetNextWorldTask();
                        if (task != null) this.CurrentWorld.AddWorldTask(task);
                    });
            }
            else
            {
                return null;
            }
        }
        public void Dispose()
        {
            if (this._Disposed == false)
            {
                this._Disposed = true;
                if (this.CurrentWorld != null)
                {
                    this.CurrentWorld.RemoveOn(this, this.PosX, this.PosY);
                    this.CurrentWorld = null;
                }
                this.PosX = -1;
                this.PosY = -1;
            }
        }


        
        
        
        
        
        
        private static bool?[,] _table = new bool?[2, 9] {
        //  0       1      2      3      4      5      6      7      8
            {null,  null,  null,  true,  null,  null,  null,  null,  null},  // from dead state
            {false, false,  true,  true,  false, false, false, false, false}   // from alive state
        };

        private const double InitAliveProbability = 0.2D;

        public Cell()
        {
            this._enum = this.WholeLifeEx().GetEnumerator();


            this.IsAlive = (_rnd.NextDouble() < InitAliveProbability);
        }

        public bool IsAlive { get; private set; }

        protected virtual IEnumerable<TimeSpan> WholeLifeEx()
        {
            yield return TimeSpan.FromMilliseconds(_rnd.Next(800, 1200));

            for (int index = 0; index < int.MaxValue; index++)
            {
                int livesCount = 0;
                foreach (Cell item in this.FindNeighbors())
                {
                    if (item.IsAlive == true) livesCount++;
                }

                bool? value = _table[this.IsAlive ? 1 : 0, livesCount];
                if (value.HasValue == true)
                {
                    this.IsAlive = value.Value;
                }



                if (this.IsAlive == false && _rnd.NextDouble() < 0.1D)
                {
                    break;
                }

                yield return TimeSpan.FromMilliseconds(_rnd.Next(800, 1200));
            }


            this.Dispose();
            yield break;
        }
    }
}
