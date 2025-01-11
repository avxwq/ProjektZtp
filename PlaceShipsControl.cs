using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjektZtp
{
    public partial class PlaceShipsControl : UserControl
    {
        private int GridSize;
        private const int CellSize = 30;
        private Button[,] gridButtons;
        private List<Ship> shipsToPlace;
        private Ship currentShip;
        private bool isHorizontal = true;
        private GameBuilder gameBuilder;
        private BattleshipGameForm battleshipGameForm;
        private Game game;

        public PlaceShipsControl(GameBuilder gameBuilder, BattleshipGameForm battleshipGameForm)
        {
            this.gameBuilder = gameBuilder;
            this.battleshipGameForm = battleshipGameForm;
            game = gameBuilder.GetGame();
            GridSize = game.GetBoardSize();
            InitializeComponent();
            InitializeGrid();
            InitializeShips();
        }

        private void InitializeGrid()
        {
            gridButtons = new Button[GridSize, GridSize];
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    var button = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(col * CellSize, row * CellSize),
                        BackColor = Color.LightBlue,
                        Tag = new Point(row, col)
                    };
                    button.Click += GridButton_Click;
                    Controls.Add(button);
                    gridButtons[row, col] = button;
                }
            }
        }

        private void InitializeShips()
        {
            shipsToPlace = new List<Ship>
            {
                new BattleCruiser("Battle Cruiser"),
                new Frigate("Frigate"),
                new Frigate ("Warship"),
                new Frigate ("Aircraft Carrier")
            };

            SelectNextShip();
        }

        private void SelectNextShip()
        {
            if (shipsToPlace.Count > 0)
            {
                currentShip = shipsToPlace[0];
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

            var position = (Point)button.Tag;
            if (CanPlaceShip(position))
            {
                PlaceShip(position);
                SelectNextShip();
            }
            else
            {
                MessageBox.Show("Invalid position for ship placement.", "Error");
            }
        }

        private bool CanPlaceShip(Point start)
        {
            for (int i = 0; i < currentShip.Size; i++)
            {
                int row = isHorizontal ? start.X : start.X + i;
                int col = isHorizontal ? start.Y + i : start.Y;

                if (row >= GridSize || col >= GridSize || gridButtons[row, col].BackColor == Color.Gray)
                {
                    return false;
                }
            }
            return true;
        }

        private void PlaceShip(Point start)
        {
            for (int i = 0; i < currentShip.Size; i++)
            {
                int row = isHorizontal ? start.X : start.X + i;
                int col = isHorizontal ? start.Y + i : start.Y;
                gridButtons[row, col].BackColor = Color.Gray;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var toggleOrientationButton = new Button
            {
                Text = "Toggle Orientation",
                Location = new Point(GridSize * CellSize + 10, 10),
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

