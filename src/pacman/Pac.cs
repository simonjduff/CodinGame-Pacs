using System.Collections.Generic;

namespace pacman
{
    public struct Pac
    {
        public Pac(int id, bool mine)
        {
            Id = id;
            Mine = mine;
            LocationHistory  = new List<Location>(50);
        }
        public int Id { get; }
        public bool Mine { get; }
        public Location Location => LocationHistory[^1]; // ^1 is a System.Index indicating the last value
        public override int GetHashCode() => Id * 10061;
        public List<Location> LocationHistory;

        public void AddLocation(Location location)
        {
            LocationHistory.Add(location);
        }
    }
}