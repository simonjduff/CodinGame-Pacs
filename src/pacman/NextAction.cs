namespace pacman
{
    public abstract class NextAction
    {
        protected NextAction(Pac pac)
        {
            Pac = pac;
        }
        public Pac Pac { get; }
        public int PacId => Pac.Id;
        public abstract string ActionString { get; }
        public sealed override string ToString() => ActionString;
    }

    public class NoAction : NextAction
    {
        public NoAction(Pac pac) : base(pac)
        {
        }

        public override string ActionString => string.Empty;
    }

    public class SpeedAction : NextAction
    {
        public SpeedAction(Pac pac):base(pac)
        {
            
        }

        public override string ActionString => $"SPEED {Pac.Id}";
    }

    public class SwitchAction : NextAction
    {
        public SwitchAction(Pac pac, PacType newType):base(pac)
        {
            NewType = newType;
        }

        public PacType NewType { get; }

        public override string ActionString => $"SWITCH {Pac.Id} {NewType}";
    }

    public class MoveAction : NextAction
    {
        public MoveAction(Pac pac, Location location) : base(pac)
        {
            Location = location;
        }

        public Location Location { get; }
        public override string ActionString => $"MOVE {PacId} {Location.X} {Location.Y}";
    }
}