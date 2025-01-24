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
        public Stack<Ship> ShipsToPlace { get; private set; }

        public Player(string username)
        {
            Username = username;
            Invoker = new Invoker();
            ShipsToPlace = new Stack<Ship>();
        }


        public void MakeShot(Position position)
        {

        }

        public void PlaceShip(Position position, bool isHorizontal, bool isHuman)
        {
            if (ShipsToPlace.Count != 0)
            {
                var command = new PlaceShipCommand(PlayerBoard, ShipsToPlace.Peek(), position, isHorizontal, ShipsToPlace, isHuman);
                this.Invoker.ExecuteCommand(command);
            }
        }
        public abstract bool AddShipToFleet(Ship ship);

        public void SetFleet(Fleet fleet)
        {
            PlayerFleet = fleet;
            for (int i = 0; i < PlayerFleet.GetComponents().Count; i++)
            {
                ShipsToPlace.Push(PlayerFleet._components[i] as Ship);
            }
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



    }

    public class PlayerAi : Player
    {
        private IAIPlayerStrategy aiStrategy;

        public PlayerAi() : base("AI")
        {

        }

        public void placeShips()
        {
            while (ShipsToPlace.Count > 0)
            {

            }
        }

        public void SetAIStrategy(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.easy:
                    aiStrategy = new EasyAIPlayerStrategy();
                    break;
                case Difficulty.medium:
                    aiStrategy = new MediumAIPlayerStrategy();
                    break;
                case Difficulty.hard:
                    aiStrategy = new HardAIPlayerStrategy();
                    break;
                default:
                    aiStrategy = new EasyAIPlayerStrategy();
                    break;
            }
        }

        public override bool AddShipToFleet(Ship ship)
        {
            throw new NotImplementedException();
        }


    }
}