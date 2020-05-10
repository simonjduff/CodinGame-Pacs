using System.Threading;
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
#pragma warning disable 618
            var pac = new Pac(0, true, new ClosestFoodMovementStrategy(), new GiveWayMovementStrategy());
#pragma warning restore 618
            pac.AddLocation(new Location(9, 1));

#pragma warning disable 618
            var enemy = new Pac(1, false, new ClosestFoodMovementStrategy(), new GiveWayMovementStrategy());
#pragma warning restore 618
            enemy.AddLocation(new Location(25, 11));

            Task awaiter = new GameTestHarness()
                .WithTestGrid(31, 13, mapString)
#pragma warning disable 618
                .WithMovementStrategy(new ClosestFoodMovementStrategy())
#pragma warning restore 618
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