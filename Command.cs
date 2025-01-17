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

    public class Invoker
    {
        private readonly Stack<ICommand> commandStack;
        private readonly Stack<ICommand> redoStack;

        public Invoker()
        {
            commandStack = new Stack<ICommand>();
            redoStack = new Stack<ICommand>();
        }

        // Wykonanie komendy
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            commandStack.Push(command);  // Dodanie komendy do historii wykonanych komend
            redoStack.Clear();  // Kasujemy stos redo, ponieważ wykonano nową komendę
        }

        // Cofnięcie ostatniej komendy
        public void Undo()
        {
            if (commandStack.Count > 0)
            {
                var command = commandStack.Pop();
                command.Undo();
                redoStack.Push(command);  // Dodajemy do stosu redo
            }
            else
            {
                Console.WriteLine("No commands to undo.");
            }
        }

        // Ponowne wykonanie ostatniej cofniętej komendy
        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                var command = redoStack.Pop();
                command.Execute();
                commandStack.Push(command);  // Ponownie dodajemy do stosu undo
            }
            else
            {
                Console.WriteLine("No commands to redo.");
            }
        }
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
