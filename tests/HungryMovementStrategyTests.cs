using System.Threading.Tasks;
using pacman;
using Xunit;

namespace tests
{
    public class HungryMovementStrategyTests
    {
        [Fact]
        public async Task PacGoesToMostFood()
        {
            var pac = new Pac(0, true);
            pac.AddLocation(new Location(9, 1));

            Task awaiter = new GameTestHarness()
                .WithTestGrid(31, 13, mapString)
                .WithPac(pac)
                .RunAsync(out var inputOutput);
            await awaiter;
            Assert.True(inputOutput.CanReadOutput);
            var output = inputOutput.ReadOutput();
            Assert.Equal("MOVE 0 9 2", output);
        }

        private const string mapString = @"
###############################
###.#.### ..#.....#...###.#.###
###.#.###.#####.#####.###.#.###
...............................
###.###.#.#.###.###.#.#.###.###
#...###...#...#.#...#...###...#
#.#.###.#####.#.#.#####.###.#.#
#.#.......#.X.....X.#.......#.#
#.###.#.#.#.#.#.#.#.#.#.#.###.#
#.....#X......#.#......X#.....#
###.#.###.#.###.###.#.###.#.###
....#.#...#.........#...#.#....
###############################";
    }
}