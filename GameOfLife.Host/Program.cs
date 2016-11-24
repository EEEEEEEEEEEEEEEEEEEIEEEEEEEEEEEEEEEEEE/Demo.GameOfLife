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
