using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.AI
{
    class DecisionMaker
    {
        #region Properties & Fields

        private IDecisionAlgorithm algorithm = new BasicAlgorithm(); 

        #endregion

        #region Instanced Methods

        public Position GetDecision(Grid grid, AIPlayer player) 
        {
            return algorithm.Invoke(grid, player);
        }

        public void Reset()
        {
            algorithm.Reset();
        }

        #endregion
    }
}
