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
    public partial class ChooseOpponentControl : UserControl
    {

        BattleshipGameForm gameForm;
        public ChooseOpponentControl(BattleshipGameForm battleshipGameForm)
        {
            InitializeComponent();
            gameForm = battleshipGameForm;
        }

        private void OpponentChooseLabel_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            gameForm.ShowCurrentControl(new AiSetupControl(gameForm));
        }
    }
}
