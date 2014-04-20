using System;
using System.Collections.Generic;
using TicTacToe;

namespace TicTacToe.AI
{
    class GameState //: Grid
    {
        public List<GameState> Descendants;

        public GameState()
        {
            Descendants = new List<GameState>();
        }
    }
}
