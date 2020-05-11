using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using pacman;
using Xunit;

namespace tests
{
    public class LocationTests
    {
        [Theory]
        [InlineData(1,1,1,1,true)]
        [InlineData(1, 1, 1, 2, false)]
        public void EqualityTests(short x1, short y1, short x2, short y2, bool expected)
        {
            Location left = new Location(x1, y1);
            Location right = new Location(x2, y2);
            Assert.Equal(expected, left.Equals(right));
            Assert.Equal(expected, right.Equals(left));
            Assert.Equal(expected, left == right);
            Assert.Equal(expected, right == left);
        }

        [Fact]
        public void ContainsTest()
        {
            List<Location> locations = new List<Location>();
            var left = new Location(1,1);
            var right = new Location(1, 1);
            locations.Add(left);
            Assert.Contains(right, locations);
        }

    }
}