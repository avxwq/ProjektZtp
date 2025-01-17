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
        private readonly Stack<ICommand> commandStack;
        private List<FleetComponent> shipsToPlace;
        private Ship currentShip;
        private bool isHorizontal = true;
        private Game game;
        private BattleshipGameForm battleshipGameForm;

        public PlaceShipsControl(GameBuilder gameBuilder, BattleshipGameForm battleshipGameForm)
        {
            GameBoard = new Board(gameBuilder.GetGame().GetBoardSize());
            commandStack = new Stack<ICommand>();
            game = gameBuilder.GetGame();
            this.battleshipGameForm = battleshipGameForm;

            InitializeComponent();
            InitializeGridUI();
            InitializeShips();
        }

        private void InitializeGridUI()
        {
            // Oblicz szerokość i wysokość planszy
            int boardWidth = GameBoard.boardSize * CellSize;
            int boardHeight = GameBoard.boardSize * CellSize;

            // Oblicz pozycję początkową, aby wyśrodkować planszę w kontrolce
            int startX = (this.Width - boardWidth) / 2;
            int startY = (this.Height - boardHeight) / 2;

            // Tworzenie przycisków siatki
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

                    // Połącz UI z logiką planszy
                    GameBoard.GetCell(new Position(x, y)).Button = button;
                }
            }
        }

        private void InitializeShips()
        {
            Player player = game.GetPlayer1();
            shipsToPlace = player.PlayerFleet.GetComponents();

            SelectNextShip();
        }

        private void SelectNextShip()
        {
            if (shipsToPlace.Count > 0)
            {
                currentShip = (Ship)shipsToPlace[0];
                shipsToPlace.RemoveAt(0);
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

            var command = new PlaceShipCommand(GameBoard, currentShip, position, isHorizontal);

            // Sprawdzenie, czy można wykonać komendę
            if (command.CanExecute())
            {
                command.Execute();
                commandStack.Push(command);
                SelectNextShip();
            }
            else
            {
                MessageBox.Show("Invalid position for ship placement.", "Error");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int boardWidth = GameBoard.boardSize * CellSize;
            int startX = (this.Width - boardWidth) / 2;

            var toggleOrientationButton = new Button
            {
                Text = "Toggle Orientation",
                Size = new Size(120, 30),
                Location = new Point(startX + boardWidth / 2 - 60, 10) // Wyśrodkowanie względem planszy
            };

            toggleOrientationButton.Click += (s, args) =>
            {
                isHorizontal = !isHorizontal;
                MessageBox.Show($"Orientation: {(isHorizontal ? "Horizontal" : "Vertical")}", "Orientation Toggled");
            };

            Controls.Add(toggleOrientationButton);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MainGameControl control = new MainGameControl(battleshipGameForm, game);
            battleshipGameForm.ShowCurrentControl(control);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterBoard();
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
