using System.Threading;
using pacman;
using pacman.ActionStrategies;

namespace tests.MovementStrategyTests
{
    public class FixedMovementStrategy : IActionStrategy
    {
        public NextAction Next(Pac pac, CancellationToken cancellation)
        {
            return new MoveAction(pac, new Location(15, 10));
        }
    }
}