﻿using System;
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

            Fleet player1Fleet = new Fleet("Player 1 Fleet");
            Fleet player2Fleet = new Fleet("Player 2 Fleet");

            for (int i = 0; i < numBattleCruisers; i++)
            {
                player1Fleet.Add(new BattleCruiser("Battle Cruiser"));
                player2Fleet.Add(new BattleCruiser("Battle Cruiser"));
            }
            for (int i = 0; i < numFrigates; i++)
            {
                player1Fleet.Add(new Frigate("Frigate"));
                player2Fleet.Add(new Frigate("Battle Cruiser"));
            }
            for (int i = 0; i < numWarships; i++)
            {
                player1Fleet.Add(new Warship("Warship"));
                player2Fleet.Add(new Warship("Battle Cruiser"));
            }
            for (int i = 0; i < numAircraftCarriers; i++)
            {
                player1Fleet.Add(new AircraftCarrier("Aircraft Carrier"));
                player2Fleet.Add(new AircraftCarrier("Battle Cruiser"));
            }

            Player player1 = new PlayerHuman("Zbyszek");
            Player player2 = new PlayerAi();

            builder.SetPlayer1(player1);
            builder.SetPlayer2(player2);

            builder.SetPlayer1Fleet(player1Fleet);
            builder.SetPlayer2Fleet(player2Fleet);

            builder.BuildGame();
            gameForm.ShowCurrentControl(new PlaceShipsControl(builder, gameForm));
        }
    }
}
