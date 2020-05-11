namespace pacman.ActionStrategies
{
    using System.Threading;
    public interface IActionStrategy
    {
        NextAction Next(Pac pac, CancellationToken cancellation);
    }
}