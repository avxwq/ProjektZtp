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
        private readonly Stack<ICommand> placeCommandStack;  // Stos dla komend umieszczania statków
        private readonly Stack<ICommand> shotCommandStack;   // Stos dla komend strzałów
        private readonly Stack<ICommand> redoPlaceStack;     // Stos redo dla komend umieszczania statków
        private readonly Stack<ICommand> redoShotStack;      // Stos redo dla komend strzałów

        public Invoker()
        {
            placeCommandStack = new Stack<ICommand>();
            shotCommandStack = new Stack<ICommand>();
            redoPlaceStack = new Stack<ICommand>();
            redoShotStack = new Stack<ICommand>();
        }

        // Wykonanie komendy (umieszczania statku lub strzału)
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();

            if (command is PlaceShipCommand)
            {
                placeCommandStack.Push(command);  // Dodanie do stosu dla komend umieszczania statków
            }
            //else if (command is MakeShotCommand)
            //{
            //    shotCommandStack.Push(command);   // Dodanie do stosu dla komend strzałów
            //}

            //// Kasowanie stosów redo po wykonaniu nowej komendy
            //redoPlaceStack.Clear();
            //redoShotStack.Clear();
        }

        // Cofnięcie ostatniej komendy (umieszczania statku lub strzału)
        public bool Undo()
        {
            if (placeCommandStack.Count > 0)
            {
                var command = placeCommandStack.Pop();
                command.Undo();
                redoPlaceStack.Push(command);
                return true;//Dodanie do stosu redo dla umieszczania statków
            }
            else if (shotCommandStack.Count > 0)
            {
                var command = shotCommandStack.Pop();
                command.Undo();
                redoShotStack.Push(command);
                return true;// Dodanie do stosu redo dla strzałów
            }
            else
            {
                Console.WriteLine("No commands to undo.");
                return false;
            }
        }

        // Ponowne wykonanie ostatniej cofniętej komendy
        public bool Redo()
        {
            if (redoPlaceStack.Count > 0)
            {
                var command = redoPlaceStack.Pop();
                command.Execute();
                placeCommandStack.Push(command);
                return true;// Dodanie do stosu umieszczania statków
            }
            else if (redoShotStack.Count > 0)
            {
                var command = redoShotStack.Pop();
                command.Execute();
                shotCommandStack.Push(command);
                return true;// Dodanie do stosu strzałów
            }
            else
            {
                Console.WriteLine("No commands to redo.");
                return false;
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
