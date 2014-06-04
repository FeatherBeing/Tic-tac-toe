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
            var expected = new DummyCell[3, 3];

            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    expected[i, j] = new DummyCell(new Position(i, j), Mark.Empty);
                }
            }

            var dummyViewer = (IGameViewer)new DummyViewer();
            var controller = new GameController(dummyViewer);

            var actual = ((DummyViewer)dummyViewer).dummyCells;

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestViewerCellModification()
        {
            var expected = new DummyCell(new Position(1,1), Mark.Nought);
            var dummyViewer = (IGameViewer)new DummyViewer();
            var controller = new GameController(dummyViewer);

            controller.Grid.cells[1,1].Mark = Mark.Nought;
            var actual = ((DummyViewer)dummyViewer).dummyCells.Find(entry => entry.Position == expected.Position);

            Assert.AreEqual(expected, actual);
        }
    }
}
