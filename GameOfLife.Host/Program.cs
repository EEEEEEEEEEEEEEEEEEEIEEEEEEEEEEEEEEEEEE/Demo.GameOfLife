using System;
using System.Collections.Generic;
using System.Linq;
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
            _MultiThreadGameHost(args);
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
