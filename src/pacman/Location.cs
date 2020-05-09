namespace pacman
{
    public struct Location
    {
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }

        public override int GetHashCode()
        {
            var hash = 50543 * X;
            hash = 50543 * hash * Y;
            return hash;
        }

        public override string ToString() => $"{X} {Y}";
    }
}