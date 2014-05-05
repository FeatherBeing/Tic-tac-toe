using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TicTacToe;

namespace TicTacToe.AI
{
    /// <summary>
    /// This is a very simple strategy that will first and foremost try to win and secondly try to force a draw.
    /// </summary>
    class BasicAlgorithm : IDecisionAlgorithm
    {
        private static Position[] corners = new Position[] { new Position(0, 0), new Position(2, 2) };
        private static Position[] corners2 = new Position[] { new Position(0, 2), new Position(2, 0) };
        private static Position middle = new Position(1, 1);
        private List<int> horizontalWin = new List<int>() { 0, 1, 2 };
        private List<int> verticalWin = new List<int>() { 0, 1, 2 };
        private bool diagonalWin = true;
        private bool diagonalWin2 = true;
        private Mark opponentMark = default(Mark);

        Position IDecisionAlgorithm.Invoke(Grid grid, AIPlayer player) 
        {
            //If this is the first invocation then assign opponentMark value to field
            if (opponentMark == default(Mark))
            {
                opponentMark = (player.mark == Mark.Cross) ? Mark.Nought : Mark.Cross;
            }

            var strategies = new Decision[] { StrategyOne(grid, player), StrategyTwo(grid, player) };

            return 
                (strategies[0].Priority == strategies[1].Priority) ? 
                    strategies[new Random().Next(1)].Position 
              : (strategies[0].Priority > strategies[1].Priority) ? 
                    strategies[0].Position : strategies[1].Position;
        }

        void IDecisionAlgorithm.Reset()
        {
            horizontalWin = new List<int>() { 0, 1, 2 };
            verticalWin = new List<int>() { 0, 1, 2 };
            diagonalWin = true;
            diagonalWin2 = true;
        }

        /// <summary>
        /// Tries to win the game, through horizontal, vertical diagonal lines. Priority returned will range from 0(no priority) to 
        /// 3 (absolute action).
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private Decision StrategyOne(Grid grid, AIPlayer player)
        {
            // Start by analyzing board state to identify what wins that are possible
            foreach (Cell cell in grid)
            {
                if (cell.Mark == opponentMark)
                {
                    horizontalWin.RemoveAll(n => n == cell.Position.Y);
                    verticalWin.RemoveAll(n => n == cell.Position.X);

                    //If opponent has his mark in the middle then all diagonal wins are impossible
                    if (cell.Position == middle && cell.Mark == opponentMark)
                    {
                        diagonalWin = false;
                        diagonalWin2 = false;

                    }
                    // For other cells different rules apply, as there are 3 horizontal and diagonal wins each
                    else if (cell.Mark == opponentMark)
                    {
                        if (corners.Contains(cell.Position)) // Check if diagonal win is possible
                        {
                            diagonalWin = false;
                        }

                        if (corners2.Contains(cell.Position))
                        {
                            diagonalWin2 = false;
                        }
                    }
                }
            }

            //Check if any type of win is possible
            if (horizontalWin.Count == 0 && verticalWin.Count == 0 && !diagonalWin && !diagonalWin2)
            {
                // In this case winning is impossible so we should then always return lowest priority from this strategy
                return new Decision(0, new Position());
            }

            //Now calculate which type of win to prioritize, starting by checking where we already have marks placed
            var friendlyCells = ((IEnumerable)grid.cells).Cast<Cell>().Where(cell => cell.Mark == player.mark);

            //If we don't have any friendly marks placed then we will have to base our decision on any row where a win is possible
            if (friendlyCells.Count() < 1)
            {
                //Get all empty rows
                var options = grid.GetEmptyLines();
                var rnd = new Random();

                //Since these are all equally viable options just randomize our choice between them all with priority 1.
                var decision = options[rnd.Next(options.Count)];
                // Each array will always contain 3 cells so cleaner with a static number here
                return new Decision(1, decision[rnd.Next(2)].Position); 
            } 
            else // Okay if we have friendly cells then we need to how many and assign each a unique priority
            {
                var options = new List<Tuple<int, Position>>();

                foreach (var cell in friendlyCells)
                {
                    var horizontalNeighbours = grid.HorizontalRelatives(cell);
                    var verticalNeighbours = grid.VerticalRelatives(cell);
                    var diagonalNeighbours = grid.DiagonalRelatives(cell);
                    var diagonalNeighbours2 = grid.DiagonalRelatives2(cell);

                    //Now check if a win can be achieved in Y-axis
                    if (horizontalWin.Contains(cell.Position.Y))
                    {
                        // Okay since horizontal wins are OK at this point then just find the cell and add it to the list
                        // The priority is calculated as if (number of neighbours == 2) then priority = 3 else priority = 1.
                        options.Add(Tuple.Create((horizontalNeighbours.Length == 2) ? 3 : 1, ((IEnumerable)grid.cells).Cast<Cell>().First(
                            (entry) => 
                                entry.Position.Y == cell.Position.Y && 
                                entry.Mark == Mark.Empty)
                                .Position)); 
                    }

                    //Same thing here but X-axis instead
                    if (verticalWin.Contains(cell.Position.X))
                    {
                        options.Add(Tuple.Create((verticalNeighbours.Length == 2) ? 3 : 1, ((IEnumerable)grid.cells).Cast<Cell>().First(
                            (entry) =>
                                entry.Position.X == cell.Position.X &&
                                entry.Mark == Mark.Empty)
                                .Position)); 
                    }

                    if (diagonalWin && corners.Any(pos => pos == cell.Position) || diagonalWin && cell.Position == middle) //Only check for diagonal wins if cell is in a corner
                    {
                        options.Add(Tuple.Create((diagonalNeighbours.Length == 2) ? 3 : 1, ((IEnumerable)grid.cells).Cast<Cell>().First(
                            (entry) => 
                                corners.Any(position => position == entry.Position && entry.Mark == Mark.Empty || 
                                entry.Position == middle) && entry.Mark == Mark.Empty)
                                .Position));
                    }

                    if (diagonalWin2 && corners2.Any(pos => pos == cell.Position) || diagonalWin2 && cell.Position == middle) 
                    {
                        options.Add(Tuple.Create((diagonalNeighbours2.Length == 2) ? 3 : 1, ((IEnumerable)grid.cells).Cast<Cell>().First(
                            (entry) =>
                                corners2.Any(position => position == entry.Position && entry.Mark == Mark.Empty ||
                                entry.Position == middle && entry.Mark == Mark.Empty))
                                .Position));
                    }
                }

                //If it doesn't find any neighbouring cells that can win just place mark on an empty valid space
                if (options.Count < 1)
                {
                    var emptyRows = grid.GetEmptyLines();
                    var rnd = new Random();

                    //Since these are all equally viable options just randomize our choice between them all with priority 1.
                    var decision = emptyRows[rnd.Next(options.Count)];
                    // Each array will always contain 3 cells so cleaner with a static number here
                    return new Decision(1, decision[rnd.Next(2)].Position); 
                }

                //Sort dictionary by key value
                var sortedOptions = options.OrderByDescending(entry => entry.Item1);

                //Okay now first check if we have more than one entry with the same priority
                if (sortedOptions.Where(entry => entry.Item1 == sortedOptions.First().Item1).Count() > 1)
                {
                    var selection = sortedOptions.Where(entry => entry.Item1 == sortedOptions.First().Item1).ToList();
                    return new Decision(sortedOptions.First().Item1, selection[new Random().Next(selection.Count)].Item2);
                } 
                else 
                {
                    return new Decision(sortedOptions.First().Item1, sortedOptions.First().Item2);
                }
            }
        }

