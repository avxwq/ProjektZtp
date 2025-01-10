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

        private BattleshipGameForm gameForm;
        private GameBuilder gameBuilder;
        public ChooseOpponentControl(BattleshipGameForm battleshipGameForm, GameBuilder gameBuilder)
        {
            InitializeComponent();
            gameForm = battleshipGameForm;
            this.gameBuilder = gameBuilder;
        }

        private void OpponentChooseLabel_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            gameBuilder.SetGameMode(GameMode.vsHuman);
            gameForm.ShowCurrentControl(new ChooseGameModeControl(gameForm, gameBuilder));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            gameBuilder.SetGameMode(GameMode.vsAi);
            gameForm.ShowCurrentControl(new AiSetupControl(gameForm, gameBuilder));
        }

    }
}
