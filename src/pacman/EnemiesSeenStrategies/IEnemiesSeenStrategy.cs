namespace pacman.EnemiesSeenStrategies
{
    using System.Threading;
    public interface IEnemiesSeenStrategy
    {
        NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation, params Pac[] enemies);
    }
}