using System;
using System.Linq;
using System.Collections.Generic;
using TicTacToe.MVP;

namespace TicTacToe
{
    public enum OutcomeType
    {
        None = -1, CrossWin, NoughtWin, Draw
    }

    class Grid
    {
        const int MAX_CELLS = 3;
        
        #region Properties & Fields
        
        public Cell[,] cells { get; set; }
        public OutcomeType Outcome { get; private set; }
        
        #endregion

        #region Constructors

        public Grid(IGameViewer viewer)
        {
            Outcome = OutcomeType.None;
            cells = new Cell[MAX_CELLS, MAX_CELLS];

            for (int x = 0; x < MAX_CELLS; x++)
            {
                for (int y = 0; y < MAX_CELLS; y++)
                {
                    cells[x, y] = new Cell(viewer, new Position(x, y));
                }
            }
        }

        #endregion

        #region Instanced Methods

        public bool CheckOutcome(Position coords, Player player)
        {
            //Check for player wins first
            var corners = new Position[] { new Position(0,0), new Position(2,0), new Position(0,2), new Position(2,2) };
            var middle = new Position(1,1);
            var checkDiagonals = false;
            
            //If the cell is at the corner or the middle we have to check for diagonal wins too
            if (corners.Any(e => e.Equals(coords)) || middle.Equals(coords)) { checkDiagonals = true; }

            if (player.PlayerWon(cells[coords.X, coords.Y], this, checkDiagonals))
            {
                switch (player.marker)
                {
                    case Mark.Cross:
                        Outcome = OutcomeType.CrossWin;
                        break;
                    case Mark.Nought:
                        Outcome = OutcomeType.NoughtWin;
                        break;
                }

                return true;
            }

            //Now we can check for draws
            if (cells.GetEmptyCells().Length == 0)
            {
                Outcome = OutcomeType.Draw;
                return true;
            }

            //If execution reaches this point then no one has won, return false
            return false;
        }

        public List<Cell[]> GetEmptyRows()
        {
            var selection = new List<Cell[]>();

            for (int i = 0; i < this.cells.GetLength(0); i++)
            {
                var rows = new List<Cell[]>() { cells[i,i].HorizontalRelatives(this), cells[i,i].VerticalRelatives(this) };

                if (i == 1)
                {
                    rows.Add(cells[i, i].DiagonalRelatives(this));
                    rows.Add(cells[i, i].DiagonalRelatives2(this));
                }

                selection.AddRange(rows.FindAll(array => array.Length.Equals(3)));
            }

            return selection;
        }

       #endregion
    }
}