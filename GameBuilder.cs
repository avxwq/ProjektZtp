using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektZtp
{
    public interface IGameBuilder
    {
        void SetAiStrategy(Difficulty difficulty);
        void SetBoardSize(int size);
        void SetBackgroundColor(Color color);
        void SetPlayer1(PlayerHuman player);
        void SetPlayer2(PlayerAi player);
        void SetPlayer1Fleet(Fleet fleet);
        void SetPlayer2Fleet(Fleet fleet);
        void BuildStandardFleet();
        void BuildAdvancedFleet(Fleet fleet);
        void BuildGame();
        Game GetGame();
    }

    public class GameBuilder : IGameBuilder
    {
        private Player player1;
        private PlayerAi player2;
        public int boardSize;
        private Game game;
        Color backgroundColor = Color.Gray;
        private Difficulty difficulty;

        public void BuildGame()
        {
            game = new Game(player1, player2, boardSize, difficulty);
        }

        public Game GetGame()
        {
            return game;
        }

        public void SetAiStrategy(Difficulty difficulty)
        {
            this.difficulty = difficulty;
        }

        public void SetBackgroundColor(Color color)
        {
            backgroundColor = color;
        }

        public void SetBoardSize(int size)
        {
            this.boardSize = size;
            Board board1 = new Board(boardSize);
            Board board2 = new Board(boardSize);
            player1.SetBoard(board1);
            player2.SetBoard(board2);
        }

        public void SetPlayer1(PlayerHuman player)
        {
            player1 = player;
        }
        public void SetPlayer2(PlayerAi player)
        {
            player2 = player;
        }

        public void BuildStandardFleet()
        {
        }
        public void BuildAdvancedFleet(Fleet fleet)
        {

        }

        public void SetPlayer1Fleet(Fleet fleet)
        {
            player1.SetFleet(fleet);
        }

        public void SetPlayer2Fleet(Fleet fleet)
        {
            player2.SetFleet(fleet);
        }
    }

    public class GameDirector
    {
        private IGameBuilder _builder;

        public GameDirector(IGameBuilder builder)
        {
            _builder = builder;
        }
        public void CreateStandardGame(string playerName)
        {
            PlayerHuman player = new PlayerHuman(playerName);
            PlayerAi player2 = new PlayerAi();
            _builder.SetPlayer1(player);
            _builder.SetPlayer2(player2);
            _builder.SetBoardSize(10);
            Fleet fleet = new Fleet("Player 1 Fleet");
            fleet.Add(new BattleCruiser("Battle cruiser"));
            fleet.Add(new Warship("Warship"));
            fleet.Add(new AircraftCarrier("Aircraft carrier"));
            fleet.Add(new Frigate("Frigate"));
            Fleet fleet2 = new Fleet("Player 2 Fleet");
            fleet2.Add(new BattleCruiser("Battle cruiser"));
            fleet2.Add(new Warship("Warship"));
            fleet2.Add(new AircraftCarrier("Aircraft carrier"));
            fleet2.Add(new Frigate("Frigate"));
            _builder.SetPlayer1Fleet(fleet);
            _builder.SetPlayer2Fleet(fleet2);
            _builder.BuildGame();
        }
    }

    public enum Difficulty
    {
        easy,
        medium,
        hard
    }
}