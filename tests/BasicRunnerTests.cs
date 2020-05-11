using System.Threading.Tasks;
using pacman;
using tests.MovementStrategyTests;
using Xunit;
using Xunit.Abstractions;

namespace tests
{
    public class BasicRunnerTests
    {
        private readonly ITestOutputHelper _testOutput;

        public BasicRunnerTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public async Task Test1()
        {
            var harness = new GameTestHarness()
                .WithTestGrid(31, 13, mapString)
                .WithPac(new TestPac(0, new Location(9, 1), true))
                .WithMovementStrategy(g => new FixedMovementStrategy())
                .RunAsync(out FakeInputOutput inputOutput);

            await harness;
            while (inputOutput.CanReadOutput)
            {
                var output = inputOutput.ReadOutput();
                Assert.Equal("MOVE 0 15 10", output);
            }
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