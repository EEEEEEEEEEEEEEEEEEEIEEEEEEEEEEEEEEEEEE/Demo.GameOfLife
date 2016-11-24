using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using GameOfLife.Livies;
using System.Threading;

namespace GameOfLife.Host
{
    class Program
    {


        static void Main(string[] args)
        {
            //_SimpleGameHost(args);
            //_MultiThreadGameHost(args);
            _YieldReturnGameHost(args);
        }


        public class CellComparer : IComparer<Cell>
        {
            #region IComparer<Cell> Members

            public int Compare(Cell x, Cell y)
            {
                int dtresult = DateTime.Compare(x.NextWakeUpTime, y.NextWakeUpTime);
                if (dtresult != 0) return dtresult;

                return x.GetHashCode() - y.GetHashCode();
            }

            #endregion
        }



        public class CellToDoList
        {
            private object SyncRoot = new object();
            private SortedDictionary<Cell, Cell> _storage = new SortedDictionary<Cell,Cell>(new CellComparer());
            public void AddCell(Cell cell)
            {
                lock (this.SyncRoot)
                {
                    this._storage.Add(cell, cell);
                }
            }

            public Cell GetNextCell()
            {
                lock (this.SyncRoot)
                {
                    Cell item = this.CheckNextCell();
                    if (item == null)
                    {
                        throw new InvalidOperationException("queue empty");
                    }
                    else
                    {
                        this._storage.Remove(item);
                        //System.Diagnostics.Trace.WriteLine(string.Format("CELL({0},{1},{2})", item.PosX, item.PosY, item.NextWakeUpTime));
                        return item;
                    }
                }
            }

            public Cell CheckNextCell()
            {
                lock (this.SyncRoot)
                {
                    foreach (Cell item in this._storage.Keys)
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


        static CellToDoList _cq;
        static void _YieldReturnGameHost(string[] args)
        {
            int worldSizeX = 30;
            int worldSizeY = 30;

            World realworld = new World(worldSizeX, worldSizeY);

            _cq = new CellToDoList();
            // init threads for each cell
            for (int positionX = 0; positionX < worldSizeX; positionX++)
            {
                for (int positionY = 0; positionY < worldSizeY; positionY++)
                {
                    Cell cell = realworld.GetCell(positionX, positionY);
                    cell.OnNextStateChangeEx();
                    _cq.AddCell(cell);
                }
            }

            // 啟動定期更新畫面的執行緒
            Thread t = new Thread(RefreshScreen);
            t.Start(realworld);

            while (_cq.Count > 0)
            {
                Cell item = _cq.GetNextCell();
                if (item.NextWakeUpTime > DateTime.Now)
                {
                    // 時間還沒到，發呆一下等到時間到為止
                    Thread.Sleep(item.NextWakeUpTime - DateTime.Now);
                }
                
                ThreadPool.QueueUserWorkItem(RunCellNextStateChange, item);
            }
        }

        private static void RunCellNextStateChange(object state)
        {
            Cell item = state as Cell;
            TimeSpan? ts = item.OnNextStateChangeEx();
            if (ts != null) _cq.AddCell(item);
        }


        private static void RefreshScreen(object state)
        {
            while (true)
            {
                Thread.Sleep(500);
                (state as World).ShowMaps("");
            }
        }

        






























        static void _MultiThreadGameHost(string[] args)
        {
            int worldSizeX = 30;
            int worldSizeY = 30;
            int maxGenerationCount = 100;

            World realworld = new World(worldSizeX, worldSizeY);

            // init threads for each cell
            List<Thread> threads = new List<Thread>();
            for (int positionX = 0; positionX < worldSizeX; positionX++)
            {
                for (int positionY = 0; positionY < worldSizeY; positionY++)
                {
                    Cell cell = realworld.GetCell(positionX, positionY);
                    Thread t = new Thread(cell.WholeLife);
                    threads.Add(t);
                    t.Start(maxGenerationCount);
                }
            }

            // reflesh maps
            do
            {
                realworld.ShowMaps("");
                Thread.Sleep(100);
            } while (IsAllThreadStopped(threads) == false);

            // wait all thread exit.
            foreach (Thread t in threads) t.Join();
        }

        private static bool IsAllThreadStopped(List<Thread> threads)
        {
            foreach (Thread t in threads)
            {
                if (t.ThreadState != ThreadState.Stopped) return false;
            }
            return true;
        }

        //private static Random rnd = new Random();
        //private static void CellThread(object cell)
        //{
        //    for(int gen = 1; gen < 10; gen++)
        //    {
        //        (cell as Cell).OnNextStateChange();
        //        Thread.Sleep(950+rnd.Next(100));
        //    }
        //}











        static void _SimpleGameHost(string[] args)
        {
            int worldSizeX = 30;
            int worldSizeY = 30;
            int maxGenerationCount = 100;

            World realworld = new World(worldSizeX, worldSizeY);
            for (int generation = 1; generation <= maxGenerationCount; generation++)
            {
                realworld.ShowMaps(string.Format("Generation: {0}", generation));
                Thread.Sleep(1000);

                for (int positionX = 0; positionX < worldSizeX; positionX++)
                {
                    for (int positionY = 0; positionY < worldSizeY; positionY++)
                    {
                        // do day pass
                        Cell cell = realworld.GetCell(positionX, positionY) as Cell;
                        cell.OnNextStateChange();
                    }
                }
            }
        }
    }
}
