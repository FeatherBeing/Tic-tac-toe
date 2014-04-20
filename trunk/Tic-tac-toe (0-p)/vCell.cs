using System;
using System.Windows.Forms;

namespace TicTacToe
{
    class VisualCell : Button
    {
        #region Fields & Properties

        public Position CellPos { get; set; }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Constructors

        public VisualCell(): base()
        {

            this.SetStyle(ControlStyles.Selectable, false);
        }

        #endregion
    }
}
   
