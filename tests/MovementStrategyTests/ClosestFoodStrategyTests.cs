using System.Threading.Tasks;
using pacman;
using pacman.ActionStrategies;
using Xunit;

namespace tests.MovementStrategyTests
{
    public class ClosestFoodStrategyTests
    {
        [Fact]
        public async Task PacGoesToClosestFood()
        {
            Task awaiter = new GameTestHarness()
                .WithTestGrid(31, 13, mapString)
#pragma warning disable 618
                .WithMovementStrategy(g => new ClosestFoodMovementStrategy(g))
#pragma warning restore 618
                //.WithCancellationToken(new CancellationTokenSource(3000).Token)
                .WithPac(new TestPac(1, new Location(25, 11), false))
                .WithPac(new TestPac(0, new Location(9, 1), true))
                .RunAsync(out var inputOutput);
            await awaiter;
            Assert.True(inputOutput.CanReadOutput);
            var output = inputOutput.ReadOutput();
            Assert.Equal("MOVE 0 10 1", output);
        }

        private const string mapString = @"
###############################
###.#.### ..#.....#...###.#.###
###.#.### #####.#####.###.#.###
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