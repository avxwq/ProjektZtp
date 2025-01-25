using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZtp
{
    public interface IAIPlayerStrategy
    {
        Tuple<Position, bool> GetShipPlacement(Board board, Ship ship);
        Position GetShotPosition(Board board);
    }

    public class EasyAIPlayerStrategy : IAIPlayerStrategy
    {
        private HashSet<Position> hitPositions = new HashSet<Position>();

        public Tuple<Position, bool> GetShipPlacement(Board board, Ship ship)
        {
            var random = new Random();
            var availablePositions = GetAllPositions(board.boardSize);

            while (true)
            {
                var startPosition = availablePositions[random.Next(availablePositions.Count)];
                bool isHorizontal = random.Next(2) == 0;

                if (board.CanPlaceShip(ship, startPosition, isHorizontal))
                {
                    return Tuple.Create(startPosition, isHorizontal);
                }
            }
        }

        public Position GetShotPosition(Board board)
        {
            var random = new Random();
            var availablePositions = GetAvailableShotPositions(board);

            if (availablePositions.Count == 0)
            {
                throw new InvalidOperationException("All positions have already been shot at.");
            }

            var position = availablePositions[random.Next(availablePositions.Count)];

            hitPositions.Add(position);

            return position;
        }

        private List<Position> GetAllPositions(int boardSize)
        {
            var positions = new List<Position>();
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    positions.Add(new Position(x, y));
                }
            }
            return positions;
        }

        private List<Position> GetAvailableShotPositions(Board board)
        {
            var positions = new List<Position>();
            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    var position = new Position(x, y);
                    if (!hitPositions.Contains(position))
                    {
                        positions.Add(position);
                    }
                }
            }
            return positions;
        }
    }


    public class MediumAIPlayerStrategy : IAIPlayerStrategy
    {

        public Tuple<Position, bool> GetShipPlacement(Board board, Ship ship)
        {
            throw new NotImplementedException();
        }

        public Position GetShotPosition(Board board)
        {
            throw new NotImplementedException();
        }

    }

    public class HardAIPlayerStrategy : IAIPlayerStrategy
    {

        public Tuple<Position, bool> GetShipPlacement(Board board, Ship ship)
        {
            throw new NotImplementedException();
        }

        public Position GetShotPosition(Board board)
        {
            throw new NotImplementedException();
        }

    }



}