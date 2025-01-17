using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZtp
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }


    public class PlaceShipCommand : ICommand
    {
        private readonly Board board;
        private readonly Ship ship;
        private readonly Position start;
        private readonly bool isHorizontal;
        private readonly List<Position> occupiedPositions;

        public PlaceShipCommand(Board board, Ship ship, Position start, bool isHorizontal)
        {
            this.board = board;
            this.ship = ship;
            this.start = start;
            this.isHorizontal = isHorizontal;
            occupiedPositions = new List<Position>();
        }

        public bool CanExecute()
        {
            return board.CanPlaceShip(ship, start, isHorizontal);
        }

        public void Execute()
        {
            if (!CanExecute())
            {
                throw new InvalidOperationException("Cannot place ship at the specified position.");
            }

            for (int i = 0; i < ship.Size; i++)
            {
                int x = isHorizontal ? start.X : start.X + i;
                int y = isHorizontal ? start.Y + i : start.Y;

                // Zajmowanie komórki
                board.GetCell(new Position(x, y)).IsOccupied = true;
                occupiedPositions.Add(new Position(x, y));

                // Zmieniamy kolor przycisku na zielony, aby pokazać, że statek jest ustawiony
                var button = board.GetCell(new Position(x, y)).Button;
                if (button != null)
                {
                    button.BackColor = Color.Green;  // Możesz zmienić kolor na dowolny
                }
            }
        }

        public void Undo()
        {
            foreach (var position in occupiedPositions)
            {
                board.GetCell(position).IsOccupied = false;
                var button = board.GetCell(position).Button;
                if (button != null)
                {
                    button.BackColor = Color.LightBlue;  // Przywracamy oryginalny kolor
                }
            }
            occupiedPositions.Clear();
        }
    }


}
