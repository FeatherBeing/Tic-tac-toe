using System;
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
        #region Fields & Properties

        private static Position[] corners = new Position[] { new Position(0, 0), new Position(2, 2) };
        private static Position[] corners2 = new Position[] { new Position(0, 2), new Position(2, 0) };
        private List<int> horizontalWin = new List<int>() { 0, 1, 2 };
        private List<int> verticalWin = new List<int>() { 0, 1, 2 };
        private bool diagonalWin = true;
        private bool diagonalWin2 = true;

        #endregion

        #region Instanced Methods

        Position IDecisionAlgorithm.Invoke(Grid grid, AIPlayer player) 
        {
            var strategy = StrategyOne(grid, player);
            var strategy2 = StrategyTwo(grid, player);

            if (strategy.Priority > strategy2.Priority) // If S1 > S2 return S1
            {
                return strategy.Position;
            } 
            else if(strategy.Priority < strategy2.Priority) // If S2 > S1 return S2
            {
                return strategy2.Position;
            } 
            else // If S1 == S2 return a random one
            { 
                var rng = new Random();
                return (rng.Next(1) > 0) ? strategy.Position : strategy2.Position;
            }
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
        /// <returns>Decision</returns>
        private Decision StrategyOne(Grid grid, AIPlayer player)
        {
            var middle = new Position(1, 1);

            // Start by analyzing board state to identify what wins that are possible
            foreach (Cell cell in grid.cells)
            {
                if (cell.MarkType.Equals(Mark.Cross))
                {
                    horizontalWin.RemoveAll(n => n.Equals(cell.position.Y));
                    verticalWin.RemoveAll(n => n.Equals(cell.position.X));

                    //If opponent has his marker in the middle then all diagonal wins are impossible
                    if (cell.position.Equals(middle) && cell.MarkType.Equals(Mark.Cross))
                    {
                        diagonalWin = false;
                        diagonalWin2 = false;

                    }
                    // For other cells different rules apply, as there are 3 horizontal and diagonal wins each
                    else if (cell.MarkType.Equals(Mark.Cross))
                    {
                        if (corners.Contains(cell.position)) // Check if diagonal win is possible
                        {
                            diagonalWin = false;
                        }

                        if (corners2.Contains(cell.position))
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
            var friendlyCells = grid.cells.Select(cell => cell.MarkType.Equals(player.marker));

            //If we don't have any friendly marks placed then we will have to base our decision on any row where a win is possible
            if (friendlyCells.Length < 1)
            {
                //Get all empty rows
                var options = grid.GetEmptyRows();
                var rnd = new Random();

                //Since these are all equally viable options just randomize our choice between them all with priority 1.
                var decision = options[rnd.Next(options.Count)];
                // Each array will always contain 3 cells so cleaner with a static number here
                return new Decision(1, decision[rnd.Next(2)].position); 
            } 
            else // Okay if we have friendly cells then we need to how many and assign each a unique priority
            {
                var options = new List<Tuple<int, Position>>();

                foreach (var cell in friendlyCells)
                {
                    var horizontalNeighbours = cell.HorizontalRelatives(grid);
                    var verticalNeighbours = cell.VerticalRelatives(grid);
                    var diagonalNeighbours = cell.DiagonalRelatives(grid);
                    var diagonalNeighbours2 = cell.DiagonalRelatives2(grid);

                    //Now check if a win can be achieved in Y-axis
                    if (horizontalWin.Contains(cell.position.Y))
                    {
                        // Okay since horizontal wins are OK at this point then just find the cell and add it to the list
                        // The priority is calculated as if (number of neighbours == 2) then priority = 3 else priority = 1.
                        options.Add(Tuple.Create((horizontalNeighbours.Length == 2) ? 3 : 1, grid.cells.Find(
                            (entry) => 
                                entry.position.Y.Equals(cell.position.Y) && 
                                entry.MarkType.Equals(Mark.Empty))
                                .position)); 
                    }

                    //Same thing here but X-axis instead
                    if (verticalWin.Contains(cell.position.X))
                    {
                        options.Add(Tuple.Create((verticalNeighbours.Length == 2) ? 3 : 1, grid.cells.Find(
                            (entry) =>
                                entry.position.X.Equals(cell.position.X) &&
                                entry.MarkType.Equals(Mark.Empty))
                                .position)); 
                    }

                    if (diagonalWin && corners.Any(pos => pos.Equals(cell.position)) || diagonalWin && cell.position.Equals(middle)) //Only check for diagonal wins if cell is in a corner
                    {
                        options.Add(Tuple.Create((diagonalNeighbours.Length == 2) ? 3 : 1, grid.cells.Find(
                            (entry) => 
                            corners.Any(pos => pos.Equals(entry.position)) && entry.MarkType.Equals(Mark.Empty) || 
                            entry.position.Equals(middle) && entry.MarkType.Equals(Mark.Empty)).position));
                    }

                    if (diagonalWin2 && corners2.Any(pos => pos.Equals(cell.position)) || diagonalWin2 && cell.position.Equals(middle)) 
                    {
                        options.Add(Tuple.Create((diagonalNeighbours2.Length == 2) ? 3 : 1, grid.cells.Find(
                            (entry) =>
                            corners2.Any(pos => pos.Equals(entry.position)) && entry.MarkType.Equals(Mark.Empty) ||
                            entry.position.Equals(middle) && entry.MarkType.Equals(Mark.Empty)).position));
                    }
                }

                //If it doesn't find any neighbouring cells that can win just place marker on an empty valid space
                if (options.Count < 1)
                {
                    var emptyRows = grid.GetEmptyRows();
                    var rnd = new Random();

                    //Since these are all equally viable options just randomize our choice between them all with priority 1.
                    var decision = emptyRows[rnd.Next(options.Count)];
                    // Each array will always contain 3 cells so cleaner with a static number here
                    return new Decision(1, decision[rnd.Next(2)].position); 
                }

                //Sort dictionary by key value
                var sortedOptions = options.OrderByDescending(entry => entry.Item1);

                //Okay now first check if we have more than one entry with the same priority
                if (sortedOptions.Where(entry => entry.Item1.Equals(sortedOptions.First().Item1)).Count() > 1)
                {
                    var selection = sortedOptions.Where(entry => entry.Item1.Equals(sortedOptions.First().Item1)).ToList();
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
            var unfriendlyCells = new List<Tuple<int,Cell>>();
            var emptyCells = grid.cells.GetEmptyCells();

            foreach (var cell in grid.cells)
            {
                if (cell.MarkType.Equals(Mark.Cross))
                {
                    unfriendlyCells.Add(Tuple.Create(1, cell)); //Set these to priority 1 by default
                }
            }

            var prioritizedOptions = new List<Tuple<int,Position>>();
            
            //Now get all relatives + empty cells of those X-marked cells
            foreach (var option in unfriendlyCells)
            {
                int horizontalNeighbours = option.Item2.HorizontalRelatives(grid).Length;
                var emptyHorizontalCells = emptyCells.Where(entry => entry.position.Y.Equals(option.Item2.position.Y));
                int verticalNeighbours = option.Item2.VerticalRelatives(grid).Length;
                var emptyVerticalCells = emptyCells.Where(entry => entry.position.X.Equals(option.Item2.position.X));
                int diagonalNeighbours = option.Item2.DiagonalRelatives(grid).Length;
                var emptyDiagonalCells = emptyCells.Where(entry => corners.Any(corner => corner.Equals(entry.position)));
                int diagonalNeighbours2 = option.Item2.DiagonalRelatives2(grid).Length;
                var emptyDiagonalCells2 = emptyCells.Where(entry => corners2.Any(corner => corner.Equals(entry.position)));

                if(horizontalNeighbours > 1 && emptyHorizontalCells.Count() == 1) 
                {
                    //If we have 1 neighbouring X-cell + 1 empty cell then we must block them off with the highest priority
                    prioritizedOptions.Add(Tuple.Create(2, emptyHorizontalCells.First().position));
                }

                if (verticalNeighbours > 1 && emptyVerticalCells.Count() == 1)
                {
                    prioritizedOptions.Add(Tuple.Create(2, emptyVerticalCells.First().position));
                }

                if (diagonalNeighbours > 1 && emptyDiagonalCells.Count() == 1)
                {
                    prioritizedOptions.Add(Tuple.Create(2, emptyDiagonalCells.First().position));
                }

                if (diagonalNeighbours2 > 1 && emptyDiagonalCells2.Count() == 1)
                {
                    prioritizedOptions.Add(Tuple.Create(2, emptyDiagonalCells2.First().position));
                }
            }

            /*For now if there's more than one prioritized block entry then we just randomly choose one, later on we should use
             * extra analysis to determine if one block can succesfully block out two axises. */

            if (prioritizedOptions.Count < 1)
            {
                //If there are no priority 2 options then filter out all empty cells that are within the same axis as a X-marked cell
                var options = emptyCells.Where(
                    (entry) => 
                        unfriendlyCells.Any(cell => cell.Item2.position.X.Equals(entry.position.X)) ||
                        unfriendlyCells.Any(cell => cell.Item2.position.Y.Equals(entry.position.Y))).ToArray();

                //Randomly return one of them
                return new Decision(1, options[new Random().Next(options.Length)].position);
            } 
            else
            {
                return new Decision(2, prioritizedOptions[new Random().Next(prioritizedOptions.Count)].Item2);
            }
        }

        #endregion
    }
}
