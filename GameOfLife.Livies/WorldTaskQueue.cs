using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameOfLife.Livies
{
    internal class WorldTaskQueue
    {
        private object SyncRoot = new object();
        private AutoResetEvent _wait = new AutoResetEvent(false);
        //private SortedDictionary<Cell, Cell> _storage = new SortedDictionary<Cell, Cell>(new CellComparer());
        private SortedDictionary<WorldTask, Cell> _storage = new SortedDictionary<WorldTask, Cell>(new WorldTaskComparer());

        public void AddCell(WorldTask cell)
        {
            lock (this.SyncRoot)
            {
                this._storage.Add(cell, null);
                this._wait.Set();
            }
        }

        public WorldTask GetNextCell()
        {
            WorldTask item = null;
            while ((item = this.CheckNextCell()) == null)
            {
                //Thread.Sleep(0);
                this._wait.WaitOne();
            }

            lock (this.SyncRoot)
            {
                this._storage.Remove(item);
                //System.Diagnostics.Trace.WriteLine(string.Format("CELL({0},{1},{2})", item.PosX, item.PosY, item.NextWakeUpTime));
                return item;
            }
        }

        public WorldTask CheckNextCell()
        {
            lock (this.SyncRoot)
            {
                foreach (WorldTask item in this._storage.Keys)
                {
                    return item;
                }
                return null;
            }
        }

        public int Count
        {
            get
            {
                return this._storage.Count;
            }
        }
    }
}
