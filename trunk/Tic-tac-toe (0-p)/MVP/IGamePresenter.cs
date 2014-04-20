using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe;

namespace TicTacToe.MVP
{
    /// <summary>
    /// GamePresenter interface will control the flow of the game, by manipulating the model(Cell/Grid) and 
    /// formatting the data so it is presentable to the viewer(GameForm).
    /// </summary>
    interface IGamePresenter
    {
        Grid grid { get; }
        void RestartGame();
        void PlayerChoice(Player player, Position coords);
        event GameEndHandler OnGameEnd;
        event PlayedEventHandler OnPlayed;
    }
}
