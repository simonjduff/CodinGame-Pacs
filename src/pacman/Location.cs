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
    }
}