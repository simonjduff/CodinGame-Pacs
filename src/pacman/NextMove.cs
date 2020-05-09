namespace pacman
{
    public struct NextMove
    {
        public NextMove(Pac pac, Location location)
        {
            Pac = pac;
            Location = location;
        }
        public Pac Pac { get; }
        public Location Location { get; }
        public int X => Location.X;
        public int Y => Location.Y;
        public int PacId => Pac.Id;
        public override string ToString() => $"MOVE {PacId} {X} {Y}";
        }
}