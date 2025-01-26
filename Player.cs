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
        public string Username { get; private set; }
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

        // Initializes the player's fleet and populates the stack of ships to place.
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

        public Board getBoard()
        {
            return PlayerBoard;
        }

        // Fires a shot at a specific position on the board and notifies observers if successful.
        public void FireShot(Position position, Board board, bool isHuman)
        {
            var command = new FireShotCommand(board, position, isHuman);
            if (this.Invoker.ExecuteCommand(command)) Notify();
        }

        // Places a ship on the player's board if there are ships left to place.
        public void PlaceShip(Position position, bool isHorizontal, bool isHuman)
        {
            if (ShipsToPlace.Count != 0)
            {
                var command = new PlaceShipCommand(PlayerBoard, ShipsToPlace.Peek(), position, isHorizontal, ShipsToPlace, isHuman);
                this.Invoker.ExecuteCommand(command);
            }
        }
    }

    public class PlayerHuman : Player
    {
        public PlayerHuman(string username) : base(username)
        {

        }
    }

    public class PlayerAi : Player
    {
        private IAIPlayerStrategy aiStrategy;

        public PlayerAi() : base("AI")
        {

        }

        // Places ships automatically on the board using the AI strategy.
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

        // Makes a shot on the board using the AI strategy to determine the position.
        public void MakeShot(Board board)
        {
            Position position = aiStrategy.GetShotPosition(board);
            FireShot(position, board, false);
        }

        // Sets the AI's difficulty level by selecting the corresponding strategy.
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
    }
}
