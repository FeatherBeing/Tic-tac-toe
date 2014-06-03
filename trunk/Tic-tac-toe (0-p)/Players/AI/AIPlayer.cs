using System;
using System.Threading.Tasks;
using TicTacToe;
using TicTacToe.MVP;

namespace TicTacToe.AI
{
    public class AIPlayer : Player
    {
        private readonly IGamePresenter presenter;
        private readonly DecisionMaker decisionMaker;
        public int Turn { get; set; } // We aren't using this right now but it's good for state-based AIs
        public bool AllowPlay { get; set; }

        public AIPlayer(Mark marker, IGamePresenter presenter) : base(marker)
        {
            this.presenter = presenter;
            decisionMaker = new DecisionMaker();
            Turn = new int();
            presenter.Played += DoTurn;
            presenter.GameEnd += new GameEndHandler(delegate { AllowPlay = false; }); // Disable play for AI after game has been won
        }

        private void DoTurn(Player player)
        {
            //This check is necessary, otherwise we'll cause an infinite loop
            if (player is AIPlayer || !AllowPlay) 
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
            Turn = 0;
            decisionMaker.Reset();
        }
    }
}
