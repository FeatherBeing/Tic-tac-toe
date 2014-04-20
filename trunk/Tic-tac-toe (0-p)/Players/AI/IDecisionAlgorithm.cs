using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.AI
{
    /// <summary>
    /// Decision algorithms are modular algorithms used to calculate each next mark placement for the AI Player they will need to 
    /// implement this interface.
    /// </summary>
    interface IDecisionAlgorithm
    {
        Position Invoke(Grid boardState, AIPlayer player);
        void Reset();
    }
}
