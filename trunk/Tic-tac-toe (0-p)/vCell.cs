using System;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using TicTacToe.MVP;
using TicTacToe.AI;

namespace TicTacToe
{
    class VisualCell : Button
    {
        private readonly IGamePresenter presenter;
        public Position CellPosition { get; set; }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        public VisualCell(Cell cell, IGamePresenter presenter)
            : base()
        {
            this.presenter = presenter;
            AutoSize = true;
            Location = new Point(cell.Position.X * 110, cell.Position.Y * 110);
            Size = new Size(124, 124);
            Font = new Font(Font.FontFamily, 40);
            CellPosition = new Position(cell.Position.X, cell.Position.Y);
            this.SetStyle(ControlStyles.Selectable, false);
            Click += OnClick;
        }

        private void OnClick(Object sender, EventArgs e)
        {
            // Viewer -> Presenter, when btn is clicked call PlayerChoice();
            presenter.PlayerChoice(presenter.HumanPlayer, (sender as VisualCell).CellPosition); 
        }
    }
}
   
