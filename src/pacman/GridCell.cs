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
            PossiblePelletValue = 1;
        }

        public bool Traversable { get; }
        public Location Location { get; }
        public short PossiblePelletValue { get; set; }
    }
}