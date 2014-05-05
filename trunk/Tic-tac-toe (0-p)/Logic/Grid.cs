using System;
using System.Linq;
using System.Collections.Generic;
using TicTacToe.MVP;

namespace TicTacToe
{
    public enum Outcome : int
    {
        None = -1, CrossWin, NoughtWin, Draw
    }

    public enum Win
    {
        HorizontalWin, VerticalWin, DiagonalWin
    }

    class Grid
    {
        const int MAX_CELLS = 3;
        public readonly Cell[,] cells = new Cell[MAX_CELLS, MAX_CELLS];
        public Outcome Outcome { get; private set; }        

        public Grid(IGamePresenter presenter, IGameViewer viewer)
        {
            Outcome = Outcome.None;

            for (int x = 0; x < MAX_CELLS; x++)
            {
                for (int y = 0; y < MAX_CELLS; y++)
                {
                    cells[x, y] = new Cell(presenter, viewer, new Position(x, y));
                }
            }
        }

        public bool HasWinner(Position position, Player player)
        {
            //Check for player wins first
            var corners = new Position[] { new Position(0,0), new Position(2,0), new Position(0,2), new Position(2,2) };
            var middle = new Position(1,1);
            //If the cell is at the corner or the middle we have to check for diagonal wins too
            var checkDiagonals = corners.Any(e => e == position) || middle == position;
          
            if (player.PlayerWon(cells[position.X, position.Y], this, checkDiagonals))
            {
                Outcome = (player.mark == Mark.Cross) ? Outcome.CrossWin : Outcome.NoughtWin;
                return true;
            }

            //Now we can check for draws
            if (cells.Cast<Cell>().Where(cell => cell.Mark == Mark.Empty).Count() == 0)
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

                selection.AddRange(rows.FindAll(array => array.Length == 3 && array.First().Mark == Mark.Empty));
            }

            return selection;
        }

        public Cell[] DiagonalRelatives(Cell cell)
        {
            var relatives = new List<Cell>();

            for (int x = 0; x < 3; x++)
            {
                if (cells[x, x].Mark == cell.Mark) 
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
                if (cells[x, 2 - x].Mark == cell.Mark) { relatives.Add(cells[x, 2 - x]); }
            }

            return relatives.ToArray();
        }

        public Cell[] HorizontalRelatives(Cell cell)
        {
            var relatives = new List<Cell>();
            int rowNum = cell.Position.Y;

            for (int x = 0; x < 3; x++)
            {
                //Find row of cell
                if (cells[x, rowNum].Mark == cell.Mark) { relatives.Add(cells[x, rowNum]); }
            }

            return relatives.ToArray();
        }

        public Cell[] VerticalRelatives(Cell cell)
        {
            var relatives = new List<Cell>();
            int colNum = cell.Position.X;

            for (int y = 0; y < 3; y++)
            {
                //Find row of cell
                if (cells[colNum, y].Mark == cell.Mark) { relatives.Add(cells[colNum, y]); }
            }

            return relatives.ToArray();
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