using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.MVP;
using TicTacToe;

namespace UnitTests
{
    class DummyViewer : IGameViewer
    {
        internal readonly List<DummyCell> dummyCells;

        public DummyViewer()
        {
            dummyCells = new List<DummyCell>();
        }

        void IGameViewer.CellChanged(Cell cell)
        {
            dummyCells.FirstOrDefault(dummy => dummy.Position == cell.Position).Mark = cell.Mark;
        }

        void IGameViewer.DisplayCell(IGamePresenter present, Cell cell)
        {
            dummyCells.Add(new DummyCell(cell.Position, cell.Mark));
        }

        void IGameViewer.ResetCell(Cell cell)
        {
            dummyCells.FirstOrDefault(dummy => dummy.Position == cell.Position).Mark = Mark.Empty;
        }
    }
}
