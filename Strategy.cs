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
            var availablePositions = GetAllPlacePositions(board.boardSize);

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

        private List<Position> GetAllPlacePositions(int boardSize)
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
        private Queue<Position> targetQueue = new Queue<Position>();
        private Queue<Position> forwardQueue = new Queue<Position>();
        private Queue<Position> backwardQueue = new Queue<Position>();
        private HashSet<Position> shotPositions = new HashSet<Position>();
        private Position? lastShot = null;
        private List<Position> currentHits = new List<Position>();
        private bool isShootingInLine = false;
        private bool isForward;

        public Tuple<Position, bool> GetShipPlacement(Board board, Ship ship)
        {
            var random = new Random();
            var availablePositions = GetAllPlacePositions(board.boardSize);

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

        private List<Position> GetAllPlacePositions(int boardSize)
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

        public Position GetShotPosition(Board board)
        {
            // Analyze the result of the last shot
            if (lastShot != null)
            {
                var cell = board.GetCell(lastShot.Value);
                if (cell.IsHit && cell.Ship != null) // Hit a ship
                {
                    currentHits.Add(lastShot.Value);
                    if (!isShootingInLine) ProcessHit(lastShot.Value, board);
                    if (cell.Ship.isSunk()) // Check if the ship is sunk
                    {
                        // Clear the target queue and hits if the ship is sunk
                        targetQueue.Clear();
                        currentHits.Clear();
                        isShootingInLine = false;
                    }
                }
                else if (isShootingInLine)
                {
                    SwitchShootingDirection(); 
                }
            }

            // If shooting in line, take from targetQueue
            if (targetQueue.Count > 0)
            {
                var nextTarget = targetQueue.Dequeue();
                if (IsValidShot(nextTarget, board))
                {
                    shotPositions.Add(nextTarget);
                    lastShot = nextTarget;
                    return nextTarget;
                }
            }

            // Hunt Mode: fire at random optimized positions
            var huntPosition = GetRandomHuntPosition(board);
            shotPositions.Add(huntPosition);
            lastShot = huntPosition;
            return huntPosition;
        }

        private void ProcessHit(Position position, Board board)
        {
            // If multiple hits are detected, prioritize along the axis
            if (currentHits.Count > 1)
            {
                var direction = DetermineDirection();
                EnqueuePositionsInLine(position, board, direction);
                isShootingInLine = true;
            }
            else
            {
                // If only one hit, enqueue all adjacent positions
                EnqueueAdjacentPositions(position, board);
            }
        }

        private void EnqueueAdjacentPositions(Position position, Board board)
        {
            var directions = new List<Position>
            {
                new Position(position.X + 1, position.Y),
                new Position(position.X - 1, position.Y),
                new Position(position.X, position.Y + 1),
                new Position(position.X, position.Y - 1)
            };

            foreach (var adjacent in directions)
            {
                if (IsWithinBounds(adjacent, board.boardSize) && IsValidShot(adjacent, board))
                {
                    targetQueue.Enqueue(adjacent);
                }
            }
        }

        private void EnqueuePositionsInLine(Position position, Board board, (int dx, int dy) direction)
        {
            forwardQueue.Clear();
            backwardQueue.Clear();

            // Determine the start and end positions based on all hits
            var sortedHits = currentHits.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            var startPosition = sortedHits.First();
            var endPosition = sortedHits.Last();

            // Enqueue in the forward direction (from the end position)
            int step = 1;
            while (true)
            {
                var nextPos = new Position(endPosition.X + step * direction.dx, endPosition.Y + step * direction.dy);
                if (IsWithinBounds(nextPos, board.boardSize) && IsValidShot(nextPos, board))
                {
                    forwardQueue.Enqueue(nextPos);
                }
                else break; // Stop if out of bounds or invalid
                step++;
            }

            // Enqueue in the backward direction (from the start position)
            step = 1;
            while (true)
            {
                var prevPos = new Position(startPosition.X - step * direction.dx, startPosition.Y - step * direction.dy);
                if (IsWithinBounds(prevPos, board.boardSize) && IsValidShot(prevPos, board))
                {
                    backwardQueue.Enqueue(prevPos);
                }
                else break; // Stop if out of bounds or invalid
                step++;
            }

            // Initially set target queue to forward direction
            if (new Random().Next(2) == 0)
            {
                targetQueue = forwardQueue.Any() ? forwardQueue : backwardQueue; // Choose forward if not empty, else backward
                isForward = targetQueue == forwardQueue;
            }
            else
            {
                targetQueue = backwardQueue.Any() ? backwardQueue : forwardQueue; // Choose backward if not empty, else forward
                isForward = targetQueue == forwardQueue;
            }
        }

        private (int dx, int dy) DetermineDirection()
        {
            var first = currentHits.First();
            var second = currentHits.ElementAt(1);

            if (first.X == second.X) return (0, 1); // Vertical
            return (1, 0); // Horizontal
        }

        private void SwitchShootingDirection()
        {
            if (isForward)
            {
                targetQueue = backwardQueue;
                isForward = false;
            }
            else
            {
                targetQueue = forwardQueue;
                isForward = true;
            }
        }

        private bool IsValidShot(Position position, Board board)
        {
            // A valid shot is on a cell that hasn't already been hit
            var cell = board.GetCell(position);
            return !shotPositions.Contains(position) && !cell.IsHit;
        }

        private bool IsWithinBounds(Position position, int boardSize)
        {
            return position.X >= 0 && position.X < boardSize && position.Y >= 0 && position.Y < boardSize;
        }

        private Position GetRandomHuntPosition(Board board)
        {
            // Generate positions in a checkerboard pattern
            var potentialPositions = new List<Position>();
            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    if (IsValidShot(new Position(x, y), board))
                    {
                        potentialPositions.Add(new Position(x, y));
                    }
                }
            }

            var random = new Random();
            return potentialPositions[random.Next(potentialPositions.Count)];
        }

    }



    public class HardAIPlayerStrategy : IAIPlayerStrategy
    {
        private Queue<Position> targetQueue = new Queue<Position>();
        private Queue<Position> forwardQueue = new Queue<Position>();
        private Queue<Position> backwardQueue = new Queue<Position>();
        private HashSet<Position> shotPositions = new HashSet<Position>();
        private Position? lastShot = null;
        private List<Position> currentHits = new List<Position>();
        private bool isShootingInLine = false;
        private bool isForward;

        public Tuple<Position, bool> GetShipPlacement(Board board, Ship ship)
        {
            var random = new Random();
            var availablePositions = GetAllPlacePositions(board.boardSize);

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

        private List<Position> GetAllPlacePositions(int boardSize)
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

        public Position GetShotPosition(Board board)
        {
            // Analyze the result of the last shot
            if (lastShot != null)
            {
                var cell = board.GetCell(lastShot.Value);
                if (cell.IsHit && cell.Ship != null) // Hit a ship
                {
                    currentHits.Add(lastShot.Value);
                    if (!isShootingInLine) ProcessHit(lastShot.Value, board);
                    if (cell.Ship.isSunk()) // Check if the ship is sunk
                    {
                        // Clear the target queue and hits if the ship is sunk
                        targetQueue.Clear();
                        currentHits.Clear();
                        isShootingInLine = false;
                    }
                }
                else if (isShootingInLine)
                {
                    SwitchShootingDirection();
                }
            }

            // If shooting in line, take from targetQueue
            if (targetQueue.Count > 0)
            {
                var nextTarget = targetQueue.Dequeue();
                if (IsValidShot(nextTarget, board))
                {
                    shotPositions.Add(nextTarget);
                    lastShot = nextTarget;
                    return nextTarget;
                }
            }

            // Hunt Mode: fire at random optimized positions
            var huntPosition = GetRandomHuntPosition(board);
            shotPositions.Add(huntPosition);
            lastShot = huntPosition;
            return huntPosition;
        }

        private void ProcessHit(Position position, Board board)
        {
            // If multiple hits are detected, prioritize along the axis
            if (currentHits.Count > 1)
            {
                var direction = DetermineDirection();
                EnqueuePositionsInLine(position, board, direction);
                isShootingInLine = true;
            }
            else
            {
                // If only one hit, enqueue all adjacent positions
                EnqueueAdjacentPositions(position, board);
            }
        }

        private void EnqueueAdjacentPositions(Position position, Board board)
        {
            var directions = new List<Position>
            {
                new Position(position.X + 1, position.Y),
                new Position(position.X - 1, position.Y),
                new Position(position.X, position.Y + 1),
                new Position(position.X, position.Y - 1)
            };

            foreach (var adjacent in directions)
            {
                if (IsWithinBounds(adjacent, board.boardSize) && IsValidShot(adjacent, board))
                {
                    targetQueue.Enqueue(adjacent);
                }
            }
        }

        private void EnqueuePositionsInLine(Position position, Board board, (int dx, int dy) direction)
        {
            forwardQueue.Clear();
            backwardQueue.Clear();

            // Determine the start and end positions based on all hits
            var sortedHits = currentHits.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            var startPosition = sortedHits.First();
            var endPosition = sortedHits.Last();

            // Enqueue in the forward direction (from the end position)
            int step = 1;
            while (true)
            {
                var nextPos = new Position(endPosition.X + step * direction.dx, endPosition.Y + step * direction.dy);
                if (IsWithinBounds(nextPos, board.boardSize) && IsValidShot(nextPos, board))
                {
                    forwardQueue.Enqueue(nextPos);
                }
                else break; // Stop if out of bounds or invalid
                step++;
            }

            // Enqueue in the backward direction (from the start position)
            step = 1;
            while (true)
            {
                var prevPos = new Position(startPosition.X - step * direction.dx, startPosition.Y - step * direction.dy);
                if (IsWithinBounds(prevPos, board.boardSize) && IsValidShot(prevPos, board))
                {
                    backwardQueue.Enqueue(prevPos);
                }
                else break; // Stop if out of bounds or invalid
                step++;
            }

            // Initially set target queue to forward direction
            if (new Random().Next(2) == 0)
            {
                targetQueue = forwardQueue.Any() ? forwardQueue : backwardQueue; // Choose forward if not empty, else backward
                isForward = targetQueue == forwardQueue;
            }
            else
            {
                targetQueue = backwardQueue.Any() ? backwardQueue : forwardQueue; // Choose backward if not empty, else forward
                isForward = targetQueue == forwardQueue;
            }
        }

        private (int dx, int dy) DetermineDirection()
        {
            var first = currentHits.First();
            var second = currentHits.ElementAt(1);

            if (first.X == second.X) return (0, 1); // Vertical
            return (1, 0); // Horizontal
        }

        private void SwitchShootingDirection()
        {
            if (isForward)
            {
                targetQueue = backwardQueue;
                isForward = false;
            }
            else
            {
                targetQueue = forwardQueue;
                isForward = true;
            }
        }

        private bool IsValidShot(Position position, Board board)
        {
            // A valid shot is on a cell that hasn't already been hit
            var cell = board.GetCell(position);
            return !shotPositions.Contains(position) && !cell.IsHit;
        }

        private bool IsWithinBounds(Position position, int boardSize)
        {
            return position.X >= 0 && position.X < boardSize && position.Y >= 0 && position.Y < boardSize;
        }

        private Position GetRandomHuntPosition(Board board)
        {
            // Generate positions in a checkerboard pattern
            var potentialPositions = new List<Position>();
            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    if ((x + y) % 2 == 0 && IsValidShot(new Position(x, y), board))
                    {
                        potentialPositions.Add(new Position(x, y));
                    }
                }
            }

            var random = new Random();
            return potentialPositions[random.Next(potentialPositions.Count)];
        }
    }

}