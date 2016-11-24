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
        //private SortedDictionary<Life, Life> _storage = new SortedDictionary<Life, Life>(new LifeComparer());
        private SortedDictionary<WorldTask, Life> _storage = new SortedDictionary<WorldTask, Life>(new WorldTaskComparer());

        public void AddLife(WorldTask Life)
        {
            lock (this.SyncRoot)
            {
                this._storage.Add(Life, null);
                this._wait.Set();
            }
        }

        public WorldTask GetNextLife()
        {
            WorldTask item = null;
            while ((item = this.CheckNextLife()) == null)
            {
                //Thread.Sleep(0);
                this._wait.WaitOne();
            }

            lock (this.SyncRoot)
            {
                this._storage.Remove(item);
                //System.Diagnostics.Trace.WriteLine(string.Format("Life({0},{1},{2})", item.PosX, item.PosY, item.NextWakeUpTime));
                return item;
            }
        }

        public WorldTask CheckNextLife()
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
