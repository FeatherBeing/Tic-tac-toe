using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using TicTacToe.MVP;

namespace TicTacToe
{
    public partial class GameForm : Form, IGameViewer
    {
        private VisualCell[,] vGrid = new VisualCell[3, 3];
        private IGamePresenter presenter;

        public GameForm()
        {
            InitializeComponent();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            Cell.CellChanged += (this as IGameViewer).CellChanged;
            presenter = new GameController(this);
            presenter.GameEnd += ShowCompletionDialog; // When IGamePresenter raises GameEnd event then the viewer is free to show completion dialog
        }

        private void ShowCompletionDialog(Outcome outcome)
        {
            string outcomeText = (outcome == Outcome.Draw) ? "The game ended in a draw!" : "Player " + ((int)outcome + 1) + " won!";

            DialogResult dialogResult = MessageBox.Show("Play again?", outcomeText, MessageBoxButtons.YesNo);
            
            if (dialogResult == DialogResult.Yes)
            {
              presenter.RestartGame(); // Viewer -> Presenter, restart the game
            } 
            else if (dialogResult == DialogResult.No)
            {
                //do nothing..
            }
        }

        void IGameViewer.DisplayCell(IGamePresenter present, Cell cell)
        {
            vGrid[cell.Position.X, cell.Position.Y] = new VisualCell(cell, present);
            Controls.Add(vGrid[cell.Position.X, cell.Position.Y]);
        }

        void IGameViewer.CellChanged(Cell cell) 
        {
            //For thread safety since AIPlayer will make cross-thread calls
            if (vGrid[cell.Position.X, cell.Position.Y].InvokeRequired) 
            { 
                vGrid[cell.Position.X, cell.Position.Y].Invoke(new MethodInvoker(() => (this as IGameViewer).CellChanged(cell)));
            } 
            else 
            {
                vGrid[cell.Position.X, cell.Position.Y].Text = (cell.Mark == Mark.Cross) ? "X" : "O";
                vGrid[cell.Position.X, cell.Position.Y].Enabled = false;
            }
        }

        void IGameViewer.ResetCell(Cell cell) 
        {
            vGrid[cell.Position.X, cell.Position.Y].Text = string.Empty;
            vGrid[cell.Position.X, cell.Position.Y].Enabled = true; 
        } 
    }
}
