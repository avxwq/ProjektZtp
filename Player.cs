using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZtp
{
    public abstract class Player
    {
        public string Username;
        public Fleet PlayerFleet;
        private Board PlayerBoard;

        public abstract Position MakeShot();
        public abstract bool PlaceShips(Fleet fleet);
        public abstract bool AddShipToFleet(Ship ship);

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
        public override bool AddShipToFleet(Ship ship)
        {
            throw new NotImplementedException();
        }

        public override Position MakeShot()
        {
            throw new NotImplementedException();
        }

        public override bool PlaceShips(Fleet fleet)
        {
            throw new NotImplementedException();
        }
    }

    public class PlayerAi : Player
    {
        public override bool AddShipToFleet(Ship ship)
        {
            throw new NotImplementedException();
        }

        public override Position MakeShot()
        {
            throw new NotImplementedException();
        }

        public override bool PlaceShips(Fleet fleet)
        {
            throw new NotImplementedException();
        }
    }
}
