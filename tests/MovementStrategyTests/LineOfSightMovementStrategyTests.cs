using System.Threading.Tasks;
using pacman;
using pacman.ActionStrategies;
using pacman.PelletsSeenStrategies;
using Xunit;

namespace tests.MovementStrategyTests
{
    public class LineOfSightMovementStrategyTests
    {
        [Fact]
        public async Task PacMovesToClosestLineOfSight()
        {
            await new GameTestHarness()
                .WithTestGrid(31, 13, mapString)
                .WithMovementStrategy(g => new HungryStrategy(new LineOfSightMovementStrategy(g), g))
                //.WithCancellationToken(new CancellationTokenSource(3000).Token)
                .WithPac(new TestPac(1, new Location(25,11), false))
                .WithPac(new TestPac(0, new Location(9,1), true))
                .RunAsync(out var inputOutput);

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