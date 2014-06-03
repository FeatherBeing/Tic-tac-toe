using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe;
using TicTacToe.MVP;

namespace UnitTests
{
    [TestClass]
    public class DummyViewTests
    {
        [TestMethod]
        public void TestViewerCellCreation()
        {
            var expected = new DummyCell(new Position(1,1), Mark.Nought);
            var dummyViewer = (IGameViewer)new DummyViewer();
            Cell.CellChanged += dummyViewer.CellChanged;
            var controller = new GameController(dummyViewer);

            controller.Grid.cells[1,1].Mark = Mark.Nought;
            var actual = ((DummyViewer)dummyViewer).dummyCells.Find(entry => entry.Position == expected.Position);

            Assert.AreEqual(expected, actual);
        }
    }
}
