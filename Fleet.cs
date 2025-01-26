using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjektZtp
{
    public interface FleetComponent
    {
        string Name { get; }
        int Size { get; }
        bool isSunk();
    }

    public abstract class Ship : FleetComponent
    {
        public string Name { get; }
        public abstract int Size { get; }
        public List<Cell> Cells { get; } = new List<Cell>();

        protected Ship(string name)
        {
            Name = name;
        }
        public void AddCells(Board board, Position start, bool isHorizontal)
        {
            for (int i = 0; i < Size; i++)
            {
                int x = isHorizontal ? start.X : start.X + i;
                int y = isHorizontal ? start.Y + i : start.Y;

                var cell = board.GetCell(new Position(x, y));

                Cells.Add(cell);
            }
        }

        public bool isSunk()
        {
            return Cells.All(cell => cell.IsHit);
        }
    }


    public class BattleCruiser : Ship
    {
        public override int Size => 2;

        public BattleCruiser(string name) : base(name)
        {
        }
    }

    public class Frigate : Ship
    {
        public override int Size => 3;

        public Frigate(string name) : base(name)
        {
        }
    }

    public class Warship : Ship
    {
        public override int Size => 4;

        public Warship(string name) : base(name)
        {
        }
    }

    public class AircraftCarrier : Ship
    {
        public override int Size => 6;

        public AircraftCarrier(string name) : base(name)
        {
        }
    }

    public class Fleet : FleetComponent
    {
        public readonly List<FleetComponent> _components = new List<FleetComponent>();

        public string Name { get; }

        public bool isSunk()
        {
            return _components.All(component => component.isSunk());
        }

        public Fleet(string name)
        {
            Name = name;
        }

        public int Size => _components.Sum(component => component.Size);

        public void Add(FleetComponent component)
        {
            _components.Add(component);
        }

        public void Remove(FleetComponent component)
        {
            _components.Remove(component);
        }

        public List<FleetComponent> GetComponents()
        {
            return _components;
        }
    }

    public class FleetTemplate
    {
        private readonly List<Ship> _ships;

        public FleetTemplate()
        {
            _ships = new List<Ship>();
        }

        public FleetTemplate AddBattleCruiser(int count = 2)
        {
            for (int i = 0; i < count; i++)
            {
                _ships.Add(new BattleCruiser($"Battle Cruiser {count - i}"));
            }
            return this;
        }

        public FleetTemplate AddFrigate(int count = 2)
        {
            for (int i = 0; i < count; i++)
            {
                _ships.Add(new Frigate($"Frigate {count - i}"));
            }
            return this;
        }

        public FleetTemplate AddWarship(int count = 2)
        {
            for (int i = 0; i < count; i++)
            {
                _ships.Add(new Warship($"Warship {count - i}"));
            }
            return this;
        }

        public FleetTemplate AddAircraftCarrier(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                _ships.Add(new AircraftCarrier($"Aircraft Carrier {count - i}"));
            }
            return this;
        }



        public Fleet Build(string fleetName)
        {
            Fleet fleet = new Fleet(fleetName);
            foreach (var ship in _ships)
            {
                fleet.Add(ship);
            }
            return fleet;
        }
    }
}