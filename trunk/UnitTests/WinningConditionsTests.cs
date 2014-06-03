using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe;
using TicTacToe.MVP;

namespace UnitTests
{
    [TestClass]
    public class WinningConditionsTests
    {
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
    }
}
