using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe;
using TicTacToe.MVP;

namespace UnitTests
{
    [TestClass]
    public class WinningConditionsTests
    {
        #region CROSS WINNING CONDITIONS

        [TestMethod]
        public void CrossWinHorizontally()
        {
            var controller = new GameController(new DummyViewer());
            var expected = Outcome.CrossWin;

            for (int i = 0; i < controller.Grid.cells.GetLength(0); i++)
            {
                controller.Grid.cells[i, 0].Mark = Mark.Cross;
            }

            Assert.AreEqual(expected, controller.Grid.Outcome);
        }

        [TestMethod]
        public void CrossWinVertically()
        {
            var controller = new GameController(new DummyViewer());
            var expected = Outcome.CrossWin;

            for (int i = 0; i < controller.Grid.cells.GetLength(0); i++)
            {
                controller.Grid.cells[0, i].Mark = Mark.Cross;
            }

            Assert.AreEqual(expected, controller.Grid.Outcome);
        }

        [TestMethod]
        public void CrossWinDiagonally()
        {
            var controller = new GameController(new DummyViewer());
            var expected = Outcome.CrossWin;

            for (int i = 0; i < controller.Grid.cells.GetLength(0); i++)
            {
                controller.Grid.cells[i, i].Mark = Mark.Cross;
            }

            Assert.AreEqual(expected, controller.Grid.Outcome);
        }

        [TestMethod]
        public void CrossWinDiagonally2()
        {
            var controller = new GameController(new DummyViewer());
            var expected = Outcome.CrossWin;

            for (int i = 0; i < controller.Grid.cells.GetLength(0); i++)
            {
                controller.Grid.cells[i, 2 - i].Mark = Mark.Cross;
            }

            Assert.AreEqual(expected, controller.Grid.Outcome);
        }

        #endregion

        #region NOUGHT WINNING CONDITIONS

        [TestMethod]
        public void NoughtWinHorizontally()
        {
            var controller = new GameController(new DummyViewer());
            var expected = Outcome.NoughtWin;

            for (int i = 0; i < controller.Grid.cells.GetLength(0); i++)
            {
                controller.Grid.cells[i, 0].Mark = Mark.Nought;
            }

            Assert.AreEqual(expected, controller.Grid.Outcome);
        }

        [TestMethod]
        public void NoughtWinVertically()
        {
            var controller = new GameController(new DummyViewer());
            var expected = Outcome.NoughtWin;

            for (int i = 0; i < controller.Grid.cells.GetLength(0); i++)
            {
                controller.Grid.cells[0, i].Mark = Mark.Nought;
            }

            Assert.AreEqual(expected, controller.Grid.Outcome);
        }

        [TestMethod]
        public void NoughtWinDiagonally()
        {
            var controller = new GameController(new DummyViewer());
            var expected = Outcome.NoughtWin;

            for (int i = 0; i < controller.Grid.cells.GetLength(0); i++)
            {
                controller.Grid.cells[i, i].Mark = Mark.Nought;
            }

            Assert.AreEqual(expected, controller.Grid.Outcome);
        }

        [TestMethod]
        public void NoughtWinDiagonally2()
        {
            var controller = new GameController(new DummyViewer());
            var expected = Outcome.NoughtWin;

            for (int i = 0; i < controller.Grid.cells.GetLength(0); i++)
            {
                controller.Grid.cells[i, 2 - i].Mark = Mark.Nought;
            }

            Assert.AreEqual(expected, controller.Grid.Outcome);
        }

        #endregion
    }
}
