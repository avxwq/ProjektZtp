using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjektZtp
{
    public abstract class FleetComponent
    {
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
 
        public override abstract int Size { get; }


    }

    public class BattleCruiser : Ship
    {
        public override int Size => 5;


    }

    public class Frigate : Ship
    {
        public override int Size => 3;

    
    }

    public class Warship : Ship
    {
        public override int Size => 4;


    }

    public class AircraftCarrier : Ship
    {
        public override int Size => 8;

    }

    public class Fleet : FleetComponent
    {
        private readonly List<FleetComponent> _components = new List<FleetComponent>();


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
