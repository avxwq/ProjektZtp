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
        private HashSet<Position> hitPositions = new HashSet<Position>();
        private Queue<Position> shotQueue = new Queue<Position>();
        private bool isHunting = false;

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
            if (shotQueue.Count > 0)
            {
                var position = shotQueue.Dequeue();
                if (board.GetCell(position).Ship != null)
                {
                    AddAdjacentPositions(position, board);
                    isHunting = true;
                }

                else if (shotQueue.Count == 0)
                {
                    isHunting = false;
                }
                return position;
            }

            var random = new Random();
            var availablePositions = GetAvailableShotPositions(board);

            if (availablePositions.Count == 0)
                throw new InvalidOperationException("All positions have already been shot at.");

            var randomPosition = availablePositions[random.Next(availablePositions.Count)];
            hitPositions.Add(randomPosition);

            if (board.GetCell(randomPosition).Ship != null)
            {
                AddAdjacentPositions(randomPosition, board);
                isHunting = true;
            }


            return randomPosition;
        }

        private void AddAdjacentPositions(Position hitPosition, Board board)
        {
            var adjacentPositions = new List<Position>
            {
                new Position(hitPosition.X - 1, hitPosition.Y),
                new Position(hitPosition.X + 1, hitPosition.Y),
                new Position(hitPosition.X, hitPosition.Y - 1),
                new Position(hitPosition.X, hitPosition.Y + 1)
            };

            foreach (var adjPos in adjacentPositions)
            {
                if (IsValidPosition(board, adjPos) && !hitPositions.Contains(adjPos))
                {
                    shotQueue.Enqueue(adjPos);
                    hitPositions.Add(adjPos);
                }
            }
        }

        private bool IsValidPosition(Board board, Position pos)
        {
            return pos.X >= 0 && pos.X < board.boardSize && pos.Y >= 0 && pos.Y < board.boardSize;
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
    }

    public class HardAIPlayerStrategy : IAIPlayerStrategy
    {
        private HashSet<Position> hitPositions = new HashSet<Position>();
        private Queue<Position> shotQueue = new Queue<Position>();
        private bool isHunting = false;

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
            if (shotQueue.Count > 0)
            {
                var positions = shotQueue.Dequeue();
                if (board.GetCell(positions).Ship != null)
                {
                    AddAdjacentPositions(positions, board);
                    isHunting = true;
                }

                else if (shotQueue.Count == 0)
                {
                    isHunting = false;
                }
                return positions;
            }

            var availablePositions = GetGridShotPositions(board);

            if (availablePositions.Count == 0)
                throw new InvalidOperationException("All positions have already been shot at.");

            var position = availablePositions.First();
            hitPositions.Add(position);

            if (board.GetCell(position).Ship != null)
            {
                AddAdjacentPositions(position, board);
                isHunting = true;
            }


            return position;
        }

        private void AddAdjacentPositions(Position hitPosition, Board board)
        {
            var adjacentPositions = new List<Position>
            {
                new Position(hitPosition.X - 1, hitPosition.Y),
                new Position(hitPosition.X + 1, hitPosition.Y),
                new Position(hitPosition.X, hitPosition.Y - 1),
                new Position(hitPosition.X, hitPosition.Y + 1)
            };

            foreach (var adjPos in adjacentPositions)
            {
                if (IsValidPosition(board, adjPos) && !hitPositions.Contains(adjPos))
                {
                    shotQueue.Enqueue(adjPos);
                    hitPositions.Add(adjPos);
                }
            }
        }

        private bool IsValidPosition(Board board, Position pos)
        {
            return pos.X >= 0 && pos.X < board.boardSize && pos.Y >= 0 && pos.Y < board.boardSize;
        }

        private List<Position> GetGridShotPositions(Board board)
        {
            var positions = new List<Position>();
            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = x % 2; y < board.boardSize; y += 2)
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
    }





}