using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektZtp
{
    public class Game : IObserver
    {
        private Player player1;
        private PlayerAi player2;
        public int BoardSize;
        private bool isPlayer1Turn;

        public event Action GameOverEvent;
        public Game(Player player1, PlayerAi player2, int BoardSize, Difficulty difficulty)
        {
            this.player1 = player1;
            this.player2 = player2;
            player2.SetAIStrategy(difficulty);
            this.BoardSize = BoardSize;

            player1.Attach(this);
            player2.Attach(this);

            isPlayer1Turn = true;
        }
        public void startGame()
        {
            player2.placeShips();
        }
        public int GetBoardSize()
        {
            return BoardSize;
        }

        public async void Update()
        {
            if (IsGameOver())
            {
                var winner = GetWinner();
                MessageBox.Show($"Game over! Winner: {winner}");

            }

            if (isPlayer1Turn)
            {
                isPlayer1Turn = false;

                LockPlayerBoard(true);

                await Task.Delay(500);

                player2.MakeShot(player1.getBoard());

                LockPlayerBoard(false);
            }
            else
            {
                isPlayer1Turn = true;
            }
        }

        private void LockPlayerBoard(bool lockBoard)
        {
            foreach (var cell in player2.getBoard().GetAllCells())
            {
                cell.Button.Enabled = !lockBoard;
            }
        }

        public bool IsGameOver()
        {
            return player1.GetFleet().isSunk() || player2.GetFleet().isSunk();
        }

        public string GetWinner()
        {
            if (!IsGameOver()) return null;
            return player1.GetFleet().isSunk() ? "AI" : player1.Username;
        }

        public Player GetPlayer1()
        {
            return player1;
        }
        public Player GetPlayer2()
        {
            return player2;
        }
    }



}