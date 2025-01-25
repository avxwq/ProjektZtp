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
            Size = new Size(1400, 1200);
        }

        private void InitializeBoards()
        {
            int totalWidth = Math.Max(playerBoard.boardSize, enemyBoard.boardSize) * CellSize + 20;

            // Wyśrodkowanie planszy gracza
            GroupBox playerBoardGroup = new GroupBox
            {
                Text = "Player Board",
                Location = new Point(
                    (Width - totalWidth * 2 - 50) / 2, // Wyśrodkowanie względem szerokości MainGameControl
                    (Height - playerBoard.boardSize * CellSize - 20) / 2 // Wyśrodkowanie względem wysokości
                ),
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
                    cell.Button.TabStop = false;
                    playerBoardGroup.Controls.Add(cell.Button);
                }
            }

            Controls.Add(playerBoardGroup);

            // Wyśrodkowanie planszy przeciwnika
            GroupBox enemyBoardGroup = new GroupBox
            {
                Text = "Enemy Board",
                Location = new Point(
                    playerBoardGroup.Right + 30, // Ustawienie z odstępem od planszy gracza
                    playerBoardGroup.Top // Ta sama pozycja pionowa co plansza gracza
                ),
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
                    cell.Button.TabStop = false;
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

            button.Parent.Focus();

            var position = (Position)button.Tag;

            game.GetPlayer1().FireShot(position, enemyBoard, true);

        }


    }
}