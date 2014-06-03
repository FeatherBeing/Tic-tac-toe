using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Player
    {
        public readonly Mark mark;
        public int Score { get; set; }

        public Player(Mark marker)
        {
            this.mark = marker;
        }

        public bool PlayerWon(Cell cell, Grid grid, bool checkDiagonals) 
        {
            return (grid.HorizontalRelatives(cell).Length == 3 || grid.VerticalRelatives(cell).Length == 3) ? true 
                    : (checkDiagonals) ?
                        (grid.DiagonalRelatives(cell).Length == 3 || grid.DiagonalRelatives2(cell).Length == 3) ? true : false 
                    : false;
        }
    }
}
