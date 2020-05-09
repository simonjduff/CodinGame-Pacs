namespace pacman.ActionStrategies
{
    using System.Threading;
    public interface IActionStrategy
    {
        NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation);
    }
}