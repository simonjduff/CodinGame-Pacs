using pacman;
using Xunit;

namespace tests
{
    public class PacTypeTests
    {
        [Theory]
        [InlineData("SCISSORS", "SCISSORS", true)]
        [InlineData("ROCK", "ROCK", true)]
        [InlineData("PAPER", "PAPER", true)]
        [InlineData("SCISSORS", "PAPER", false)]
        [InlineData("ROCK", "PAPER", false)]
        public void EqualityTests(string left, string right, bool expected)
        {
            var leftType = new PacType(left);
            var rightType = new PacType(right);
            Assert.Equal(expected, leftType.Equals(rightType));
            Assert.Equal(expected, rightType.Equals(leftType));
            Assert.Equal(expected, leftType == rightType);
            Assert.Equal(expected, rightType == leftType);

            Assert.Equal(!expected, leftType != rightType);
            Assert.Equal(!expected, rightType != leftType);
        }
    }
}