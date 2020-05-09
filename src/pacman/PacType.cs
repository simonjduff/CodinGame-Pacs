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

        public static PacType Rock { get; } = new PacType("ROCK");
        public static PacType Paper { get; } = new PacType("PAPER");
        public static PacType Scissors { get; } = new PacType("SCISSORS");
        public static PacType Neutral { get; } = new PacType("NEUTRAL");

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
                default:
                    throw new ArgumentException($"Unknown pac type {input}", nameof(input));
            }
        }
    }
}