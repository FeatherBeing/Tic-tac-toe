using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.MVP;
using TicTacToe.AI;

namespace TicTacToe
{
    public delegate void GameEndHandler(OutcomeType outcome);
    internal delegate void PlayedEventHandler(Cell cell, Player player);

    class GameController : IGamePresenter 
    {
        #region Fields & Properties

        public readonly Player[] players = new Player[2];
        public event GameEndHandler OnGameEnd;
        public event PlayedEventHandler OnPlayed;
        public Grid grid { get; private set; }

        #endregion        

        #region Constructors

        public GameController(IGameViewer viewer) 
        { 
            grid = new Grid(viewer);
            players[0] = new Player(Mark.Cross); // Always set to human 
            players[1] = new AIPlayer(Mark.Nought, this); // Set to AI            
        }

        #endregion

        #region Instanced Methods

        void IGamePresenter.PlayerChoice(Player player, Position position) 
        {
            //This is so the AI can start playing again when a new round is started
            (players[1] as AIPlayer).DisallowPlay = false;

            // Presenter -> Model, place marker at coords
            grid.cells[position.X, position.Y].MarkType = player.marker;

            // Model -> Presenter, if grid reaches an outcome end the game
            if(grid.CheckOutcome(position, player)) { OnGameEnd(grid.Outcome); }

            //Raise OnPlayed Event
            OnPlayed(grid.cells[position.X, position.Y], player);  
        }

        void IGamePresenter.RestartGame() 
        {
            // Reset each cell to it's original state
            foreach (Cell cell in grid.cells)
            {
                cell.Reset();
            }

            // Reset AIPlayer turn counter
            (players[1] as AIPlayer).Turn = 0;
            (players[1] as AIPlayer).Reset();
        }

        #endregion
    }
}
