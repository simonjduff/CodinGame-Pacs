using Microsoft.VisualBasic.CompilerServices;

namespace pacman
{
    public struct Location
    {
        public Location(short x, short y)
        {
            X = x;
            Y = y;
        }
        public short X { get; }
        public short Y { get; }

        public override int GetHashCode()
        {
            var hash = 50543 * X;
            hash = 50543 * hash * Y;
            return hash;
        }

        public override string ToString() => $"{X} {Y}";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            Location loc = (Location) obj;
            return this == loc;
        }

        public static bool operator ==(Location left, Location right) => left.X == right.X && left.Y == right.Y;

        public static bool operator !=(Location left, Location right)
        {
            return !(left == right);
        }
    }
}