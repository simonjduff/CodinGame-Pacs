using System.Threading;

namespace pacman.ActionStrategies
{
    public class YappyDogStrategy : IEnemiesSeenStrategy
    {
        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation, params Pac[] enemies)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IEnemiesSeenStrategy
    {
        NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation, params Pac[] enemies);
    }
}