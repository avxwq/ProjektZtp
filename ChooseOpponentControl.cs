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
            Player player1 = new PlayerHuman();
            Player player2 = new PlayerHuman();
            gameBuilder.SetGameMode(GameMode.vsHuman);
            gameBuilder.SetPlayer1(player1);
            gameBuilder.SetPlayer2(player2);
            gameForm.ShowCurrentControl(new ChooseGameModeControl(gameForm, gameBuilder));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Player player1 = new PlayerHuman();
            Player player2 = new PlayerAi();
            gameBuilder.SetGameMode(GameMode.vsAi);
            gameBuilder.SetPlayer1(player1);
            gameBuilder.SetPlayer2(player2);
            gameForm.ShowCurrentControl(new AiSetupControl(gameForm, gameBuilder));
        }
    }
}
