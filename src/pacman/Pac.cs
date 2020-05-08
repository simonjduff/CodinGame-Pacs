namespace pacman
{
    public struct Pac
    {
        public Pac(int id, bool mine, Location location)
        {
            Id = id;
            Mine = mine;
            Location = location;
        }
        public int Id { get; }
        public bool Mine { get; }
        public Location Location { get; }
    }
}