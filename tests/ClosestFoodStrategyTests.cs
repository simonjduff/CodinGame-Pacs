using System.Threading;
using System.Threading.Tasks;
using pacman;
using Xunit;

namespace tests
{
    public class ClosestFoodStrategyTests
    {
        [Fact]
        public async Task PacGoesToClosestFood()
        {
            var pac = new Pac(0, true);
            pac.AddLocation(new Location(9, 1));

            var enemy = new Pac(1, false);
            enemy.AddLocation(new Location(25, 11));

            Task awaiter = new GameTestHarness()
                .WithTestGrid(31, 13, mapString)
                .WithMovementStrategy(new ClosestFoodMovementStrategy())
                //.WithCancellationToken(new CancellationTokenSource(3000).Token)
                .WithPac(enemy)
                .WithPac(pac)
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