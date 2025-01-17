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
        private Ship currentShip;
        private List<FleetComponent> shipsToPlace;
        private bool isHorizontal = true;
        private Game game;
        private Invoker invoker;  // Dodanie referencji do Invokera

        public PlaceShipsControl(GameBuilder gameBuilder, BattleshipGameForm battleshipGameForm)
        {
            GameBoard = new Board(gameBuilder.GetGame().GetBoardSize());
            game = gameBuilder.GetGame();
            invoker = new Invoker();  // Inicjalizacja Invokera

            InitializeComponent();
            InitializeGridUI();
            InitializeShips();
        }

        private void InitializeGridUI()
        {
            for (int x = 0; x < GameBoard.boardSize; x++)
            {
                for (int y = 0; y < GameBoard.boardSize; y++)
                {
                    var button = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(y * CellSize, x * CellSize),
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
                MessageBox.Show($"Place your ship (size: {currentShip.Size})", "Ship Placement");
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

                // Użycie Invokera do zapisania wykonanej komendy
                invoker.ExecuteCommand(command);
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

            var toggleOrientationButton = new Button
            {
                Text = "Toggle Orientation",
                Location = new Point(GameBoard.boardSize * CellSize + 10, 10),
                Size = new Size(120, 30)
            };

            toggleOrientationButton.Click += (s, args) =>
            {
                isHorizontal = !isHorizontal;
                MessageBox.Show($"Orientation: {(isHorizontal ? "Horizontal" : "Vertical")}", "Orientation Toggled");
            };

            Controls.Add(toggleOrientationButton);
        }
    }


}

