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
        private readonly BattleshipGameForm gameForm;
        private readonly GameBuilder builder;
        public AiSetupControl(BattleshipGameForm battleshipGameForm, GameBuilder builder)
        {
            InitializeComponent();
            gameForm = battleshipGameForm;
            this.builder = builder;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Advanced")
            {
                gameForm.ShowCurrentControl(new ChooseGamePropertiesControl(gameForm, builder));
            }
            else if (comboBox2.Text == "Standard")
            {
                GameDirector director = new GameDirector(builder);
                director.CreateStandardGame("Gracz 1");
                gameForm.ShowCurrentControl(new PlaceShipsControl(builder, gameForm));
            }
        }

    }
}
