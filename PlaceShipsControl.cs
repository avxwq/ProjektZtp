using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjektZtp
{
    public partial class PlaceShipsControl : UserControl
    {
        private const int CellSize = 30;
        private readonly Board GameBoard;
        private bool isHorizontal = true;
        private Game game;
        private BattleshipGameForm battleshipGameForm;

        public PlaceShipsControl(GameBuilder gameBuilder, BattleshipGameForm battleshipGameForm)
        {
            game = gameBuilder.GetGame();
            GameBoard = game.GetPlayer1().getBoard();
            this.battleshipGameForm = battleshipGameForm;
            InitializeComponent();
            InitializeGridUI();

        }

        private void InitializeGridUI()
        {
            int boardWidth = GameBoard.boardSize * CellSize;
            int boardHeight = GameBoard.boardSize * CellSize;

            int startX = (this.Width - boardWidth) / 2;
            int startY = (this.Height - boardHeight - 60) / 2;

            for (int x = 0; x < GameBoard.boardSize; x++)
            {
                for (int y = 0; y < GameBoard.boardSize; y++)
                {
                    var cell = GameBoard.GetCell(new Position(x, y));
                    cell.Button.Size = new Size(CellSize, CellSize);
                    cell.Button.Location = new Point(startX + y * CellSize, startY + x * CellSize);
                    cell.Button.Tag = new Position(x, y);
                    

                    cell.Button.Click += GridButton_Click;

                    Controls.Add(cell.Button);

                }
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int boardWidth = GameBoard.boardSize * CellSize;
            int boardHeight = GameBoard.boardSize * CellSize;
            int startX = (this.Width - boardWidth) / 2;
            int startY = (this.Height - boardHeight) / 2;

            var currentShipLabel = new Label
            {
                Name = "currentShipLabel",
                Text = GetCurrentShipText(),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(boardWidth, 30),
                Location = new Point(startX, startY - 70),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(currentShipLabel);

            var toggleOrientationButton = new Button
            {
                Name = "toggleOrientationButton",
                Text = "Toggle Orientation",
                Size = new Size(120, 30),
                Location = new Point(startX + boardWidth / 2 - 60, startY - 150)
            };
            toggleOrientationButton.Click += (s, args) =>
            {
                isHorizontal = !isHorizontal;
                MessageBox.Show($"Orientation: {(isHorizontal ? "Horizontal" : "Vertical")}", "Orientation Toggled");
            };
            Controls.Add(toggleOrientationButton);

            var undoButton = new Button
            {
                Name = "undoButton",
                Text = "Undo",
                Size = new Size(80, 30),
                Location = new Point(startX + boardWidth / 2 - 100, startY - 110)
            };
            undoButton.Click += UndoButton_Click;
            Controls.Add(undoButton);

            var redoButton = new Button
            {
                Name = "redoButton",
                Text = "Redo",
                Size = new Size(80, 30),
                Location = new Point(startX + boardWidth / 2 + 20, startY - 110)
            };
            redoButton.Click += RedoButton_Click;
            Controls.Add(redoButton);

            var startGameButton = new Button
            {
                Name = "startGameButton",
                Text = "Start Game",
                Size = new Size(120, 30),
                Location = new Point(startX + boardWidth / 2 - 60, startY + boardHeight + 20)
            };
            startGameButton.Click += startGameButton_Click;
            Controls.Add(startGameButton);
        }

        private string GetCurrentShipText()
        {
            if (game.GetPlayer1().ShipsToPlace.Count > 0)
            {
                var currentShip = game.GetPlayer1().ShipsToPlace.Peek();
                return $"Place your {currentShip.Name} (size: {currentShip.Size})";
            }
            return "All ships placed!";
        }

        private void UpdateCurrentShipLabel()
        {
            var currentShipLabel = Controls["currentShipLabel"] as Label;
            if (currentShipLabel != null)
            {
                currentShipLabel.Text = GetCurrentShipText();
            }
        }

        private void GridButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var position = (Position)button.Tag;

            game.GetPlayer1().PlaceShip(position, isHorizontal, true);
            UpdateCurrentShipLabel();
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            game.GetPlayer1().Invoker.Undo();
            UpdateCurrentShipLabel();
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            game.GetPlayer1().Invoker.Redo();
            UpdateCurrentShipLabel();
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            if (game.GetPlayer1().ShipsToPlace.Count > 0)
            {
                MessageBox.Show("Place all the ships before starting the game");
                return;
            }
            game.startGame();
            MainGameControl control = new MainGameControl(battleshipGameForm, game);
            battleshipGameForm.ShowCurrentControl(control);
            
        }



    }

}