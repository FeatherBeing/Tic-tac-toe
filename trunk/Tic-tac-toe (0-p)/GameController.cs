using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.AI;
using TicTacToe.MVP;

namespace TicTacToe
{
    public delegate void GameEndHandler(OutcomeType outcome);
    internal delegate void PlayedEventHandler(Cell cell, Player player);

    class GameController : IGamePresenter 
    {
        #region Fields & Properties

        public readonly Player[] Players = new Player[2];
        public event GameEndHandler OnGameEnd;
        public event PlayedEventHandler OnPlayed;
        public Grid Grid { get; private set; }

        #endregion        

        #region Constructors

        public GameController(IGameViewer viewer) 
        { 
            Grid = new Grid(viewer);
            Players[0] = new Player(Mark.Cross); // Always set to human 
            Players[1] = new AIPlayer(Mark.Nought, this); // Set to AI            
        }

        #endregion

        #region Instanced Methods

        void IGamePresenter.PlayerChoice(Player player, Position position) 
        {
            //This is so the AI can start playing again when a new round is started
            (Players[1] as AIPlayer).DisallowPlay = false;

            // Presenter -> Model, place marker at coords
            Grid[position.X, position.Y].MarkType = player.marker;

            // Model -> Presenter, if grid reaches an outcome end the game
            if (Grid.CheckOutcome(position, player)) { OnGameEnd(Grid.Outcome); }

            //Raise OnPlayed Event
            OnPlayed(Grid[position.X, position.Y], player);  
        }

        void IGamePresenter.RestartGame() 
        {
            // Reset grid
            Grid.Reset();

            // Reset AIPlayer turn counter
            (Players[1] as AIPlayer).Turn = 0;
            (Players[1] as AIPlayer).Reset();
        }

        #endregion
    }
}
