using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.AI;
using TicTacToe.MVP;

namespace TicTacToe
{
    public delegate void GameEndHandler(Outcome outcome);
    internal delegate void PlayedEventHandler(Cell cell, Player player);

    class GameController : IGamePresenter 
    {

        public Player[] Players { get; private set; }
        public event GameEndHandler GameEnd;
        public event PlayedEventHandler Played;
        public Grid Grid { get; private set; }

        public GameController(IGameViewer viewer) 
        { 
            Grid = new Grid(this, viewer);
            Players = new Player[2];
            Players[0] = new Player(Mark.Cross); // Always set to human 
            Players[1] = new AIPlayer(Mark.Nought, this); // Set to AI            
        }

        void IGamePresenter.PlayerChoice(Player player, Position position) 
        {
            //This is so the AI can start playing again when a new round is started
            (Players[1] as AIPlayer).AllowPlay = true;

            // Presenter -> Model, place _mark at position
            Grid.cells[position.X, position.Y].Mark = player.mark;

            // Model -> Presenter, if grid reaches an outcome end the game
            if (Grid.HasWinner(position, player)) 
            { 
                GameEnd(Grid.Outcome); 
            }

            //Raise Played Event
            Played(Grid.cells[position.X, position.Y], player);  
        }

        void IGamePresenter.RestartGame() 
        {
            // Reset grid
            Grid.cells.Cast<Cell>().ToList().ForEach(cell => cell.Reset());

            // Reset AIPlayer
            (Players[1] as AIPlayer).Reset();
        }
    }
}
