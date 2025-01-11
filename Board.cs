using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZtp
{
    public class Board
    {
        private int BoardSize;
        private Cell[][] cells;
        private Fleet fleet;

        public Board(int boardSize, Cell[][] cells, Fleet fleet)
        {
            BoardSize = boardSize;
            this.cells = cells;
            this.fleet = fleet;
        }

        public bool PlaceFleet(Fleet fleet)
        {
            return true;
        }

        public ShotResult MakeShot(Position position)
        {
            return new ShotResult();
        }

        private bool AddShip(Ship ship, int startX, int startY, bool isHorizontal)
        {
            return true;
        }
    }
    public class Cell
    {
    }

    public class Position
    {
        public int X;
        public int Y;
    }



}
