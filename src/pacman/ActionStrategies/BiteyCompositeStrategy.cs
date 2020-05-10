using System.Threading;

namespace pacman.ActionStrategies
{
    public class BiteyCompositeStrategy : IActionStrategy
    {
        private readonly IActionStrategy _foodVisibleStrategy;
        private readonly IEnemiesSeenStrategy _enemyVisibleStrategy;

        public BiteyCompositeStrategy(IActionStrategy foodVisibleStrategy,
            IEnemiesSeenStrategy enemyVisibleStrategy)
        {
            _enemyVisibleStrategy = enemyVisibleStrategy;
            _foodVisibleStrategy = foodVisibleStrategy;
        }

        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            throw new System.NotImplementedException();
        }
    }
}