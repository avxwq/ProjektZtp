using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektZtp
{
    public partial class BattleshipGameForm : Form
    {
        UserControl currentControl;
        public BattleshipGameForm()
        {
            InitializeComponent();

            currentControl = new ChooseOpponentControl(this);
            this.Controls.Add(currentControl);
        }

        public void ShowCurrentControl(UserControl control)
        {
            this.Controls.Remove(currentControl);
            currentControl = control;
            this.Controls.Add(currentControl);
        }

    }
}
