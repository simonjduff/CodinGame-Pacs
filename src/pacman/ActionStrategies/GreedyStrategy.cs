namespace pacman.ActionStrategies
{
    using System.Threading;
    public class GreedyStrategy : IActionStrategy
    {
        private readonly GameGrid _grid;

        public GreedyStrategy(GameGrid grid)
        {
            _grid = grid;
        }

        public NextAction Next(Pac pac, CancellationToken cancellation)
        {
            return null;
        }
    }
}