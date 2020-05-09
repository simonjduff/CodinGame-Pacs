namespace pacman
{
    public struct Pellet
    {
        public Pellet(Location location, short value)
        {
            Location = location;
            Value = value;
        }

        public Pellet(short x, short y, short value) : this(new Location(x,y), value)
        {
        }

        public Location Location { get; }
        public short Value { get; }

        public override int GetHashCode()
        {
            var hash = 5701 * Location.GetHashCode();
            hash = hash * 5711 * Value;
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            Pellet pellet = (Pellet) obj;
            return pellet.Location.Equals(Location) && pellet.Value == Value;
        }
    }
}