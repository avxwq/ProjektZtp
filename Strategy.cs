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
        private HashSet<Position> hitPositions = new HashSet<Position>();  // List of positions that were hit
        private Queue<Position> shotQueue = new Queue<Position>();  // Queue for shots, will shoot in adjacent cells once hit
        private bool isHunting = false;  // Flag to determine if AI is hunting for a ship

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

            // If we are in "hunting" mode (i.e., after hitting a ship), continue shooting around the hit
            if (isHunting && shotQueue.Count > 0)
            {
                var position = shotQueue.Dequeue();

                // If we hit, continue hunting by adding adjacent cells to the queue
                if (board.GetCell(position).IsHit)
                {
                    AddAdjacentPositions(position, board);
                }

                // If the ship is sunk, reset the hunting mode and continue with random shooting
                if (IsShipSunk())
                {
                    isHunting = false;
                }

                return position;
            }

            // If not hunting, shoot randomly
            var availablePositions = GetAvailableShotPositions(board);
            if (availablePositions.Count == 0)
            {
                throw new InvalidOperationException("All positions have already been shot at.");
            }

            var randomPosition = availablePositions[random.Next(availablePositions.Count)];
            hitPositions.Add(randomPosition);
            return randomPosition;
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
                    shotQueue.Enqueue(adjPos);  // Enqueue for next shot
                    hitPositions.Add(adjPos);   // Mark it as hit to avoid repeat shots
                }
            }
        }

        private bool IsValidPosition(Board board, Position pos)
        {
            return pos.X >= 0 && pos.X < board.boardSize && pos.Y >= 0 && pos.Y < board.boardSize;
        }

        private bool IsShipSunk()
        {
            // Check if all cells for the current ship have been hit
            return shotQueue.All(position => hitPositions.Contains(position));
        }
    }



    public class HardAIPlayerStrategy : IAIPlayerStrategy
    {
        private HashSet<Position> hitPositions = new HashSet<Position>();
        private HashSet<Position> shotPositions = new HashSet<Position>();
        private List<Position> lastHitPositions = new List<Position>();

        private Random random = new Random();

        public Tuple<Position, bool> GetShipPlacement(Board board, Ship ship)
        {
            var availablePositions = GetAllPositions(board.boardSize);

            while (true)
            {
                var startPosition = availablePositions[random.Next(availablePositions.Count)];
                bool isHorizontal = random.Next(2) == 0;

                if (board.CanPlaceShip(ship, startPosition, isHorizontal) && !AreShipsAdjacent(board, ship, startPosition, isHorizontal))
                {
                    return Tuple.Create(startPosition, isHorizontal);
                }
            }
        }

        public Position GetShotPosition(Board board)
        {
            // Check if we are currently targeting a ship
            if (lastHitPositions.Count == 0)
            {
                // Continue shooting in grid pattern until we hit a ship
                var position = GetNextGridShotPosition(board);
                shotPositions.Add(position);
                return position;
            }
            else
            {
                // After hitting a ship, start shooting around the last hit position to sink the ship
                var position = GetAdjacentShotPosition(board);
                shotPositions.Add(position);
                return position;
            }
        }

        private Position GetNextGridShotPosition(Board board)
        {
            // Strzelaj w kratkę (co 2 komórki)
            for (int x = 0; x < board.boardSize; x += 2)  // Skip every second row/column for grid
            {
                for (int y = 0; y < board.boardSize; y += 2)
                {
                    var pos = new Position(x, y);
                    if (!shotPositions.Contains(pos))
                    {
                        return pos;
                    }
                }
            }

            return new Position(0, 0); // Fallback to random if grid pattern is exhausted
        }

        private Position GetAdjacentShotPosition(Board board)
        {
            // Similar logic as in Medium AI, shoot adjacent to last hit
            foreach (var hitPos in lastHitPositions)
            {
                var adjacentPositions = new List<Position>
            {
                new Position(hitPos.X - 1, hitPos.Y),
                new Position(hitPos.X + 1, hitPos.Y),
                new Position(hitPos.X, hitPos.Y - 1),
                new Position(hitPos.X, hitPos.Y + 1)
            };

                foreach (var adjPos in adjacentPositions)
                {
                    if (IsValidPosition(board, adjPos) && !shotPositions.Contains(adjPos))
                    {
                        lastHitPositions.Add(adjPos); // Store this as a new target for next round
                        return adjPos;
                    }
                }
            }

            return new Position(0, 0); // Fallback to random if no adjacent shot possible
        }

        private bool IsValidPosition(Board board, Position position)
        {
            return position.X >= 0 && position.X < board.boardSize && position.Y >= 0 && position.Y < board.boardSize;
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

        private bool AreShipsAdjacent(Board board, Ship ship, Position start, bool isHorizontal)
        {
            return false; // Assuming no adjacent ship for now
        }
    }





}