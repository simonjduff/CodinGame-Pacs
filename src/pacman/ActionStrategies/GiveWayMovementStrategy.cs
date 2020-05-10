namespace pacman.ActionStrategies
{
    using System.Threading;
    public class GiveWayMovementStrategy
    {
        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            if (pac.LocationHistory.Count < 2)
            {
                return new MoveAction(pac, gameGrid.RandomLocation);
            }

            return new MoveAction(pac, pac.LocationHistory[^2]);
        }
    }
}