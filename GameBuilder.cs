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
        private BattleshipGameForm gameForm;

        public void BuildGame()
        {
            game = new Game(player1, player2, boardSize, difficulty, gameForm);
        }

        public Game GetGame()
        {
            return game;
        }

        public void SetGameForm(BattleshipGameForm gameForm)
        {
            this.gameForm = gameForm;
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
            Fleet fleet1 = new FleetTemplate()
                .AddBattleCruiser()
                .AddWarship()
                .AddAircraftCarrier()
                .AddFrigate()
                .Build("Player 1 Fleet");

            Fleet fleet2 = new FleetTemplate()
                .AddBattleCruiser()
                .AddWarship()
                .AddAircraftCarrier()
                .AddFrigate()
                .Build("Player 2 Fleet");
            _builder.SetPlayer1Fleet(fleet1);
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