namespace pacman
{
    using System.Threading;
    public interface IMovementStrategy
    {
        NextMove Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation);
    }
}