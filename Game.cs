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
        private BattleshipGameForm gameForm;

        public Game(Player player1, PlayerAi player2, int BoardSize, Difficulty difficulty, BattleshipGameForm gameForm)
        {
            this.player1 = player1;
            this.player2 = player2;
            player2.SetAIStrategy(difficulty);
            this.BoardSize = BoardSize;
            this.gameForm = gameForm;

            player1.Attach(this); // Attach the game as an observer to receive updates.
            player2.Attach(this);

            isPlayer1Turn = true; 
        }

        public void startGame()
        {
            player2.placeShips(); // Automatically place AI ships at the start of the game.
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

                // Reset the UI to allow the user to start a new game.
                var aiSetupControl = new AiSetupControl(gameForm, new GameBuilder());
                gameForm.ShowCurrentControl(aiSetupControl);
                return;
            }

            if (isPlayer1Turn)
            {
                isPlayer1Turn = false;

                LockPlayerBoard(true); // Prevent the player from interacting during the AI's turn.

                await Task.Delay(500); // Simulate a delay for the AI's action to improve the user experience.

                player2.MakeShot(player1.getBoard());

                LockPlayerBoard(false); // Allow the player to interact again.
            }
            else
            {
                isPlayer1Turn = true; // Switch to Player 1's turn.
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
            // The game is over when either player's fleet is fully sunk.
            return player1.GetFleet().isSunk() || player2.GetFleet().isSunk();
        }

        public string GetWinner()
        {
            if (!IsGameOver()) return null; // No winner if the game isn't over.
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
