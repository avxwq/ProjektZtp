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
