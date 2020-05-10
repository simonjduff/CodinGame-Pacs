namespace pacman.ActionStrategies
{
    using System.Linq;
    using System.Threading;
    using pacman.EnemiesSeenStrategies;
    using pacman.PelletsSeenStrategies;
    using System;
    public class BiteyCompositeStrategy : IActionStrategy
    {
        private readonly IPelletsSeenStrategy _foodVisibleStrategy;
        private readonly IEnemiesSeenStrategy _enemyVisibleStrategy;

        public BiteyCompositeStrategy(IPelletsSeenStrategy foodVisibleStrategy,
            IEnemiesSeenStrategy enemyVisibleStrategy)
        {
            _enemyVisibleStrategy = enemyVisibleStrategy;
            _foodVisibleStrategy = foodVisibleStrategy;
        }

        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            NextAction nextAction = new NoAction(pac);

            var enemies = gameGrid.VisibleEnemiesFrom(pac.Location).ToArray();
            if (enemies.Any())
            {
                nextAction = _enemyVisibleStrategy.Next(gameGrid, pac, cancellation, enemies);
                Console.Error.WriteLine($"Pac {pac.Id} has seen enemies. Next action {nextAction}");
            }

            if (!(nextAction is NoAction))
            {
                return nextAction;
            }

            var pellets = gameGrid.VisiblePelletsFrom(pac.Location);
            nextAction = _foodVisibleStrategy.Next(gameGrid, pac, cancellation, pellets.ToList());
            Console.Error.WriteLine($"Pac {pac.Id} food search. Next action {nextAction}. {(cancellation.IsCancellationRequested ? "CANCELLED" : string.Empty)}");
            return nextAction;
        }
    }
}