﻿using System.Threading;
using pacman;

namespace tests.MovementStrategyTests
{
    public class FixedMovementStrategy : IActionStrategy
    {
        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            return new NextAction(pac, new Location(15, 10));
        }
    }
}