        /// <summary>
        /// Will attempt to force a draw by analyzing board state and opponent mark placements and attempting to block them.
        /// Priority can never exceed 2 so a high-probability win will always take precedent over this strategy.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns>Decision</returns>
        private Decision StrategyTwo(Grid grid, AIPlayer player)
        {
            //Start by getting each X-marked cell
            var unfriendlyCells = new List<Cell>();
            var emptyCells = grid.cells.Cast<Cell>().Where(cell => cell.Mark == Mark.Empty);

            grid.cells.Cast<Cell>().Where(cell => cell.Mark == opponentMark).ToList().ForEach(unfriendlyCells.Add);

            var prioritizedOptions = new List<Tuple<int,Position>>();
            
            //Now get all relatives + empty cells of those X-marked cells
            foreach (var entry in unfriendlyCells)
            {
                var neighbours = new 
                { 
                    Horizontal = grid.HorizontalRelatives(entry).Length, 
                    Vertical = grid.VerticalRelatives(entry).Length,
                    Diagonal = grid.DiagonalRelatives(entry).Length,
                    Diagonal2 = grid.DiagonalRelatives2(entry).Length
                };
                var emptyLines = new 
                { 
                    Horizontal = emptyCells.Where(cell => cell.Position.Y == entry.Position.Y),
                    Vertical = emptyCells.Where(cell => cell.Position.X == entry.Position.X),
                    Diagonal = emptyCells.Where(cell => corners.Any(corner => corner == entry.Position)),
                    Diagonal2 = emptyCells.Where(cell => corners2.Any(corner => corner == entry.Position))
                };

                //If we have 1 neighbouring X-cell + 1 empty cell then we must block them off with the highest priority
                if(neighbours.Horizontal > 1 && emptyLines.Horizontal.Count() == 1) 
                {
                    prioritizedOptions.Add(Tuple.Create(2, emptyLines.Horizontal.First().Position));
                }

                if (neighbours.Vertical > 1 && emptyLines.Vertical.Count() == 1)
                {
                    prioritizedOptions.Add(Tuple.Create(2, emptyLines.Vertical.First().Position));
                }

                if (neighbours.Diagonal > 1 && emptyLines.Diagonal.Count() == 1)
                {
                    prioritizedOptions.Add(Tuple.Create(2, emptyLines.Diagonal.First().Position));
                }

                if (neighbours.Diagonal2 > 1 && emptyLines.Diagonal2.Count() == 1)
                {
                    prioritizedOptions.Add(Tuple.Create(2, emptyLines.Diagonal2.First().Position));
                }
            }

            /*For now if there's more than one prioritized block entry then we just randomly choose one, later on we should use
             * extra analysis to determine if one block can succesfully block out two axises. */

            if (prioritizedOptions.Count < 1)
            {
                //If there are no priority 2 options then filter out all empty cells that are within the same axis as a X-marked cell
                var options = emptyCells.Where(
                    (entry) => 
                        unfriendlyCells.Any(cell => cell.Position.X == entry.Position.X) ||
                        unfriendlyCells.Any(cell => cell.Position.Y == entry.Position.Y))
                        .ToArray();

                //Randomly return one of them
                return new Decision(1, options[new Random().Next(options.Length)].Position);
            } 
            else
            {
                return new Decision(2, prioritizedOptions[new Random().Next(prioritizedOptions.Count)].Item2);
            }
        }
    }
}
