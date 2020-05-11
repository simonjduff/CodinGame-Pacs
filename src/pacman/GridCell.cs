namespace pacman
{
    public class GridCell
    {
        public GridCell(bool traversable,
            Location location
        )
        {
            Traversable = traversable;
            Location = location;
        }

        public bool Traversable { get; }
        public Location Location { get; }
    }
}