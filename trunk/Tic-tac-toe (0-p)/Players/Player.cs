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
        public readonly Mark marker;
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
            if (grid.HorizontalRelatives(cell).Length == 3 || grid.VerticalRelatives(cell).Length == 3)
            {
                return true;
            }
            
            if (checkDiagonals)
            {
                if (grid.DiagonalRelatives(cell).Length == 3) 
                { 
                    return true; 
                } 
                else if (grid.DiagonalRelatives2(cell).Length == 3) 
                { 
                    return true; 
                }
            }

            return false;
        }
  
        #endregion 
    }
}
