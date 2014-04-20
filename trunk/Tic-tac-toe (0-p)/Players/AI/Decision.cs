using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.AI
{
    class Decision
    {
        public readonly Position Position;
        public readonly int Priority;

        public Decision(int priority, Position position)
        {
            Priority = priority;
            Position = position;
        }
    }
}
