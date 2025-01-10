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
    public partial class AiSetupControl : UserControl
    {
        BattleshipGameForm gameForm;
        public AiSetupControl(BattleshipGameForm battleshipGameForm)
        {
            InitializeComponent();
            gameForm = battleshipGameForm;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            gameForm.ShowCurrentControl(new MainGameControl(gameForm));
        }
    }
}
