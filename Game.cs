using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZtp
{
    public class Game
    {
    }

    public abstract class Player
    {

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
        void GetGame();
    }

    public class GameBuilder : IGameBuilder
    {
        public void GetGame()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void SetGameMode(GameMode gameMode)
        {
            throw new NotImplementedException();
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
