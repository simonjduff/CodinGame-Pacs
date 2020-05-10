using pacman;
using pacman.ActionStrategies;
using tests.MovementStrategyTests;
using Xunit;

namespace tests
{
    public class PacTests
    {
        [Fact]
        public void HashCodesDifferOnMine()
        {
            var left = new Pac(1, true, new FixedMovementStrategy(), new GiveWayMovementStrategy());
            var right = new Pac(1, false, new FixedMovementStrategy(), new GiveWayMovementStrategy());
            Assert.NotEqual(left.Key, right.Key);
            Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
        }
    }
}