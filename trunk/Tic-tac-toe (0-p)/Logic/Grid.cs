﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TicTacToe.MVP;

namespace TicTacToe
{
    public enum Outcome : int
    {
        None = -1, CrossWin, NoughtWin, Draw
    }

    public enum Axis
    {
        Horizontal, Vertical, Diagonal, Diagonal2
    }

    public class Grid
    {
        public const int MAX_CELLS = 3;
        public readonly Cell[,] cells = new Cell[MAX_CELLS, MAX_CELLS];
        public Outcome Outcome { get; private set; }      
        public readonly Position[] Corners;
        public readonly Position Middle;
        private readonly IGamePresenter controller;

        public Grid(IGamePresenter presenter, IGameViewer viewer)
        {
            this.controller = presenter;
            this.Outcome = Outcome.None;
            Cell.CellChanged += CheckOutcome;

            for (int x = 0; x < MAX_CELLS; x++)
            {
                for (int y = 0; y < MAX_CELLS; y++)
                {
                    cells[x, y] = new Cell(presenter, viewer, new Position(x, y));
                }
            }

            Corners = new Position[] 
            {
               this.cells[this.cells.GetLowerBound(0), this.cells.GetLowerBound(0)].Position, 
               this.cells[this.cells.GetUpperBound(0), 0].Position, 
               this.cells[0, this.cells.GetUpperBound(0)].Position, 
               this.cells[this.cells.GetUpperBound(0), this.cells.GetUpperBound(0)].Position 
            };

            Middle = this.cells[(this.cells.GetLength(0) - 1) / 2, (this.cells.GetLength(0) - 1) / 2].Position;
        }

        public void CheckOutcome(Cell cell)
        {
            var player = this.controller.Players.First(p => p.mark == cell.Mark);

            if (player.PlayerWon(cell, this, true))
            {
                this.Outcome = (player.mark == Mark.Cross) ? Outcome.CrossWin : Outcome.NoughtWin;
            }
            else if (cells.Cast<Cell>().Where(entry => entry.Mark == Mark.Empty).Count() == 0)
            {
                Outcome = Outcome.Draw;
            }
        }

        public void Reset()
        {
            this.Outcome = Outcome.None;
            this.cells.Cast<Cell>().ToList().ForEach(cell => cell.Reset());
        }

        public Position FindInAxis(Cell cell, Axis axis, Predicate<Cell> selector)
        {
            // Start by identifying the axis so we know how to search.
            var selection = new List<Cell>();

            for (int i = 0; i < MAX_CELLS; i++)
            {
                switch (axis)
                {
                    case Axis.Horizontal:
                        if (selector.Invoke(this.cells[i, cell.Position.Y]))
                        {
                            return this.cells[i, cell.Position.Y].Position;
                        }
                        break;
                    case Axis.Vertical:
                        if (selector.Invoke(this.cells[cell.Position.X, i]))
                        {
                            return this.cells[cell.Position.X, i].Position;
                        }
                        break;
                    case Axis.Diagonal:
                        if (selector.Invoke(this.cells[i, i]))
                        {
                            return this.cells[i, i].Position;
                        }
                        break;
                    case Axis.Diagonal2:
                        if (selector.Invoke(this.cells[i, 2 - i]))
                        {
                            return this.cells[i, 2 - i].Position;
                        }
                        break;
                }
            }

            // Return an empty position if we can't find any cell that matches the predicate conditions.
            return Position.Empty;
        }
 
        public List<Cell[]> GetEmptyLines()
        {
            var selection = new List<Cell[]>();

            for (int i = 0; i < MAX_CELLS; i++)
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

            for (int x = 0; x < MAX_CELLS; x++)
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

            for (int x = 0; x < MAX_CELLS; x++)
            {
                if (cells[x, 2 - x].Mark == cell.Mark) { relatives.Add(cells[x, 2 - x]); }
            }

            return relatives.ToArray();
        }

        public Cell[] HorizontalRelatives(Cell cell)
        {
            var relatives = new List<Cell>();
            int rowNum = cell.Position.Y;

            for (int x = 0; x < MAX_CELLS; x++)
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

            for (int y = 0; y < MAX_CELLS; y++)
            {
                //Find row of cell
                if (cells[colNum, y].Mark == cell.Mark) { relatives.Add(cells[colNum, y]); }
            }

            return relatives.ToArray();
        }

        public IEnumerator GetEnumerator()
        {
            return cells.GetEnumerator();
        }
    }
}