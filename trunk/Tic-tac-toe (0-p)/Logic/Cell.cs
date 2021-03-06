﻿using System;
using TicTacToe.MVP;

namespace TicTacToe
{
    public enum Mark 
    { 
        Empty, 
        Cross, 
        Nought 
    }

    public class Cell
    {
        public delegate void CellChangedHandler(Cell cell);

        private Mark _mark = Mark.Empty; // Have to use old way of defining properties here unfortunately due to lack of support for,
                                        // default values in auto-implemented properties. 
        private readonly IGameViewer viewer;
        private readonly IGamePresenter presenter;
        public static event CellChangedHandler CellChanged; 
        public readonly Position Position;
        public Mark Mark { 
            get 
            { 
                return _mark; 
            } 
            set 
            {
                // Only allow changes to cells without a mark 
                if (_mark == Mark.Empty) 
                {    
                    _mark = value;
                    CellChanged(this); //Model -> Viewer & Presenter, both can attach to this event
                } 
                else // It's impossible for players to change assigned cells.
                     // However the AI can do it so we need to throw an exception if they do
                { 
                    throw new UnauthorizedAccessException("AI attempted to overwrite assigned cell!");
                }
            } 
        }

        public Cell(IGamePresenter presenter, IGameViewer viewer, Position coords)
        {
            this.presenter = presenter;
            this.viewer = viewer;
            this.Position = coords;
            viewer.DisplayCell(presenter, this);
        }

        public void Reset() //This method is necessary because we can't use the property to
        {
            _mark = Mark.Empty;
            viewer.ResetCell(this);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Position.ToString(), this._mark.ToString());
        }
    }
}
