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

        private void ShowCompletionDialog(OutcomeType outcome)
        {
            long winner = (long)outcome + 1;
            string WonText = "Player " + winner + " won!";

            if (outcome.Equals(OutcomeType.Draw))
            { 
                WonText = "The game ended in a draw!";
            }

            DialogResult dialogResult = MessageBox.Show("Play again?", WonText, MessageBoxButtons.YesNo);
            
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
            vGrid[cell.position.X, cell.position.Y] = new VisualCell() 
            {
                AutoSize = true,
                Location = new Point(cell.position.X * 110, cell.position.Y * 110),
                Size = new Size(124, 124),
                Font = new Font(Font.FontFamily, 40),
                CellPos = new Position(cell.position.X, cell.position.Y)
            };

            vGrid[cell.position.X, cell.position.Y].Click += new EventHandler(
                        (a, b) =>
                        {
                            var pos = new Position((a as VisualCell).CellPos.X, (a as VisualCell).CellPos.Y);
                            var player = Array.Find((presenter as GameController).players, p => p.marker.Equals(Mark.Cross));
                            presenter.PlayerChoice(player, pos); // Viewer -> Presenter, when btn is clicked call PlayerChoice();
                        });

            Controls.Add(vGrid[cell.position.X, cell.position.Y]);
        }

        void IGameViewer.CellChanged(Cell cell) 
        {
            //For thread safety since AIPlayer will make cross-thread calls
            if (vGrid[cell.position.X, cell.position.Y].InvokeRequired) 
            { 
                vGrid[cell.position.X, cell.position.Y].Invoke(new MethodInvoker(() => (this as IGameViewer).CellChanged(cell)));
            } 
            else 
            {
                vGrid[cell.position.X, cell.position.Y].Text = (cell.MarkType.Equals(Mark.Cross)) ? "X" : "O";
                vGrid[cell.position.X, cell.position.Y].Enabled = false;
            }
        }

        void IGameViewer.ResetCell(Cell cell) 
        {
            vGrid[cell.position.X, cell.position.Y].Text = String.Empty;
            vGrid[cell.position.X, cell.position.Y].Enabled = true; 
        }

        #endregion 
    }
}
