using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    struct Position
    {
        public readonly int X;
        public readonly int Y;

        public Position(int x, int y) 
        {
            this.X = x;
            this.Y = y;
        }

        public static bool operator == (Position position, Position position2) 
        {
            return position.Equals(position2);
        }

        public static bool operator != (Position position, Position position2) 
        {
            return position.Equals(position2);
        }
    }
}
