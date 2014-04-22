using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    struct Position
    {
        #region Fields & Properties

        public readonly int X;
        public readonly int Y;

        #endregion

        #region Constructors

        public Position(int x, int y) 
        {
            this.X = x;
            this.Y = y;
        }

        #endregion
    }
}
