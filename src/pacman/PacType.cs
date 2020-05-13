namespace pacman
{
    using System;
    public struct PacType
    {
        public PacType(string type)
        {
            _type = type;
        }

        private readonly string _type;

        public override string ToString() => _type;

        public Outcome Play(PacType enemy)
        {
            if (_type == enemy._type)
            {
                return Outcome.Draw;
            }

            if ((_type == "ROCK" && enemy._type == "PAPER")
                || (_type == "PAPER" && enemy._type == "SCISSORS")
                || _type == "SCISSORS" && enemy._type == "ROCK")
            {
                return Outcome.Loss;
            }

            return Outcome.Win;
        }

        public static PacType Rock { get; } = new PacType("ROCK");
        public static PacType Paper { get; } = new PacType("PAPER");
        public static PacType Scissors { get; } = new PacType("SCISSORS");
        public static PacType Neutral { get; } = new PacType("NEUTRAL");
        public static PacType Dead { get; } = new PacType("DEAD");

        public static PacType ToBeat(PacType type)
        {
            switch (type.ToString())
            {
                case "PAPER":
                    return Scissors;
                case "ROCK":
                    return Paper;
                case "SCISSORS":
                    return Rock;
                default:
                    return Rock;

            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is PacType))
            {
                return false;
            }

            return _type.Equals(((PacType) obj)._type);
        }

        public bool Equals(PacType other)
        {
            return _type == other._type;
        }

        public override int GetHashCode()
        {
            return (_type != null ? _type.GetHashCode() : 0);
        }

        public static bool operator ==(PacType left, PacType right) => left.Equals(right);

        public static bool operator !=(PacType left, PacType right) => !(left == right);

        public static implicit operator PacType(string input)
        {
            switch (input)
            {
                case "ROCK":
                    return Rock;
                case "SCISSORS":
                    return Scissors;
                case "PAPER":
                    return Paper;
                case "NEUTRAL":
                    return Neutral;
                case "DEAD":
                    return Dead;
                default:
                    throw new ArgumentException($"Unknown pac type {input}", nameof(input));
            }
        }

        public enum Outcome
        {
            Win = 'W',
            Loss = 'L',
            Draw = 'D'
        }
    }
}