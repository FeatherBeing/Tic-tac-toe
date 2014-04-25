using System;
using TicTacToe.MVP;

namespace TicTacToe
{
    enum Mark { Empty, Cross, Nought }

    class Cell
    {
        public delegate void CellChangedHandler(Cell cell);

        private Mark mark = Mark.Empty; /* Have to use old way of defining properties here unfortunately due to lack of support for,
                                             * default values in auto-implemented properties. */
        private IGameViewer viewer;
        public static event CellChangedHandler OnCellChanged; 
        public readonly Position Position;
        public Mark Mark { 
            get 
            { 
                return mark; 
            } 
            set 
            {
                // Only allow changes to cells without a mark 
                if (mark == Mark.Empty) 
                {    
                    mark = value;
                    OnCellChanged(this); //Model -> Viewer & Presenter, both can attach to this event
                } 
                else // It's impossible for players to change assigned cells.
                     // However the AI can do it so we need to throw an exception if they do
                { 
                    throw new UnauthorizedAccessException("AI attempted to overwrite assigned cell!");
                }
            } 
        }

        public Cell(IGameViewer viewer, Position coords)
        {
            this.viewer = viewer;
            this.Position = coords;
            viewer.DisplayCell(this);
        }

        public void Reset() //This method is necessary because we can't use the property to
        {
            mark = Mark.Empty;
            viewer.ResetCell(this);
        }
    }
}
