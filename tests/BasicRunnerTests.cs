using System;
using System.Threading;
using System.Threading.Tasks;
using pacman;
using pacman.ActionStrategies;
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
            var pac = new Pac(0, true, new FixedMovementStrategy(), new GiveWayMovementStrategy());
            pac.AddLocation(new Location(9, 1));
            var harness = new GameTestHarness()
                .WithTestGrid(31, 13, mapString)
                .WithPac(pac)
                .WithMovementStrategy(new FixedMovementStrategy())
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