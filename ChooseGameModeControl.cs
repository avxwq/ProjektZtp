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
    public partial class ChooseGameModeControl : UserControl
    {
        private BattleshipGameForm gameForm;
        private GameBuilder builder;
        private Game game; 
        public ChooseGameModeControl(BattleshipGameForm gameForm, GameBuilder builder)
        {
            InitializeComponent();
            this.gameForm = gameForm;
            this.builder = builder;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Standard")
            {
               builder.SetBoardSize(10);
               Fleet fleet = new Fleet("Player 1 Fleet");
                fleet.Add(new BattleCruiser("Battle cruiser"));
               fleet.Add(new Warship("Warship"));
               fleet.Add(new AircraftCarrier("Aircraft carrier"));
               fleet.Add(new Frigate("Frigate"));
               Fleet fleet2 = new Fleet("Player 2 Fleet");
               fleet2.Add(new BattleCruiser("Battle cruiser"));
               fleet2.Add(new Warship("Warship"));
               fleet2.Add(new AircraftCarrier("Aircraft carrier"));
               fleet2.Add(new Frigate("Frigate"));
                builder.SetPlayer1Fleet(fleet);
                builder.SetPlayer2Fleet(fleet2);
               builder.BuildGame();
               gameForm.ShowCurrentControl(new PlaceShipsControl(builder, gameForm)); 
            }
            else if (comboBox1.Text == "Advanced")
            {
               gameForm.ShowCurrentControl(new ChooseGamePropertiesControl(gameForm, builder)); 
            }
        }
    }
}
