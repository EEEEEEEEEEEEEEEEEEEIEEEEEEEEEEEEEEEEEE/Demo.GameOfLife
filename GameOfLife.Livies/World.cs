using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace GameOfLife.Livies
{
    public class World : IDisposable
    {
        private int SizeX = 0;
        private int SizeY = 0;
        private Cell[,] _map;

        private Thread _world_thread = null;
        private WorldTaskQueue _cq = new WorldTaskQueue();

        private bool _Disposed = false;

        public World(int maxPosX, int maxPosY)
        {
            this._map = new Cell[maxPosX, maxPosY];
            this.SizeX = maxPosX;
            this.SizeY = maxPosY;


            this._world_thread = new Thread(this.Running);
            this._world_thread.Start();
        }




        public void PutOn(Cell item, int posX, int posY)
        {
            if (this._map[posX, posY] == null)
            {
                this._map[posX, posY] = item;
                item.PosX = posX;
                item.PosY = posY;
                item.CurrentWorld = this;

                //item.OnNextStateChangeEx();
                //this._cq.AddCell(item);
                this._cq.AddCell(item.GetNextWorldTask());
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void RemoveOn(Cell item, int posX, int posY)
        {
            if (this._map[posX, posY] != null && this._map[posX, posY] == item)
            {
                this._map[posX, posY] = null;
                item.Dispose();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public Cell GetCell(int posX, int posY)
        {
            if (posX >= this.SizeX) return null;
            if (posY >= this.SizeY) return null;
            if (posX < 0) return null;
            if (posY < 0) return null;

            return this._map[posX, posY];
        }

        //public void MovePosition(Life item, int moveX, int moveY)
        //{
        //}

        //public IEnumerable<Life> FindLivies(int posX, int posY)
        //{
        //    if (this._map[posX, posY] == null) yield break;
        //    return this._map[posX, posY];
        //}

        internal void AddWorldTask(WorldTask task)
        {
            this._cq.AddCell(task);
        }


        public void ShowMaps()
        {
            //Console.Title = title;
            Console.SetWindowSize(this.SizeX * 2, this.SizeY);
            Console.SetCursorPosition(0, 0);
            //Console.Clear();

            for (int y = 0; y < this.SizeY; y++)
            {
                for (int x = 0; x < this.SizeX; x++)
                {
                    Cell item = this.GetCell(x, y);
                    Console.SetCursorPosition(x * 2, y);
                    Console.Write((item == null) ? ("  ") : (item.DisplayText));

                }
            }
        }















        private void Running()
        {
            while(this._Disposed == false)
            {
                WorldTask item = _cq.GetNextCell();
                TimeSpan idle = item.ExecuteTime - DateTime.Now;
                if (idle > TimeSpan.Zero)
                {
                    // 時間還沒到，發呆一下等到時間到為止
                    Thread.Sleep(idle);
                }
                ThreadPool.QueueUserWorkItem(item.ExecuteTask);
                //ThreadPool.QueueUserWorkItem(RunCellNextStateChange, item);
            }
        }

        /*
        private void RunCellNextStateChange(object state)
        {
            Cell item = state as Cell;
            if (item.CurrentWorld == null) return;

            TimeSpan? ts = item.OnNextStateChangeEx();
            if (this._Disposed == false && ts != null) _cq.AddCell(item);
        }
        */







        public void Dispose()
        {
            if (this._Disposed == true) throw new InvalidOperationException();

            this._Disposed = true;
            this._world_thread.Join();
            this._world_thread = null;
        }

























    }
}