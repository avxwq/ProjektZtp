using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektZtp
{
    public abstract class Player
    {
        public string Username;
        public Fleet PlayerFleet { get; private set; }
        public Board PlayerBoard { get; private set; }
        public Invoker Invoker { get; private set; }

        public Player(string username)
        {
            Username = username;
            Invoker = new Invoker();   
        }

        public abstract Position MakeShot();
        public bool PlaceShip(Ship currentShip, Position position, bool isHorizontal)
        {
            var command = new PlaceShipCommand(PlayerBoard, currentShip, position, isHorizontal);
            if (command.CanExecute())
            { 
                this.Invoker.ExecuteCommand(command);
                return true;
                
            }
            else
            {
                MessageBox.Show("Invalid position for ship placement. Please choose another location.");
                return false;
            }
        }


        public abstract bool AddShipToFleet(Ship ship);

        public void SetFleet(Fleet fleet)
        {
            PlayerFleet = fleet;
        }

        public void SetBoard(Board board)
        {
            PlayerBoard = board;
        }

        public void ResetBoard()
        {

        }

        public Board getBoard()
        {
            return PlayerBoard;
        }



    }

    public class PlayerHuman : Player
    {
        public PlayerHuman(string username) : base(username)
        {
            
        }

        public override bool AddShipToFleet(Ship ship)
        {
            throw new NotImplementedException();
        }

        public override Position MakeShot()
        {
            throw new NotImplementedException();
        }


    }

    public class PlayerAi : Player
    {

        public PlayerAi() : base("AI")
        {
            
        }
        public override bool AddShipToFleet(Ship ship)
        {
            throw new NotImplementedException();
        }

        public override Position MakeShot()
        {
            throw new NotImplementedException();
        }


    }
}
