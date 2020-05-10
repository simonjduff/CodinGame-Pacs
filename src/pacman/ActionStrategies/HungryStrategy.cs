namespace pacman.ActionStrategies
{
    using System.Linq;
    using System.Threading;
    using pacman.PelletsSeenStrategies;
    public class HungryStrategy : IActionStrategy
    {
        private readonly IPelletsSeenStrategy _pelletsSeenStrategy;

        public HungryStrategy(IPelletsSeenStrategy pelletsSeenStrategy)
        {
            _pelletsSeenStrategy = pelletsSeenStrategy;
        }
        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            var pellets = gameGrid.VisiblePelletsFrom(pac.Location);
            return _pelletsSeenStrategy.Next(gameGrid, pac, cancellation, pellets.ToList());
        }
    }
}