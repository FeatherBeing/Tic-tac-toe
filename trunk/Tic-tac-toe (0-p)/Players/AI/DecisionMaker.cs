using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.AI
{
    class DecisionMaker
    {
        private IDecisionAlgorithm algorithm = new BasicAlgorithm(); 

        public Position GetDecision(Grid grid, AIPlayer player) 
        {
            return algorithm.Invoke(grid, player);
        }

        public void Reset()
        {
            algorithm.Reset();
        }
    }
}
