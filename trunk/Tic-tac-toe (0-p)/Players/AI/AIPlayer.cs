using System;
using System.Threading.Tasks;
using TicTacToe;
using TicTacToe.MVP;

namespace TicTacToe.AI
{
    class AIPlayer : Player
    {
        #region Fields & Properties

        private IGamePresenter presenter;
        private DecisionMaker decisionMaker;
        public int Turn { get; set; }
        public bool DisallowPlay { get; set; }

        #endregion

        #region Constructors

        public AIPlayer(Mark marker, IGamePresenter presenter) : base(marker)
        {
            this.presenter = presenter;
            decisionMaker = new DecisionMaker();
            Turn = new int();
            presenter.OnPlayed += DoTurn;
            presenter.OnGameEnd += new GameEndHandler(x => DisallowPlay = true); // Disable play for AI after game has been won
        }

        #endregion

        #region Instanced Methods

        private void DoTurn(Cell cell, Player player)
        {
            //This check is necessary, otherwise we'll cause an infinite loop
            if (player is AIPlayer || DisallowPlay) 
            { 
                return;
            }

            var decision = decisionMaker.GetDecision(presenter.Grid, this);

            Turn++;
            Task.Delay(250).Wait(); // This is just a cosmetic thing, so it'll look like the AI needs some "thinking time"
            presenter.PlayerChoice(this, decision);
        }

        public void Reset()
        {
            decisionMaker.Reset();
        }

        #endregion
    }
}
