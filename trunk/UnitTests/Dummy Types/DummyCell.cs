using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe;

namespace UnitTests
{
    class DummyCell
    {
        public readonly Position Position;
        public Mark Mark;

        public DummyCell(Position position, Mark mark)
        {
            this.Position = position;
            this.Mark = mark;
        }

        public override bool Equals(object obj)
        {
            var target = (DummyCell)obj;

            if (this.Position == target.Position && this.Mark == target.Mark)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, ({1})", this.Position, this.Mark);
        }
    }
}
