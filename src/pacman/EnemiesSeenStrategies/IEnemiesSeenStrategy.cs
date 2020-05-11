namespace pacman.EnemiesSeenStrategies
{
    using System.Threading;
    public interface IEnemiesSeenStrategy
    {
        NextAction Next(Pac pac, CancellationToken cancellation, params Pac[] enemies);
    }
}