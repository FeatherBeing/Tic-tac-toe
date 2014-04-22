using System;
using System.Linq;
using System.Collections.Generic;
using TicTacToe.MVP;

namespace TicTacToe
{
    public enum Outcome
    {
        None = -1, CrossWin, NoughtWin, Draw
    }

    class Grid
    {
        const int MAX_CELLS = 3;                
        public Cell[,] Cells { get; set; }
        private Cell[,] cells = new Cell[MAX_CELLS, MAX_CELLS];
        public Cell this[int index, int index2]
        {
            get
            {
                if (index <= cells.GetUpperBound(0) && index >= cells.GetLowerBound(0) && index2 <= cells.GetUpperBound(1) && index2 >= cells.GetLowerBound(1))
                {
                    return cells[index, index2];
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }

            }
            set 
            {
                if (index <= cells.GetUpperBound(0) && index >= cells.GetLowerBound(0) && index2 <= cells.GetUpperBound(1) && index2 >= cells.GetLowerBound(1))
                {
                    cells[index, index2] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
        public Outcome Outcome { get; private set; }        

        public Grid(IGameViewer viewer)
        {
            Outcome = Outcome.None;

            for (int x = 0; x < MAX_CELLS; x++)
            {
                for (int y = 0; y < MAX_CELLS; y++)
                {
                    this[x, y] = new Cell(viewer, new Position(x, y));
                }
            }
        }

        public bool CheckOutcome(Position coords, Player player)
        {
            //Check for player wins first
            var corners = new Position[] { new Position(0,0), new Position(2,0), new Position(0,2), new Position(2,2) };
            var middle = new Position(1,1);
            var checkDiagonals = false;
            
            //If the cell is at the corner or the middle we have to check for diagonal wins too
            if (corners.Any(e => e.Equals(coords)) || middle.Equals(coords)) { checkDiagonals = true; }

            if (player.PlayerWon(this[coords.X, coords.Y], this, checkDiagonals))
            {
                switch (player.marker)
                {
                    case Mark.Cross:
                        Outcome = Outcome.CrossWin;
                        break;
                    case Mark.Nought:
                        Outcome = Outcome.NoughtWin;
                        break;
                }

                return true;
            }

            //Now we can check for draws
            if (this.GetEmptyCells().Length == 0)
            {
                Outcome = Outcome.Draw;
                return true;
            }

            //If execution reaches this point then no one has won, return false
            return false;
        }

        public List<Cell[]> GetEmptyLines()
        {
            var selection = new List<Cell[]>();

            for (int i = 0; i < this.cells.GetLength(0); i++)
            {
                var rows = new List<Cell[]>() { this.HorizontalRelatives(cells[i, i]), this.VerticalRelatives(cells[i, i]) };

                if (i == 1)
                {
                    rows.Add(this.DiagonalRelatives(cells[i, i]));
                    rows.Add(this.DiagonalRelatives2(cells[i, i]));
                }

                selection.AddRange(rows.FindAll(array => array.Length.Equals(3)));
            }

            return selection;
        }

        public Cell[] GetEmptyCells()
        {
            List<Cell> emptyCells = new List<Cell>();

            foreach (Cell cell in cells)
            {
                if (cell.Mark == Mark.Empty)
                {
                    emptyCells.Add(cell);
                }
            }

            return emptyCells.ToArray();
        }

        public Position IndexOf(Cell cell)
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

        public Cell[] DiagonalRelatives(Cell cell)
        {
            var relatives = new List<Cell>();

            for (int x = 0; x < 3; x++)
            {
                if (cells[x, x].Mark.Equals(cell.Mark)) 
                { 
                    relatives.Add(cells[x, x]); 
                }
            }

            return relatives.ToArray();
        }

        public Cell[] DiagonalRelatives2(Cell cell)
        {
            var relatives = new List<Cell>();

            for (int x = 0; x < 3; x++)
            {
                if (cells[x, 2 - x].Mark.Equals(cell.Mark)) { relatives.Add(cells[x, 2 - x]); }
            }

            return relatives.ToArray();
        }

        public Cell[] HorizontalRelatives(Cell cell)
        {
            var relatives = new List<Cell>();
            int rowNum = this.IndexOf(cell).Y;

            for (int x = 0; x < 3; x++)
            {
                //Find row of cell
                if (cells[x, rowNum].Mark.Equals(cell.Mark)) { relatives.Add(cells[x, rowNum]); }
            }

            return relatives.ToArray();
        }

        public Cell[] VerticalRelatives(Cell cell)
        {
            var relatives = new List<Cell>();
            int colNum = this.IndexOf(cell).X;

            for (int y = 0; y < 3; y++)
            {
                //Find row of cell
                if (cells[colNum, y].Mark.Equals(cell.Mark)) { relatives.Add(cells[colNum, y]); }
            }

            return relatives.ToArray();
        }

        public Cell[] Where(Predicate<Cell> cellSelector)
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

        public Cell Find(Predicate<Cell> cellSelector)
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

        public void Reset()
        {
            foreach (Cell cell in cells)
            {
                cell.Reset();
            }
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            foreach (Cell cell in cells)
            {
                yield return cell;
            }
        }
    }
}