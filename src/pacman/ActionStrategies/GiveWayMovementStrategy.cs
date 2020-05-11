namespace pacman.ActionStrategies
{
    using System.Threading;
    public class GiveWayMovementStrategy
    {
        private readonly GameGrid _gameGrid;

        public GiveWayMovementStrategy(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
        }
        public NextAction Next(Pac pac, CancellationToken cancellation)
        {
            if (pac.LocationHistory.Count < 2)
            {
                return new MoveAction(pac, _gameGrid.RandomLocation);
            }

            return new MoveAction(pac, pac.LocationHistory[^2]);
        }
    }
}