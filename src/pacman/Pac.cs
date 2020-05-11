namespace pacman
{
    using System.Threading;
    using ActionStrategies;
    using System.Collections.Generic;
    using System;
    public class Pac : IEquatable<Pac>
    {
        public Pac(int id, 
            bool mine, 
            IActionStrategy strategy,
            GiveWayMovementStrategy giveWayMovementStrategy
            )
        {
            _giveWayMovementStrategy = giveWayMovementStrategy;
            Id = id;
            Mine = mine;
            LocationHistory  = new List<Location>(50);
            SpeedTurnsLeft = 0;
            AbilityCooldown = 0;
            Key = new PacKey(Id, mine);
            CurrentStrategy = strategy;
        }

        public IActionStrategy CurrentStrategy { get; set; }

        public NextAction NextAction(GameGrid grid, CancellationToken cancellation)
        {
            var nextAction = CurrentStrategy.Next(this, cancellation);
            if (nextAction is MoveAction moveAction)
            {
                LastMoveAction = moveAction;
            }

            return nextAction;
        }

        public MoveAction LastMoveAction { get; private set; }

        public PacKey Key { get; }
        public int Id { get; }
        public bool Mine { get; }
        private PacType _pacType = "NEUTRAL";

        public PacType Type
        {
            get => _pacType;
            set
            {
                Console.Error.WriteLine($"{(Mine ? "My" : "Enemy")} pac {Id} has been told it is {value}");
                _pacType = value;
            }
        }

        public Location Location => LocationHistory[^1]; // ^1 is a System.Index indicating the last value
        public short SpeedTurnsLeft { get; set; }
        public short AbilityCooldown { get; set; }
        public bool SpecialActionReady => AbilityCooldown == 0;
        public bool Equals(Pac other)
            => Id == other?.Id && Location == other?.Location;

        public override int GetHashCode() => new PacKey(Id, Mine).GetHashCode() * 19;
        public List<Location> LocationHistory;
        private readonly GiveWayMovementStrategy _giveWayMovementStrategy;

        public void AddLocation(Location location)
        {
            LocationHistory.Add(location);
        }

        public NextAction GiveWay(CancellationToken cancellation) =>
            _giveWayMovementStrategy.Next(this, cancellation);
    }

    public struct PacKey
    {
        public PacKey(int pacId, bool mine)
        {
            var hash = 10061;
            hash = hash * 11 + pacId;
            hash = hash * 11 + (mine ? 7 : 11);
            Key = hash;
        }
        public int Key { get; }
        public override int GetHashCode() => Key;
    }
}