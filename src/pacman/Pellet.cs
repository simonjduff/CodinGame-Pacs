namespace pacman
{
    public struct Pellet
    {
        public Pellet(Location location, short value)
        {
            Location = location;
            Value = value;
        }

        public Pellet(int x, int y, short value) : this(new Location(x,y), value)
        {
        }

        public Location Location { get; }
        public short Value { get; }
    }
}