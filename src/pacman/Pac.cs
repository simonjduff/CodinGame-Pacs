﻿namespace pacman
{
    using System.Collections.Generic;
    public class Pac
    {
        public Pac(int id, 
            bool mine, 
            PacType type)
        {
            Id = id;
            Mine = mine;
            LocationHistory  = new List<Location>(50);
            Type = type;
            SpeedTurnsLeft = 0;
            AbilityCooldown = 0;
            Key = new PacKey(Id, mine);
        }

        public Pac(int id,
            bool mine) : this(id, mine, PacType.Neutral)
        {
        }

        public PacKey Key { get; }
        public int Id { get; }
        public bool Mine { get; }
        public PacType Type { get; private set; }
        public Location Location => LocationHistory[^1]; // ^1 is a System.Index indicating the last value
        public short SpeedTurnsLeft { get; set; }
        public short AbilityCooldown { get; set; }
        public override int GetHashCode() => new PacKey(Id, Mine).GetHashCode() * 19;
        public List<Location> LocationHistory;

        public void AddLocation(Location location)
        {
            LocationHistory.Add(location);
        }
    }

    public struct PacKey
    {
        public PacKey(int pacId, bool mine)
        {
            Key = pacId * 10061 * (mine ? 7 : 11);
        }
        public int Key { get; }
        public override int GetHashCode() => Key;
    }
}