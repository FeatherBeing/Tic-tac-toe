using System;
using System.Windows.Forms;

namespace TicTacToe
{
    class VisualCell : Button
    {
        public Position CellPosition { get; set; }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        public VisualCell(): base()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }
}
   
