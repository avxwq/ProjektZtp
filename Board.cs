using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ProjektZtp
{
    public class Board
    {
        public int boardSize { get; private set; }
        private Cell[,] cells;

        public Board(int boardSize)
        {
            this.boardSize = boardSize;
            cells = new Cell[boardSize, boardSize];
            InitializeCells();
        }

        // Initializes all cells to ensure the board is fully prepared before use.
        private void InitializeCells()
        {
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    cells[x, y] = new Cell(new Position(x, y));
                }
            }
        }

        // Validates whether a ship can be placed at the specified position without exceeding the board boundaries or overlapping another ship.
        public bool CanPlaceShip(Ship ship, Position start, bool isHorizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                int x = isHorizontal ? start.X : start.X + i;
                int y = isHorizontal ? start.Y + i : start.Y;

                // Prevents out-of-bounds placement or overlapping with an existing ship.
                if (x >= boardSize || y >= boardSize || cells[x, y].Ship != null)
                {
                    return false;
                }
            }
            return true;
        }

        // Places the ship on the board and updates cell states; ensures placement validity through CanPlaceShip.
        public bool AddShip(Ship ship, Position start, bool isHorizontal)
        {
            if (!CanPlaceShip(ship, start, isHorizontal))
            {
                return false;
            }

            for (int i = 0; i < ship.Size; i++)
            {
                int x = isHorizontal ? start.X : start.X + i;
                int y = isHorizontal ? start.Y + i : start.Y;
                var cell = GetCell(new Position(x, y));
                cell.Ship = ship;
                cell.UpdateState(); // Updates the cell's state to reflect the new ship.
            }

            return true;
        }

        // Exposes all cells to allow iterating over them (e.g., for rendering or validation).
        public IEnumerable<Cell> GetAllCells()
        {
            foreach (var cell in cells)
            {
                yield return cell;
            }
        }

        // Handles firing logic: ensures the cell isn't hit twice and updates its state to reflect a hit.
        public bool FireShot(Position position)
        {
            Cell cell = GetCell(position);
            if (cell.IsHit) return false; // Prevents hitting the same cell multiple times.
            cell.MarkAsHit();
            return true;
        }

        // Retrieves a specific cell based on the given position.
        public Cell GetCell(Position position)
        {
            return cells[position.X, position.Y];
        }
    }
}
