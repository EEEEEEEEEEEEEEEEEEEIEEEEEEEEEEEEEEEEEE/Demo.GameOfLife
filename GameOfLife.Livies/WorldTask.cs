using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameOfLife.Livies
{
    internal class WorldTask
    {
        internal Guid ID = Guid.NewGuid();
        public DateTime ExecuteTime { get; private set; }

        private ThreadStart _ts = null;
        private ParameterizedThreadStart _pts = null;
        private object _state = null;

        public WorldTask(DateTime executeTime, ThreadStart executeMethod)
        {
            this.ExecuteTime = executeTime;
            this._ts = executeMethod;
        }

        public WorldTask(DateTime executeTime, ParameterizedThreadStart executeMethod, object state)
        {
            this.ExecuteTime = executeTime;
            this._pts = executeMethod;
            this._state = state;
        }

        public void ExecuteTask()
        {
            if (this._ts != null)
            {
                this._ts();
            }
            else
            {
                this._pts(this._state);
            }
        }
        public void ExecuteTask(object state)
        {
            this.ExecuteTask();
        }
    }
}
