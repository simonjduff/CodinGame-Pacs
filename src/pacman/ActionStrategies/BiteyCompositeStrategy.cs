namespace pacman.ActionStrategies
{
    using System.Linq;
    using System.Threading;
    using EnemiesSeenStrategies;
    using PelletsSeenStrategies;
    using System;
    public class BiteyCompositeStrategy : IActionStrategy
    {
        private readonly IPelletsSeenStrategy _foodVisibleStrategy;
        private readonly IEnemiesSeenStrategy _enemyVisibleStrategy;
        private readonly GameGrid _gameGrid;

        public BiteyCompositeStrategy(IPelletsSeenStrategy foodVisibleStrategy,
            IEnemiesSeenStrategy enemyVisibleStrategy,
            GameGrid gameGrid)
        {
            this._gameGrid = gameGrid;
            _enemyVisibleStrategy = enemyVisibleStrategy;
            _foodVisibleStrategy = foodVisibleStrategy;
        }

        public NextAction Next(Pac pac, CancellationToken cancellation)
        {
            NextAction nextAction = new NoAction(pac);

            var enemies = _gameGrid.VisibleEnemiesFrom(pac.Location).ToArray();
            if (enemies.Any())
            {
                nextAction = _enemyVisibleStrategy.Next(pac, cancellation, enemies);
                Console.Error.WriteLine($"Pac {pac.Id} has seen enemies. Next action {nextAction}");
            }

            if (!(nextAction is NoAction))
            {
                return nextAction;
            }

            var pellets = _gameGrid.VisiblePelletsFrom(pac.Location);
            nextAction = _foodVisibleStrategy.Next(pac, cancellation, pellets.ToList());
            Console.Error.WriteLine($"Pac {pac.Id} food search. Next action {nextAction}. {(cancellation.IsCancellationRequested ? "CANCELLED" : string.Empty)}");
            return nextAction;
        }
    }
}