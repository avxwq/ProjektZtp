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

        public bool CanPlaceShip(Ship ship, Position start, bool isHorizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                int x = isHorizontal ? start.X : start.X + i;
                int y = isHorizontal ? start.Y + i : start.Y;

                if (x >= boardSize || y >= boardSize || cells[x, y].Ship != null)
                {
                    return false;
                }
            }
            return true;
        }

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
                cell.SetColor(Color.Green);
            }

            return true;
        }
        public IEnumerable<Cell> GetAllCells()
        {
            foreach (var cell in cells)
            {
                yield return cell;
            }
        }

        public ShotResult MakeShot(Position position)
        {
            var cell = cells[position.X, position.Y];
            cell.IsHit = true;

            if (cell.Ship != null)
            {
                return new ShotResult { IsHit = true, IsSunk = false }; // Simplified logic
            }

            return new ShotResult { IsHit = false, IsSunk = false };
        }

        public Cell GetCell(Position position)
        {
            return cells[position.X, position.Y];
        }

    }

    public class Cell
    {
        public Position Position { get; }
        public bool IsHit { get; set; }
        public Button Button { get; set; }
        public Ship Ship { get; set; }

        public void SetColor(Color color)
        {
            Button.BackColor = color;
        }
        public Cell(Position position)
        {
            Position = position;
            IsHit = false;
        }
    }

    public struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }




}