using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using GameOfLife.Livies;
using System.Threading;
using System.Xml;
using System.IO;

namespace GameOfLife.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            int worldSizeX = 40;
            int worldSizeY = 40;

            World realworld = new World(worldSizeX, worldSizeY);

            Random _rnd = new Random();
            for (int x = 0; x < worldSizeX; x++)
            {
                for (int y = 0; y < worldSizeY; y++)
                {
                    //Cell item = new Cell();
                    Life item = null;

                    if (_rnd.Next(100) < 3) item = new Tiger();
                    else if (_rnd.Next(100) < 8) item = new Sheep();
                    else item = new Grass();

                    realworld.PutOn(item, x, y);
                }
            }


            Thread t2 = new Thread(RefreshScreen);
            t2.Start(realworld);
            Thread.Sleep(1000 * 10);
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
