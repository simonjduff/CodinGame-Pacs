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
            var harness = new GameTestHarness();
            // Given a grid
            harness.InputOutput.AddInput("31 13");
            TestGrid grid = new TestGrid(31, 13, mapString, harness.InputOutput);
            grid.WriteGrid();

            // And a pac location
            grid.WriteScores();
            var pac = new Pac(0, true);
            pac.AddLocation(new Location(9, 1));
            grid.AddPac(pac);
            grid.WritePacs();

            // And Pellets
            grid.WritePellets();
            await harness.RunAsync();
            Assert.True(harness.InputOutput.CanReadOutput);
            var output = harness.InputOutput.ReadOutput();
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