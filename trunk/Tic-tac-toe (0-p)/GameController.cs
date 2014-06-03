using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.AI;
using TicTacToe.MVP;

namespace TicTacToe
{
    public delegate void GameEndHandler(Outcome outcome);
    internal delegate void PlayedEventHandler(Player player);

    class GameController : IGamePresenter 
    {

        public Player[] Players { get; private set; }
        public event GameEndHandler GameEnd;
        public event PlayedEventHandler Played;
        public Grid Grid { get; private set; }
        public Player HumanPlayer { get; private set; }
        public AIPlayer AIPlayer { get; private set; }

        public GameController(IGameViewer viewer) 
        { 
            Grid = new Grid(this, viewer);
            Players = new Player[2];
            Players[0] = new AIPlayer(Mark.Cross, this); 
            Players[1] = new Player(Mark.Nought);
            HumanPlayer = Players.First(player => !(player is AIPlayer));
            AIPlayer = (AIPlayer)Players.First(player => player is AIPlayer);

            // If the AI is set to cross then we must initialize him through the constructor.
            if (AIPlayer.mark == Mark.Cross) 
            {
                AIPlayer.AllowPlay = true;
                Played(null);
            }
        }

        void IGamePresenter.PlayerChoice(Player player, Position position) 
        {
            //This is so the AI can start playing again when a new round is started
            AIPlayer.AllowPlay = true;

            // Presenter -> Model, place _mark at position
            Grid.cells[position.X, position.Y].Mark = player.mark;

            // Model -> Presenter, if grid reaches an outcome end the game
            if (Grid.Outcome != Outcome.None) 
            { 
                GameEnd(Grid.Outcome); 
            }

            //Raise Played Event
            Played(player);  
        }

        void IGamePresenter.RestartGame() 
        {
            // Reset grid
            Grid.Reset();

            // Reset AIPlayer
            AIPlayer.Reset();
        }
    }
}
