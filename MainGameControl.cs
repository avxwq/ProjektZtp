using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjektZtp
{
    public partial class MainGameControl : UserControl
    {
        private const int CellSize = 30;
        private readonly Game game;
        private readonly Board playerBoard;
        private readonly Board enemyBoard;
        private readonly Button[,] playerBoardButtons;
        private readonly Button[,] enemyBoardButtons;
        private readonly BattleshipGameForm battleshipGameForm;

        public MainGameControl(BattleshipGameForm battleshipGameForm, Game game)
        {
            this.battleshipGameForm = battleshipGameForm;
            this.game = game;
            playerBoard = game.GetPlayer1().getBoard();
            enemyBoard = game.GetPlayer2().getBoard();

            playerBoardButtons = new Button[playerBoard.boardSize, playerBoard.boardSize];
            enemyBoardButtons = new Button[enemyBoard.boardSize, enemyBoard.boardSize];

            InitializeComponents();
            InitializeBoards();
        }

        private void InitializeComponents()
        {
            Size = new Size(800, 400);
        }

        private void InitializeBoards()
        {
            // Player Board UI
            GroupBox playerBoardGroup = new GroupBox
            {
                Text = "Player Board",
                Location = new Point(10, 10),
                Size = new Size(playerBoard.boardSize * CellSize + 20, playerBoard.boardSize * CellSize + 20)
            };

            for (int x = 0; x < playerBoard.boardSize; x++)
            {
                for (int y = 0; y < playerBoard.boardSize; y++)
                {
                    var button = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(y * CellSize + 10, x * CellSize + 20),
                        BackColor = Color.LightBlue
                    };

                    playerBoardButtons[x, y] = button;
                    playerBoardGroup.Controls.Add(button);

                    var cell = playerBoard.GetCell(new Position(x, y));
                    cell.Button = button;

                    UpdateButtonAppearance(button, cell);
                }
            }

            Controls.Add(playerBoardGroup);

            // Enemy Board UI
            GroupBox enemyBoardGroup = new GroupBox
            {
                Text = "Enemy Board",
                Location = new Point(playerBoard.boardSize * CellSize + 50, 10),
                Size = new Size(enemyBoard.boardSize * CellSize + 20, enemyBoard.boardSize * CellSize + 20)
            };

            for (int x = 0; x < enemyBoard.boardSize; x++)
            {
                for (int y = 0; y < enemyBoard.boardSize; y++)
                {
                    var button = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(y * CellSize + 10, x * CellSize + 20),
                        BackColor = Color.Gray,
                        Tag = new Position(x, y)
                    };

                    button.Click += EnemyBoardButton_Click;
                    enemyBoardButtons[x, y] = button;
                    enemyBoardGroup.Controls.Add(button);
                }
            }

            Controls.Add(enemyBoardGroup);
        }

        private void UpdateButtonAppearance(Button button, Cell cell)
        {
            if (cell.IsHit)
            {
                button.BackColor = cell.Ship != null ? Color.Red : Color.White;
            }
            else if (cell.Ship == null)
            {
                button.BackColor = Color.LightBlue;
            }
        }

        private void EnemyBoardButton_Click(object sender, EventArgs e)
        {
            if (game.IsGameOver())
            {
                MessageBox.Show("The game is over!", "Game Over");
                return;
            }

            var button = sender as Button;
            if (button == null) return;

            var position = (Position)button.Tag;

            if (enemyBoard.GetCell(position).IsHit)
            {
                MessageBox.Show("You already shot here!", "Invalid Move");
                return;
            }

            ShotResult shot = enemyBoard.MakeShot(position);

            button.BackColor = shot.IsHit ? Color.Red : Color.White;

            if (game.IsGameOver())
            {
                MessageBox.Show("You won!", "Game Over");
            }
            else
            {
                EnemyTurn();
            }
        }

        private void EnemyTurn()
        {
            var random = new Random();

            Position position;
            do
            {
                position = new Position(random.Next(0, playerBoard.boardSize), random.Next(0, playerBoard.boardSize));
            } while (playerBoard.GetCell(position).IsHit);

            ShotResult shot = playerBoard.MakeShot(position);

            var button = playerBoard.GetCell(position).Button;
            if (button != null)
            {
                button.BackColor = shot.IsHit ? Color.Red : Color.White;
            }

            if (game.IsGameOver())
            {
                MessageBox.Show("You lost!", "Game Over");
            }
        }
    }
}