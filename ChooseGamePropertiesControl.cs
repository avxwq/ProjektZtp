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
    public partial class ChooseGamePropertiesControl : UserControl
    {
        private readonly BattleshipGameForm gameForm;
        private readonly GameBuilder builder;
        public ChooseGamePropertiesControl(BattleshipGameForm battleshipGameForm, GameBuilder builder)
        {
            InitializeComponent();
            gameForm = battleshipGameForm;
            this.builder = builder;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int BoardSize = int.Parse(comboBox1.Text);
            int numBattleCruisers = int.Parse(comboBox2.Text);
            int numFrigates = int.Parse(comboBox3.Text);
            int numWarships = int.Parse(comboBox4.Text);
            int numAircraftCarriers = int.Parse(comboBox5.Text);
        }
    }
}
