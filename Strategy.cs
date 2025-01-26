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
        private List<Position> initialHitPositions = new List<Position>();
        private bool isHorizontal = false;
        private ShipHuntingState huntingState = ShipHuntingState.NotHunting;

        private enum ShipHuntingState
        {
            NotHunting,
            HuntingRight,
            HuntingLeft,
            HuntingUp,
            HuntingDown
        }

        public Tuple<Position, bool> GetShipPlacement(Board board, Ship ship)
        {
            var random = new Random();
            var availablePositions = GetAllPositions(board.boardSize);

            while (true)
            {
                var startPosition = availablePositions[random.Next(availablePositions.Count)];
                bool shipHorizontal = random.Next(2) == 0;

                if (board.CanPlaceShip(ship, startPosition, shipHorizontal))
                {
                    return Tuple.Create(startPosition, shipHorizontal);
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
                    initialHitPositions.Add(position);
                    HandleHitShip(position, board);
                }
                else if (shotQueue.Count == 0)
                {
                    UpdateHuntingStateAfterMiss();
                }
                return position;
            }

            var random = new Random();
            var availablePositions = GetAvailableShotPositions(board);

            if (availablePositions.Count == 0)
                throw new InvalidOperationException("All positions have already been shot at.");

            var selectedPosition = availablePositions[random.Next(availablePositions.Count)];
            hitPositions.Add(selectedPosition);

            if (board.GetCell(selectedPosition).Ship != null)
            {
                initialHitPositions.Add(selectedPosition);
                HandleHitShip(selectedPosition, board);
            }

            return selectedPosition;
        }

        private void HandleHitShip(Position hitPosition, Board board)
        {
            if (initialHitPositions.Count == 2)
            {
                DetermineShipOrientation();
                InitializeHuntingSequence(board);
            }
            else if (huntingState != ShipHuntingState.NotHunting)
            {
                ContinueHuntingSequence(board);
            }
            else
            {
                AddAdjacentPositions(hitPosition, board);
            }
        }

        private void DetermineShipOrientation()
        {
            isHorizontal = initialHitPositions[0].X == initialHitPositions[1].X;
        }

        private void InitializeHuntingSequence(Board board)
        {
            if (isHorizontal)
            {
                huntingState = initialHitPositions[0].Y < initialHitPositions[1].Y
                    ? ShipHuntingState.HuntingDown
                    : ShipHuntingState.HuntingUp;
            }
            else
            {
                huntingState = initialHitPositions[0].X < initialHitPositions[1].X
                    ? ShipHuntingState.HuntingRight
                    : ShipHuntingState.HuntingLeft;
            }
            AddDirectionalPositions(board);
        }

        private void ContinueHuntingSequence(Board board)
        {
            switch (huntingState)
            {
                case ShipHuntingState.HuntingRight:
                    TryHuntInDirection(board, new Position(1, 0), ShipHuntingState.HuntingLeft);
                    break;
                case ShipHuntingState.HuntingLeft:
                    TryHuntInDirection(board, new Position(-1, 0), ShipHuntingState.NotHunting);
                    break;
                case ShipHuntingState.HuntingUp:
                    TryHuntInDirection(board, new Position(0, -1), ShipHuntingState.HuntingDown);
                    break;
                case ShipHuntingState.HuntingDown:
                    TryHuntInDirection(board, new Position(0, 1), ShipHuntingState.NotHunting);
                    break;
            }
        }

        private void TryHuntInDirection(Board board, Position direction, ShipHuntingState nextState)
        {
            var lastHit = initialHitPositions[initialHitPositions.Count - 1];
            var nextPosition = new Position(lastHit.X + direction.X, lastHit.Y + direction.Y);

            if (IsValidPosition(board, nextPosition) && !hitPositions.Contains(nextPosition))
            {
                shotQueue.Enqueue(nextPosition);
                hitPositions.Add(nextPosition);
            }
            else
            {
                huntingState = nextState;
            }
        }

        private void UpdateHuntingStateAfterMiss()
        {
            switch (huntingState)
            {
                case ShipHuntingState.HuntingRight:
                    huntingState = ShipHuntingState.HuntingLeft;
                    break;
                case ShipHuntingState.HuntingLeft:
                    huntingState = isHorizontal ? ShipHuntingState.NotHunting : ShipHuntingState.HuntingDown;
                    break;
                case ShipHuntingState.HuntingUp:
                    huntingState = ShipHuntingState.HuntingDown;
                    break;
                case ShipHuntingState.HuntingDown:
                    huntingState = !isHorizontal ? ShipHuntingState.NotHunting : ShipHuntingState.NotHunting;
                    break;
            }

            if (huntingState == ShipHuntingState.NotHunting)
            {
                initialHitPositions.Clear();
            }
        }

        private void AddDirectionalPositions(Board board)
        {
            var firstHit = initialHitPositions[0];
            var secondHit = initialHitPositions[1];
            var directions = GetSortedDirections(firstHit, secondHit);

            foreach (var direction in directions)
            {
                if (IsValidPosition(board, direction) && !hitPositions.Contains(direction))
                {
                    shotQueue.Enqueue(direction);
                    hitPositions.Add(direction);
                }
            }
        }

        private List<Position> GetSortedDirections(Position firstHit, Position secondHit)
        {
            if (isHorizontal)
            {
                return firstHit.Y < secondHit.Y
                    ? new List<Position> { new Position(firstHit.X, firstHit.Y - 1), new Position(firstHit.X, secondHit.Y + 1) }
                    : new List<Position> { new Position(firstHit.X, firstHit.Y + 1), new Position(firstHit.X, secondHit.Y - 1) };
            }
            else
            {
                return firstHit.X < secondHit.X
                    ? new List<Position> { new Position(firstHit.X - 1, firstHit.Y), new Position(secondHit.X + 1, firstHit.Y) }
                    : new List<Position> { new Position(firstHit.X + 1, firstHit.Y), new Position(secondHit.X - 1, firstHit.Y) };
            }
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
        private List<Position> initialHitPositions = new List<Position>();
        private bool isHorizontal = false;
        private ShipHuntingState huntingState = ShipHuntingState.NotHunting;

        private enum ShipHuntingState
        {
            NotHunting,
            HuntingRight,
            HuntingLeft,
            HuntingUp,
            HuntingDown
        }

        public Tuple<Position, bool> GetShipPlacement(Board board, Ship ship)
        {
            var random = new Random();
            var availablePositions = GetAllPositions(board.boardSize);

            while (true)
            {
                var startPosition = availablePositions[random.Next(availablePositions.Count)];
                bool shipHorizontal = random.Next(2) == 0;

                if (board.CanPlaceShip(ship, startPosition, shipHorizontal))
                {
                    return Tuple.Create(startPosition, shipHorizontal);
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
                    initialHitPositions.Add(position);
                    HandleHitShip(position, board);
                }
                else if (shotQueue.Count == 0)
                {
                    UpdateHuntingStateAfterMiss();
                }
                return position;
            }

            var availablePositions = GetGridShotPositions(board);

            if (availablePositions.Count == 0)
                throw new InvalidOperationException("All positions have already been shot at.");

            var selectedPosition = availablePositions.First();
            hitPositions.Add(selectedPosition);

            if (board.GetCell(selectedPosition).Ship != null)
            {
                initialHitPositions.Add(selectedPosition);
                HandleHitShip(selectedPosition, board);
            }

            return selectedPosition;
        }

        private void HandleHitShip(Position hitPosition, Board board)
        {
            if (initialHitPositions.Count == 2)
            {
                DetermineShipOrientation();
                InitializeHuntingSequence(board);
            }
            else if (huntingState != ShipHuntingState.NotHunting)
            {
                ContinueHuntingSequence(board);
            }
            else
            {
                AddAdjacentPositions(hitPosition, board);
            }
        }

        private void DetermineShipOrientation()
        {
            isHorizontal = initialHitPositions[0].X == initialHitPositions[1].X;
        }

        private void InitializeHuntingSequence(Board board)
        {
            if (isHorizontal)
            {
                huntingState = initialHitPositions[0].Y < initialHitPositions[1].Y
                    ? ShipHuntingState.HuntingDown
                    : ShipHuntingState.HuntingUp;
            }
            else
            {
                huntingState = initialHitPositions[0].X < initialHitPositions[1].X
                    ? ShipHuntingState.HuntingRight
                    : ShipHuntingState.HuntingLeft;
            }
            AddDirectionalPositions(board);
        }

        private void ContinueHuntingSequence(Board board)
        {
            switch (huntingState)
            {
                case ShipHuntingState.HuntingRight:
                    TryHuntInDirection(board, new Position(1, 0), ShipHuntingState.HuntingLeft);
                    break;
                case ShipHuntingState.HuntingLeft:
                    TryHuntInDirection(board, new Position(-1, 0), ShipHuntingState.NotHunting);
                    break;
                case ShipHuntingState.HuntingUp:
                    TryHuntInDirection(board, new Position(0, -1), ShipHuntingState.HuntingDown);
                    break;
                case ShipHuntingState.HuntingDown:
                    TryHuntInDirection(board, new Position(0, 1), ShipHuntingState.NotHunting);
                    break;
            }
        }

        private void TryHuntInDirection(Board board, Position direction, ShipHuntingState nextState)
        {
            var lastHit = initialHitPositions[initialHitPositions.Count - 1];
            var nextPosition = new Position(lastHit.X + direction.X, lastHit.Y + direction.Y);

            if (IsValidPosition(board, nextPosition) && !hitPositions.Contains(nextPosition))
            {
                shotQueue.Enqueue(nextPosition);
                hitPositions.Add(nextPosition);
            }
            else
            {
                huntingState = nextState;
            }
        }

        private void UpdateHuntingStateAfterMiss()
        {
            if (isHorizontal)
            {
                huntingState = huntingState == ShipHuntingState.HuntingDown
                    ? ShipHuntingState.HuntingUp
                    : ShipHuntingState.NotHunting;
            }
            else
            {
                huntingState = huntingState == ShipHuntingState.HuntingRight
                    ? ShipHuntingState.HuntingLeft
                    : ShipHuntingState.NotHunting;
            }
        }

        private void AddDirectionalPositions(Board board)
        {
            var firstHit = initialHitPositions[0];
            var secondHit = initialHitPositions[1];
            var directions = GetSortedDirections(firstHit, secondHit);

            foreach (var direction in directions)
            {
                if (IsValidPosition(board, direction) && !hitPositions.Contains(direction))
                {
                    shotQueue.Enqueue(direction);
                    hitPositions.Add(direction);
                }
            }
        }

        private List<Position> GetSortedDirections(Position firstHit, Position secondHit)
        {
            if (isHorizontal)
            {
                return firstHit.Y < secondHit.Y
                    ? new List<Position> { new Position(firstHit.X, firstHit.Y - 1), new Position(firstHit.X, secondHit.Y + 1) }
                    : new List<Position> { new Position(firstHit.X, firstHit.Y + 1), new Position(firstHit.X, secondHit.Y - 1) };
            }
            else
            {
                return firstHit.X < secondHit.X
                    ? new List<Position> { new Position(firstHit.X - 1, firstHit.Y), new Position(secondHit.X + 1, firstHit.Y) }
                    : new List<Position> { new Position(firstHit.X + 1, firstHit.Y), new Position(secondHit.X - 1, firstHit.Y) };
            }
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