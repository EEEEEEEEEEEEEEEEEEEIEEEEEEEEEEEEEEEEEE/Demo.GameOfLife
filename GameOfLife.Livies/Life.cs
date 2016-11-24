using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Livies
{
    public abstract class Life : IDisposable
    {
        private bool _Disposed = false;
        public World CurrentWorld { get; internal set; }
        public int PosX { get; internal set; }
        public int PosY { get; internal set; }
        private IEnumerator<TimeSpan> _enum = null;

        protected Life()
        {
            this._enum = this.WholeLife().GetEnumerator();
        }


        internal WorldTask GetNextWorldTask()
        {
            if (this.CurrentWorld == null) return null;

            if (this._enum.MoveNext() == true)
            {
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


        public virtual void Dispose()
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



        /// <summary>
        /// 兩個字元的寬度 (英數)，或是一個全型的字元
        /// </summary>
        public abstract string DisplayText { get; }

        /// <summary>
        /// 傳回 LIFE 可看到範圍內的其它 LIFE。
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Life> FindNeighbors();

        /// <summary>
        /// LIFE 從出生到死亡，所有經例的過程都寫在這裡
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<TimeSpan> WholeLife();
    }
}
