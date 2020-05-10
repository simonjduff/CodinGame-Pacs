namespace pacman.ActionStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    public class LineOfSightMovementStrategy : IActionStrategy
    {
        private readonly Dictionary<int, NextAction> _nextMoves = new Dictionary<int, NextAction>();
        private static readonly Random Random = new Random();

        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            Location loc = pac.Location;

            var visiblePellets = gameGrid.VisiblePelletsFrom(pac.Location).ToArray();

            if (!visiblePellets.Any())
            {
                return new MoveAction(pac, new Location((short)Random.Next(0, gameGrid.Width), (short)Random.Next(0, gameGrid.Height)));
            }

            var closestY = visiblePellets.OrderBy(p => Math.Abs(p.Location.X - loc.X)).FirstOrDefault();
            var closestX = visiblePellets.OrderBy(p => Math.Abs(p.Location.Y - loc.Y)).FirstOrDefault();

            if (Math.Abs(closestX.Location.X - loc.X) < Math.Abs(closestY.Location.Y - loc.Y))
            {
                return new MoveAction(pac, closestX.Location);
            }
            return new MoveAction(pac, closestX.Location);

        }
    }
}