using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GameOfLife.Livies
{
    public class World
    {
        private int SizeX = 0;
        private int SizeY = 0;
        private Cell[,] _map;
        public World(int maxPosX, int maxPosY)
        {
            this._map = new Cell[maxPosX, maxPosY];
            this.SizeX = maxPosX;
            this.SizeY = maxPosY;

            for (int posX = 0; posX < maxPosX; posX++)
            {
                for (int posY = 0; posY < maxPosY; posY++)
                {
                    this._map[posX, posY] = new Cell(this, posX, posY);
                }
            }
        }

        internal void PutOn(Cell item, int posX, int posY)
        {
            if (this._map[posX, posY] == null)
            {
                this._map[posX, posY] = item;
                item.PosX = posX;
                item.PosY = posY;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        //internal void RemoveOn(Cell item, int posX, int posY)
        //{
        //    if (this._map[posX, posY] != null && this._map[posX, posY] == item)
        //    {
        //        this._map[posX, posY] = null;
        //        item.PosX = int.MinValue;
        //        item.PosY = int.MinValue;
        //    }
        //    else
        //    {
        //        throw new ArgumentException();
        //    }
        //}

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




        public void ShowMaps(string title)
        {
            Console.Title = title;
            Console.SetWindowSize(this.SizeX * 2, this.SizeY);
            Console.SetCursorPosition(0, 0);
            Console.Clear();

            for (int y = 0; y < this.SizeY; y++)
            {
                for (int x = 0; x < this.SizeX; x++)
                {
                    Cell item = this.GetCell(x, y);
                    Console.SetCursorPosition(x * 2, y);
                    Console.Write(item.IsAlive? "●":"○");
                }
            }
        }
    }


    public class Position
    {
    }

    public class Vector
    {
    }
}
