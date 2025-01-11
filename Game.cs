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
        public int BoardSize;
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

        public Player GetPlayer1()
        {
            return player1;
        }
        public Player GetPlayer2()
        {
            return player2;
        }
    }

    public interface IGameBuilder
    {
        void SetGameMode(GameMode gameMode);
        void SetAiStrategy();
        void SetBoardSize(int size);
        void SetBackgroundColor(Color color);
        void SetPlayer1(Player player);
        void SetPlayer2(Player player);
        void SetPlayer1Fleet(Fleet fleet);
        void SetPlayer2Fleet(Fleet fleet);
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

        public void SetPlayer1Fleet(Fleet fleet)
        {
            player1.PlayerFleet = fleet;
        }

        public void SetPlayer2Fleet(Fleet fleet)
        {
            player2.PlayerFleet = fleet;
        }
    }
    public enum GameMode
    {
        vsAi,
        vsHuman
    }
}
