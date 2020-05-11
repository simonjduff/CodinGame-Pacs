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
            MayHavePellet = true;
            PossiblePelletValue = 1;
        }

        public bool Traversable { get; }
        public Location Location { get; }
        public bool MayHavePellet { get; set; }
        public short PossiblePelletValue { get; set; }
    }
}