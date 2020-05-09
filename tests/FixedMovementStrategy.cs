using System.Threading;
using pacman;

namespace tests
{
    public class FixedMovementStrategy : IMovementStrategy
    {
        public NextMove Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            return new NextMove(pac, new Location(15, 10));
        }
    }
}