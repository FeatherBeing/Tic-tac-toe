using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Player
    {
        #region Fields & Properties

        private static int ctr;
        public Mark marker;
        public readonly int ID;
        public int Score { get; set; }

        #endregion 

        #region Constructors

        public Player(Mark marker)
        {
            ID = ctr++;
            this.marker = marker;
        }

        #endregion

        #region Instanced Methods

        public bool PlayerWon(Cell cell, Grid grid, bool checkDiagonals) 
        {
            if (cell.HorizontalRelatives(grid).Length == 3 || cell.VerticalRelatives(grid).Length == 3)
            {
                return true;
            }
            
            if (checkDiagonals)
            {
                if (cell.DiagonalRelatives(grid).Length == 3) { return true; } 
                else if (cell.DiagonalRelatives2(grid).Length == 3) { return true; }
            }

            return false;
        }
  
        #endregion 
    }
}
