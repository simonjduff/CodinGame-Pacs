namespace pacman
{
    public struct Pellet
    {
        public Pellet(Location location, int value)
        {
            Location = location;
            Value = value;
        }
        public Location Location { get; }
        public int Value { get; }
    }
}