using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Livies
{
    public abstract class Life : IDisposable
    {
        protected bool IsDisposed = false;
        protected Guid ID { get; private set; }

        public DateTime Birthday { get; protected set; }

        protected abstract IEnumerable<Life> FindNeighbors();

        protected World CurrentWorld { get; private set; }

        internal int PosX = 0;
        internal int PosY = 0;


        protected Life(World world, int posX, int posY)
        {
            this.ID = Guid.NewGuid();
            this.Birthday = DateTime.Now;
            this.CurrentWorld = world;
            
            // setup world
            this.PosX = posY;
            this.PosY = posY;
            this.CurrentWorld.PutOn(this, posX, posY);
        }

        public abstract void OnNextStateChange();

        #region IDisposable Members

        public void Dispose()
        {
            this.IsDisposed = true;
            this.CurrentWorld.RemoveOn(this, this.PosX, this.PosY);
            this.PosX = int.MinValue;
            this.PosY = int.MinValue;
        }

        #endregion
    }


}
