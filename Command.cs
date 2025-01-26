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
        private readonly Stack<ICommand> placeCommandStack;  // Tracks placed ship commands for undo functionality.
        private readonly Stack<ICommand> shotCommandStack;   // Tracks shot commands for potential future use.
        private readonly Stack<ICommand> redoPlaceStack;     // Tracks undone commands for redo functionality.

        public Invoker()
        {
            placeCommandStack = new Stack<ICommand>();
            shotCommandStack = new Stack<ICommand>();
            redoPlaceStack = new Stack<ICommand>();
        }

        public bool ExecuteCommand(ICommand command)
        {
            if (command.Execute())
            {
                if (command is PlaceShipCommand)
                {
                    placeCommandStack.Push(command);
                    redoPlaceStack.Clear(); // Clear redo stack on a new command to avoid invalid redo states.
                }
                else if (command is FireShotCommand)
                {
                    shotCommandStack.Push(command);
                }
                return true;
            }
            return false;
        }

        public bool Undo()
        {
            if (placeCommandStack.Count > 0)
            {
                var command = placeCommandStack.Pop();
                command.Undo();
                redoPlaceStack.Push(command); // Store undone command in redo stack.
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Redo()
        {
            if (redoPlaceStack.Count > 0)
            {
                var command = redoPlaceStack.Pop();
                command.Execute();
                placeCommandStack.Push(command);
                return true;
            }
            else
            {
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
            // Validate the placement and provide feedback for invalid actions.
            if (!board.AddShip(ship, start, isHorizontal))
            {
                if (isHuman) MessageBox.Show("Invalid position for ship placement. Please choose another location.");
                return false;
            }

            // Register ship cells on the board after successful placement.
            ship.AddCells(board, start, isHorizontal);
            ShipsToPlace.Pop(); // Remove the ship from the placement stack.
            return true;
        }

        public void Undo()
        {
            // Removes the ship from the board by clearing its associated cells.
            for (int i = 0; i < ship.Size; i++)
            {
                int x = isHorizontal ? start.X : start.X + i;
                int y = isHorizontal ? start.Y + i : start.Y;

                var cell = board.GetCell(new Position(x, y));
                if (cell != null)
                {
                    cell.Ship = null; // Detach the ship from the cell.
                }
                cell.UpdateState(); // Refresh cell appearance after ship removal.
            }

            // Clear the ship's registered cells and return it to the stack for re-placement.
            ship.Cells.Clear();
            ShipsToPlace.Push(ship);
        }
    }

    public class FireShotCommand : ICommand
    {
        private readonly Board board;
        private readonly Position position;
        private readonly bool isHuman;

        public FireShotCommand(Board board, Position position, bool isHuman)
        {
            this.board = board;
            this.position = position;
            this.isHuman = isHuman;
        }

        public bool Execute()
        {
            // Ensure the position hasn't already been targeted.
            if (!board.FireShot(position))
            {
                if (isHuman) MessageBox.Show("You already shot here!", "Invalid Move");
                return false;
            }
            return true;
        }

        public void Undo()
        {
            // Undo logic for a shot can be implemented if necessary (e.g., for advanced game rules).
        }
    }
}
