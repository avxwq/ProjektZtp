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
        private List<FleetComponent> shipsToPlace;
        private Ship currentShip;
        private bool isHorizontal = true;
        private Game game;
        private BattleshipGameForm battleshipGameForm;
        private int currentShipIndex = 0;

        public PlaceShipsControl(GameBuilder gameBuilder, BattleshipGameForm battleshipGameForm)
        {
            game = gameBuilder.GetGame();
            GameBoard = game.GetPlayer1().getBoard();
            this.battleshipGameForm = battleshipGameForm;
            InitializeComponent();
            InitializeGridUI();
            InitializeShips();
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
                    var button = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(startX + y * CellSize, startY + x * CellSize),
                        BackColor = Color.LightBlue,
                        Tag = new Position(x, y)
                    };

                    button.Click += GridButton_Click;

                    Controls.Add(button);

                    GameBoard.GetCell(new Position(x, y)).Button = button;
                }
            }
        }

        private void InitializeShips()
        {
            Player player = game.GetPlayer1();
            shipsToPlace = player.PlayerFleet.GetComponents();
            currentShip = (Ship)shipsToPlace[currentShipIndex];
        }

        private void SetCurrentShip()
        {
            if (currentShipIndex < shipsToPlace.Count)
            {
                currentShip = (Ship)shipsToPlace[currentShipIndex];
                MessageBox.Show($"Place your {currentShip.Name} (size: {currentShip.Size})", "Ship Placement");
            }
            else
            {
                MessageBox.Show("All ships placed!", "Placement Complete");
                currentShip = null;
            }
        }

        private void GridButton_Click(object sender, EventArgs e)
        {
            if (currentShip == null) return;

            var button = sender as Button;
            if (button == null) return;

            // Pobranie pozycji z Tag
            var position = (Position)button.Tag;

            bool isShipPlaced = game.GetPlayer1().PlaceShip(currentShip, position, isHorizontal);

            if (isShipPlaced)
            {
                currentShipIndex++;
                SetCurrentShip();
            }
        }
        private void UndoButton_Click(object sender, EventArgs e)
        {
            var isUndo = game.GetPlayer1().Invoker.Undo();
            if (isUndo)
            {
                if (currentShipIndex > 0)
                {
                    currentShipIndex--;
                    SetCurrentShip();
                }
            }
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            var isRedo = game.GetPlayer1().Invoker.Redo();
            if (isRedo)
            {
                if (currentShipIndex < shipsToPlace.Count - 1)
                {
                    currentShipIndex++;
                    SetCurrentShip();
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

            var toggleOrientationButton = new Button
            {
                Name = "toggleOrientationButton",
                Text = "Toggle Orientation",
                Size = new Size(120, 30),
                Location = new Point(startX + boardWidth / 2 - 60, startY - 120)
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
                Location = new Point(startX + boardWidth / 2 - 100, startY - 80)
            };
            undoButton.Click += UndoButton_Click;
            Controls.Add(undoButton);

            var redoButton = new Button
            {
                Name = "redoButton",
                Text = "Redo",
                Size = new Size(80, 30),
                Location = new Point(startX + boardWidth / 2 + 20, startY - 80)
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


        private void startGameButton_Click(object sender, EventArgs e)
        {
            MainGameControl control = new MainGameControl(battleshipGameForm, game);
            battleshipGameForm.ShowCurrentControl(control);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterBoard();

            UpdateButtonPositions();
        }

        private void UpdateButtonPositions()
        {
            int boardWidth = GameBoard.boardSize * CellSize;
            int startX = (this.Width - boardWidth) / 2;
  
            var toggleOrientationButton = Controls["toggleOrientationButton"] as Button;
            var undoButton = Controls["undoButton"] as Button;
            var redoButton = Controls["redoButton"] as Button;
            var startGameButton = Controls["startGameButton"] as Button;

            if (toggleOrientationButton != null)
                toggleOrientationButton.Location = new Point(startX + boardWidth / 2 - 60, 10);

            if (undoButton != null)
                undoButton.Location = new Point(startX + boardWidth / 2 - 60, 50);

            if (redoButton != null)
                redoButton.Location = new Point(startX + boardWidth / 2 + 60, 50);

            if (startGameButton != null)
                startGameButton.Location = new Point(startX + boardWidth / 2 - 60, 90);
        }

        private void CenterBoard()
        {
            int boardWidth = GameBoard.boardSize * CellSize;
            int boardHeight = GameBoard.boardSize * CellSize;

            int startX = (this.Width - boardWidth) / 2;
            int startY = (this.Height - boardHeight) / 2;

            foreach (Control control in Controls)
            {
                if (control is Button button && button.Tag is Position position)
                {
                    button.Location = new Point(startX + position.Y * CellSize, startY + position.X * CellSize);
                }
            }
        }

    }

}
