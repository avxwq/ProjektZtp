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

            builder.SetBoardSize(BoardSize);

            Fleet player1Fleet = new Fleet();
            Fleet player2Fleet = new Fleet();

            for (int i = 0; i < numBattleCruisers; i++)
            {
                player1Fleet.Add(new BattleCruiser());
                player2Fleet.Add(new BattleCruiser());
            }
            for (int i = 0; i < numFrigates; i++)
            {
                player1Fleet.Add(new Frigate());
                player2Fleet.Add(new Frigate());
            }
            for (int i = 0; i < numWarships; i++)
            {
                player1Fleet.Add(new Warship());
                player2Fleet.Add(new Warship());
            }
            for (int i = 0; i < numAircraftCarriers; i++)
            {
                player1Fleet.Add(new AircraftCarrier());
                player2Fleet.Add(new AircraftCarrier());
            }

            builder.SetPlayer1Fleet(player1Fleet);
            builder.SetPlayer2Fleet(player2Fleet);

            builder.BuildGame();
            gameForm.ShowCurrentControl(new PlaceShipsControl(builder, gameForm));
        }
    }
}
