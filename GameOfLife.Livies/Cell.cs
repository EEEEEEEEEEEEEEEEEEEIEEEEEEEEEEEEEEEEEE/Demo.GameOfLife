#define USE_TRANSITION_TABLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Livies
{
    public class Cell //: Life
    {
#if USE_TRANSITION_TABLE
        private static bool?[,] _table = new bool?[2, 9] {
        //  0       1      2      3      4      5      6      7      8
            {null,  null,  null,  true,  null,  null,  null,  null,  null},  // from dead state
            {false, null,  true,  true,  false, false, false, false, false}   // from alive state
        };
#endif

        protected World CurrentWorld { get; private set; }

        internal int PosX = 0;
        internal int PosY = 0;

        private const double InitAliveProbability = 0.2D;


        //public CellStateEnum State;
        private static Random _rnd = new Random();
        public Cell(World world, int posX, int posY) //: base(world, posX, posY)
        {
            this.CurrentWorld = world;

            // setup world
            this.PosX = posY;
            this.PosY = posY;
            this.CurrentWorld.PutOn(this, posX, posY);

            this.IsAlive = (_rnd.NextDouble() < InitAliveProbability);
        }

        public bool IsAlive { get; private set; }

        protected IEnumerable<Cell> FindNeighbors()
        {
            foreach (Cell item in new Cell[] {
                this.CurrentWorld.GetCell(this.PosX -1, this.PosY-1),
                this.CurrentWorld.GetCell(this.PosX, this.PosY-1),
                this.CurrentWorld.GetCell(this.PosX+1, this.PosY-1),
                this.CurrentWorld.GetCell(this.PosX-1, this.PosY),
                this.CurrentWorld.GetCell(this.PosX+1, this.PosY),
                this.CurrentWorld.GetCell(this.PosX-1, this.PosY+1),
                this.CurrentWorld.GetCell(this.PosX, this.PosY+1),
                this.CurrentWorld.GetCell(this.PosX+1, this.PosY+1)})
            {
                if (item != null) yield return item;
            }
            yield break;
        }

        public void OnNextStateChange()
        {
            int livesCount = 0;
            foreach (Cell item in this.FindNeighbors())
            {
                if (item.IsAlive == true) livesCount++;
            }

#if USE_TRANSITION_TABLE
            bool? value = _table[this.IsAlive ? 1 : 0, livesCount];
            if (value.HasValue == true)
            {
                this.IsAlive = value.Value;
            }
#else

            if (this.IsAlive == true && livesCount <1)
            {
                //孤單死亡：如果細胞的鄰居小於一個，則該細胞在下一次狀態將死亡。
                this.IsAlive = false;
            }
            else if (this.IsAlive == true && livesCount >= 4)
            {
                //擁擠死亡：如果細胞的鄰居在四個以上，則該細胞在下一次狀態將死亡。
                this.IsAlive = false;
            }
            else if (this.IsAlive == true && (livesCount == 2 || livesCount == 3))
            {
                //穩定：如果細胞的鄰居為二個或三個，則下一次狀態為穩定存活。
                //this.IsAlive = true;
            }
            else if (this.IsAlive == false && livesCount == 3)
            {
                //復活：如果某位置原無細胞存活，而該位置的鄰居為三個，則該位置將復活一細胞。
                this.IsAlive = true;
            }
            else
            {
                // ToDo: 未定義的狀態? assert
            }
#endif
        
        }




        //public enum CellStateEnum
        //{
        //    DEATH_WITH_LONELY,
        //    DEATH_WITH_CROWDED,
        //    STABLE,
        //    REVERT
        //}
    }
}
