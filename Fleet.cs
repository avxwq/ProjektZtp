using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjektZtp
{
    public abstract class FleetComponent
    {
        public abstract string Name { get; }
        public abstract int Size { get; }

        public virtual void Add(FleetComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(FleetComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual List<FleetComponent> GetComponents()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Ship : FleetComponent
    {
        public override string Name { get; }
        public override abstract int Size { get; }

        protected Ship(string name)
        {
            Name = name;
        }
    }

    public class BattleCruiser : Ship
    {
        public override int Size => 5;

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
        public override int Size => 8;

        public AircraftCarrier(string name) : base(name)
        {
        }
    }

    public class Fleet : FleetComponent
    {
        private readonly List<FleetComponent> _components = new List<FleetComponent>();

        public override string Name { get; }

        public Fleet(string name)
        {
            Name = name;
        }

        public override int Size => _components.Sum(component => component.Size);

        public override void Add(FleetComponent component)
        {
            _components.Add(component);
        }

        public override void Remove(FleetComponent component)
        {
            _components.Remove(component);
        }

        public override List<FleetComponent> GetComponents()
        {
            return _components;
        }
    }
}
