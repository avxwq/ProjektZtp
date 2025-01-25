using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektZtp
{
    public abstract class Player : ISubject
    {
        public string Username;
        public Fleet PlayerFleet { get; private set; }
        public Board PlayerBoard { get; private set; }
        public Invoker Invoker { get; private set; }
        public Stack<Ship> ShipsToPlace { get; private set; }
        


        private List<IObserver> observers = new List<IObserver>();

        public Player(string username)
        {
            Username = username;
            Invoker = new Invoker();
            ShipsToPlace = new Stack<Ship>();
        }


        public void FireShot(Position position, Board board, bool isHuman)
        {
            var command = new FireShotCommand(board, position, isHuman);
            if(this.Invoker.ExecuteCommand(command)) Notify();
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

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }

        public void SetFleet(Fleet fleet)
        {
            PlayerFleet = fleet;
            for (int i = 0; i < PlayerFleet.GetComponents().Count; i++)
            {
                ShipsToPlace.Push(PlayerFleet._components[i] as Ship);
            }
        }

        public Fleet GetFleet()
        {
            return PlayerFleet;
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
                var placement = aiStrategy.GetShipPlacement(getBoard(), ShipsToPlace.Peek());
                var position = placement.Item1;
                var isHorizontal = placement.Item2;

                var command = new PlaceShipCommand(PlayerBoard, ShipsToPlace.Peek(), position, isHorizontal, ShipsToPlace, false);
                this.Invoker.ExecuteCommand(command);
            }
        }

        public void MakeShot(Board board)
        {
            Position position = aiStrategy.GetShotPosition(board);
            FireShot(position, board, false);
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