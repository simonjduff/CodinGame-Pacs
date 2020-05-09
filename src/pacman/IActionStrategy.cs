namespace pacman
{
    using System.Threading;
    public interface IActionStrategy
    {
        NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation);
    }
}