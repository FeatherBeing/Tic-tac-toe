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
            presenter = new GameController(this);
            presenter.GameEnd += ShowCompletionDialog; // When IGamePresenter raises GameEnd event then the viewer is free to show completion dialog
            Cell.OnCellChanged += (this as IGameViewer).CellChanged;
        }

        private void ShowCompletionDialog(Outcome outcome)
        {
            long winner = (long)outcome + 1;
            string outcomeText = (outcome == Outcome.Draw) ? "The game ended in a draw!" : "Player " + winner + " won!";

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

        void IGameViewer.DisplayCell(Cell cell)
        {
            vGrid[cell.Position.X, cell.Position.Y] = new VisualCell() 
            {
                AutoSize = true,
                Location = new Point(cell.Position.X * 110, cell.Position.Y * 110),
                Size = new Size(124, 124),
                Font = new Font(Font.FontFamily, 40),
                CellPosition = new Position(cell.Position.X, cell.Position.Y)
            };

            vGrid[cell.Position.X, cell.Position.Y].Click += new EventHandler(
                        (a, b) =>
                        {
                            var position = new Position((a as VisualCell).CellPosition.X, (a as VisualCell).CellPosition.Y);
                            var player = Array.Find(presenter.Players, p => p.marker == Mark.Cross);
                            presenter.PlayerChoice(player, position); // Viewer -> Presenter, when btn is clicked call PlayerChoice();
                        });

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
