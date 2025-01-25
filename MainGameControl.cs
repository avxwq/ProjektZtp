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
        private readonly BattleshipGameForm battleshipGameForm;

        public MainGameControl(BattleshipGameForm battleshipGameForm, Game game)
        {
            this.battleshipGameForm = battleshipGameForm;
            this.game = game;
            playerBoard = game.GetPlayer1().getBoard();
            enemyBoard = game.GetPlayer2().getBoard();

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
                    var cell = playerBoard.GetCell(new Position(x, y));
                    cell.Button.Size = new Size(CellSize, CellSize);
                    cell.Button.Location = new Point(y * CellSize + 10, x * CellSize + 20);
                    cell.Button.Tag = new Position(x, y);
                    playerBoardGroup.Controls.Add(cell.Button);

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
                    var cell = enemyBoard.GetCell(new Position(x, y));
                    cell.Button.Size = new Size(CellSize, CellSize);
                    cell.Button.Location = new Point(y * CellSize + 10, x * CellSize + 20);
                    cell.Button.BackColor = Color.LightBlue;
                    cell.Button.Tag = new Position(x, y);

                    

                    cell.Button.Click += EnemyBoardButton_Click;
                    enemyBoardGroup.Controls.Add(cell.Button);
                }
            }

            Controls.Add(enemyBoardGroup);
        }



        private void EnemyBoardButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var position = (Position)button.Tag;

            game.GetPlayer1().FireShot(position, enemyBoard, true);

        }


    }
}