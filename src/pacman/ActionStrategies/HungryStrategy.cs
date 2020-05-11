namespace pacman.ActionStrategies
{
    using System.Linq;
    using System.Threading;
    using PelletsSeenStrategies;
    public class HungryStrategy : IActionStrategy
    {
        private readonly IPelletsSeenStrategy _pelletsSeenStrategy;
        private readonly GameGrid _gameGrid;

        public HungryStrategy(IPelletsSeenStrategy pelletsSeenStrategy,
            GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
            _pelletsSeenStrategy = pelletsSeenStrategy;
        }
        public NextAction Next(Pac pac, CancellationToken cancellation)
        {
            var pellets = _gameGrid.VisiblePelletsFrom(pac.Location);
            return _pelletsSeenStrategy.Next(pac, cancellation, pellets.ToList());
        }
    }
}