using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.MVP
{
    /// <summary>
    /// IGameViewer represents the viewing medium of the game, it could for example be WinForms, WPF or a Console application.
    /// It receives data from the Model and Presenter and displays the data in a visual way for the enduser.
    /// </summary>
    public interface IGameViewer
    {
        //This is called from Model whenever a cell changes type
        void CellChanged(Cell cell);
        //Called whenever a new cell is created
        void DisplayCell(IGamePresenter present, Cell cell);
        //Resets the cell to it's initial state, called when restarting the game
        void ResetCell(Cell cell);
    }
}
