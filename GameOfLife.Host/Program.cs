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
            int worldSizeX = 30;
            int worldSizeY = 30;

            World realworld = new World(worldSizeX, worldSizeY);

            Random _rnd = new Random();
            for (int x = 0; x < worldSizeX; x++)
            {
                for (int y = 0; y < worldSizeY; y++)
                {
                    if (_rnd.Next(100) < 5) continue;
                    Cell item = new Cell();
                    realworld.PutOn(item, x, y);
                    //break;
                }
                //break;
            }














            Thread t2 = new Thread(RefreshScreen);
            t2.Start(realworld);
            Thread.Sleep(1000 * 10);
            //realworld.RemoveOn(realworld.GetCell(5, 5), 5, 5);
            Thread.Sleep(1000 * 100);
            realworld.Dispose();
            realworld = null;
            
            
            IsExit = true;
            t2.Join();
        }

        private static bool IsExit = false;

        private static void RefreshScreen(object world)
        {
            while (IsExit == false)
            {
                Thread.Sleep(500);
                (world as World).ShowMaps();
            }
        }





    }
}
