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
            var pac = new Pac(0, true, new HungryStrategy(new LineOfSightMovementStrategy()));
            pac.AddLocation(new Location(9, 1));

            var enemy = new Pac(1, false, new HungryStrategy(new LineOfSightMovementStrategy()));
            enemy.AddLocation(new Location(25, 11));

            await new GameTestHarness()
                .WithTestGrid(31, 13, mapString)
                .WithMovementStrategy(new HungryStrategy(new LineOfSightMovementStrategy()))
                //.WithCancellationToken(new CancellationTokenSource(3000).Token)
                .WithPac(enemy)
                .WithPac(pac)
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