using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZtp
{
    public class Game
    {
        private Player player1;
        private Player player2;
        private Board board;
        private int BoardSize;
        private GameMode mode;
        public Game(Player player1, Player player2, int BoardSize, GameMode mode)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.mode = mode;
            this.BoardSize = BoardSize;
        }
        public int GetBoardSize()
        {
            return BoardSize;
        }
    }

    public class Board
    {
        private int BoardSize;
        private Cell[][] cells;
        private Fleet fleet;

        public Board(int boardSize, Cell[][] cells, Fleet fleet)
        {
            BoardSize = boardSize;
            this.cells = cells;
            this.fleet = fleet;
        }

        public bool PlaceFleet(Fleet fleet)
        {
            return true;
        }

        public ShotResult MakeShot(Position position)
        {
            return new ShotResult();
        }

        private bool AddShip(Ship ship, int startX, int startY, bool isHorizontal)
        {
            return true;
        }
    }

    public class ShotResult
    {

    }

    public class Cell
    {
    }

    public class Position
    {
        public int X;
        public int Y;
    }

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

    public abstract class FleetComponent
    {
        public abstract string Name { get; }
        public abstract int Size { get; }

        public virtual void Add(FleetComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(FleetComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<FleetComponent> GetComponents()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Ship : FleetComponent
    {
        public override string Name { get; }
        public override abstract int Size { get; }

        protected Ship(string name)
        {
            Name = name;
        }
    }

    public class BattleCruiser : Ship
    {
        public override int Size => 5;

        public BattleCruiser(string name) : base(name)
        {
        }
    }

    public class Frigate : Ship
    {
        public override int Size => 3;

        public Frigate(string name) : base(name)
        {
        }
    }

    public class Warship : Ship
    {
        public override int Size => 4;

        public Warship(string name) : base(name)
        {
        }
    }

    public class AircraftCarrier : Ship
    {
        public override int Size => 8;

        public AircraftCarrier(string name) : base(name)
        {
        }
    }

    public class Fleet : FleetComponent
    {
        private readonly List<FleetComponent> _components = new List<FleetComponent>();

        public override string Name { get; }

        public Fleet(string name)
        {
            Name = name;
        }

        public override int Size => _components.Sum(component => component.Size);

        public override void Add(FleetComponent component)
        {
            _components.Add(component);
        }

        public override void Remove(FleetComponent component)
        {
            _components.Remove(component);
        }

        public override IEnumerable<FleetComponent> GetComponents()
        {
            return _components;
        }
    }

    public enum GameMode
    {
        vsAi,
        vsHuman
    }
    public interface IGameBuilder
    {
        void SetGameMode(GameMode gameMode);
        void SetAiStrategy();
        void SetBoardSize(int size);
        void SetBackgroundColor(Color color);
        void SetPlayer1(Player player);
        void SetPlayer2(Player player);
        void BuildStandardFleet();
        void BuildAdvancedFleet(Fleet fleet);
        void BuildGame();
        Game GetGame();
    }

    public class GameBuilder : IGameBuilder
    {
        private GameMode gameMode;
        private Player player1;
        private Player player2;
        public int boardSize;
        private Game game;
        Color backgroundColor = Color.Gray;

        public void BuildGame()
        {
            game = new Game(player1, player2, boardSize, gameMode);
        }

        public Game GetGame()
        {
            return game;
        }

        public void SetAiStrategy()
        {
            throw new NotImplementedException();
        }

        public void SetBackgroundColor(Color color)
        {
            backgroundColor = color;
        }

        public void SetBoardSize(int size)
        {
            this.boardSize = size;
        }

        public void SetGameMode(GameMode gameMode)
        {
            this.gameMode = gameMode;
        }

        public void SetPlayer1(Player player)
        {
            player1 = player;
        }
        public void SetPlayer2(Player player)
        {
            player2 = player;
        }
        
        public void BuildStandardFleet()
        {
        }
        public void BuildAdvancedFleet(Fleet fleet)
        {

        }
    }
}
