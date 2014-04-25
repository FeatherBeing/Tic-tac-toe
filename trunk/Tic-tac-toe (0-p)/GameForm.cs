using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using TicTacToe.MVP;

namespace TicTacToe
{
    public partial class GameForm : Form, IGameViewer
    {
        #region Fields & Properties

        private VisualCell[,] vGrid = new VisualCell[3, 3];
        private IGamePresenter presenter;

        #endregion

        #region Constructors

        public GameForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Instanced Methods

        private void Game_Load(object sender, EventArgs e)
        {
            presenter = new GameController(this);
            presenter.OnGameEnd += ShowCompletionDialog; // When IGamePresenter raises OnGameEnd event then the viewer is free to show completion dialog
            Cell.OnCellChanged += (this as IGameViewer).CellChanged;
        }

        private void ShowCompletionDialog(Outcome outcome)
        {
            long winner = (long)outcome + 1;
            string wonText = "Player " + winner + " won!";

            if (outcome.Equals(Outcome.Draw))
            { 
                wonText = "The game ended in a draw!";
            }

            DialogResult dialogResult = MessageBox.Show("Play again?", wonText, MessageBoxButtons.YesNo);
            
            if (dialogResult == DialogResult.Yes)
            {
              presenter.RestartGame(); // Viewer -> Presenter, restart the game
            } else if (dialogResult == DialogResult.No)
            {
                //do something else
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
                CellPos = new Position(cell.Position.X, cell.Position.Y)
            };

            vGrid[cell.Position.X, cell.Position.Y].Click += new EventHandler(
                        (a, b) =>
                        {
                            var pos = new Position((a as VisualCell).CellPos.X, (a as VisualCell).CellPos.Y);
                            var player = Array.Find((presenter as GameController).Players, p => p.marker.Equals(Mark.Cross));
                            presenter.PlayerChoice(player, pos); // Viewer -> Presenter, when btn is clicked call PlayerChoice();
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
                vGrid[cell.Position.X, cell.Position.Y].Text = (cell.MarkType.Equals(Mark.Cross)) ? "X" : "O";
                vGrid[cell.Position.X, cell.Position.Y].Enabled = false;
            }
        }

        void IGameViewer.ResetCell(Cell cell) 
        {
            vGrid[cell.Position.X, cell.Position.Y].Text = string.Empty;
            vGrid[cell.Position.X, cell.Position.Y].Enabled = true; 
        }

        #endregion 
    }
}
