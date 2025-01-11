using System;
using System.Collections.Generic;
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
    }

    public abstract class Player
    {

    }
    public class Ship
    {
        public string Name { get; }
        public int Size { get; }

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
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
        void SetBackgroundColor(string color);
        void SetPlayer1(Player player);
        void SetPlayer2(Player player);
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

        public void SetBackgroundColor(string color)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void SetPlayer2(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
