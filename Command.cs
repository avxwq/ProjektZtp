using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektZtp
{
    public interface ICommand
    {
        bool Execute();
        void Undo();
    }

    public class Invoker
    {
        private readonly Stack<ICommand> placeCommandStack;  // Stos dla komend umieszczania statków
        private readonly Stack<ICommand> shotCommandStack;   // Stos dla komend strzałów
        private readonly Stack<ICommand> redoPlaceStack;     // Stos redo dla komend umieszczania statków

        public Invoker()
        {
            placeCommandStack = new Stack<ICommand>();
            shotCommandStack = new Stack<ICommand>();
            redoPlaceStack = new Stack<ICommand>();
        }

        // Wykonanie komendy (umieszczania statku lub strzału)
        public void ExecuteCommand(ICommand command)
        {
            if (command.Execute())
            {


                if (command is PlaceShipCommand)
                {
                    placeCommandStack.Push(command);
                    redoPlaceStack.Clear();// Dodanie do stosu dla komend umieszczania statków
                }
                //else if (command is MakeShotCommand)
                //{
                //    shotCommandStack.Push(command);   // Dodanie do stosu dla komend strzałów
                //}


                redoPlaceStack.Clear();
            }
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

        private readonly Stack<Ship> ShipsToPlace;
        private readonly bool isHuman;


        public PlaceShipCommand(Board board, Ship ship, Position start, bool isHorizontal, Stack<Ship> ShipsToPlace, bool isHuman)
        {
            this.board = board;
            this.ship = ship;
            this.start = start;
            this.isHorizontal = isHorizontal;
            this.ShipsToPlace = ShipsToPlace;

            this.isHuman = isHuman;
        }

        public bool Execute()
        {
            if (!board.AddShip(ship, start, isHorizontal))
            {
                if (isHuman) MessageBox.Show("Invalid position for ship placement. Please choose another location.");
                return false;
            }
            ship.AddCells(start, isHorizontal);
            ShipsToPlace.Pop();


            return true;
        }

        public void Undo()
        {
            // Usunięcie statku z planszy
            for (int i = 0; i < ship.Size; i++)
            {
                int x = isHorizontal ? start.X : start.X + i;
                int y = isHorizontal ? start.Y + i : start.Y;

                var cell = board.GetCell(new Position(x, y));
                if (cell != null)
                {
                    cell.Ship = null; // Usuwamy odniesienie do statku z komórki
                    var button = cell.Button;
                    if (button != null)
                    {
                        button.BackColor = Color.LightBlue; // Przywracamy kolor komórki
                    }
                }
            }

            // Wyczyszczenie listy komórek statku
            ship.Cells.Clear();

            // Dodanie statku z powrotem na stos
            ShipsToPlace.Push(ship);
        }
    }

    public class MakeShotCommand : ICommand
    {
        private readonly Board board;
        private readonly Position position;
        private readonly bool isHorizontal;
        bool isHuman;


        public MakeShotCommand(Board board, Position position, bool isHuman)
        {
            this.board = board;
            this.position = position;
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {

        }
    }

}