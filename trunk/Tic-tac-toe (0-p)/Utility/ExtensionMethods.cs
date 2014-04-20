using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    static class ExtensionMethods
    {
        #region Cell Extension Methods

        public static Cell[] DiagonalRelatives(this Cell cell, Grid grid) 
        {
            var relatives = new List<Cell>();

            for (int x = 0; x < 3; x++)
            {
                if (grid.cells[x, x].MarkType.Equals(cell.MarkType)) { relatives.Add(grid.cells[x, x]); }
            }

            return relatives.ToArray();
        }
        
        public static Cell[] DiagonalRelatives2(this Cell cell, Grid grid)
        {
            var relatives = new List<Cell>();

            for (int x = 0; x < 3; x++)
            {
                if (grid.cells[x, 2 - x].MarkType.Equals(cell.MarkType)) { relatives.Add(grid.cells[x, 2 - x]); }
            }

            return relatives.ToArray();
        }

        public static Cell[] HorizontalRelatives(this Cell cell, Grid grid)
        {
            var relatives = new List<Cell>();
            int rowNum = grid.cells.IndexOf(cell).Y;

            for (int x = 0; x < 3; x++)
            {
                //Find row of cell
                if (grid.cells[x, rowNum].MarkType.Equals(cell.MarkType)) { relatives.Add(grid.cells[x, rowNum]); }
            }

            return relatives.ToArray();
        }
        public static Cell[] VerticalRelatives(this Cell cell, Grid grid)
        {
            var relatives = new List<Cell>();
            int colNum = grid.cells.IndexOf(cell).X;

            for (int y = 0; y < 3; y++)
            {
                //Find row of cell
                if (grid.cells[colNum, y].MarkType.Equals(cell.MarkType)) { relatives.Add(grid.cells[colNum, y]); }
            }

            return relatives.ToArray();
        }

        #endregion

        #region Cell[,] Extension Methods

        public static Cell[] GetEmptyCells(this Cell[,] cells)
        {
            List<Cell> emptyCells = new List<Cell>();

            foreach (Cell cell in cells)
            {
                if (cell.MarkType == Mark.Empty)
                {
                    emptyCells.Add(cell);
                }
            }

            return emptyCells.ToArray();
        }

        public static Position IndexOf(this Cell[,] cells, Cell cell) 
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y].Equals(cell))
                    {
                        return new Position(x, y);
                    }
                }
            }

            //If code reaches this point, then it didn't find anything, return -1
            return new Position(-1, -1);
        }

        public static Cell[] Select(this Cell[,] cells, Predicate<Cell> cellSelector)
        {
            var results = new List<Cell>();

            foreach (var cell in cells)
            {
                if (cellSelector.Invoke(cell))
                {
                    results.Add(cell);
                }
            }

            return results.ToArray();
        }

        public static Cell Find(this Cell[,] cells, Predicate<Cell> cellSelector)
        {
            foreach (var cell in cells)
            {
                if (cellSelector.Invoke(cell))
                {
                    return cell;
                }
            }

            //If it doesn't find any cell that matches predicate conditions then return null
            return null;
        }
        #endregion
    }
}
