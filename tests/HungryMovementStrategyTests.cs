using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using pacman;
using Xunit;

namespace tests
{
    public class HungryMovementStrategyTests : PacTestBase
    {
        [Fact]
        public async Task PacGoesToMostFood()
        {
            // Given a grid
            InputOutput.AddInput("31 13");
            TestGrid grid = new TestGrid(31, 13, mapString, InputOutput);
            grid.WriteGrid();

            // And a pac location
            grid.WriteScores();
            var pac = new Pac(0, true);
            pac.AddLocation(new Location(9, 1));
            grid.AddPac(pac);
            grid.WritePacs();

            // And Pellets
            grid.WritePellets();
            await Awaiter;
            Assert.True(InputOutput.CanReadOutput);
            var output = InputOutput.ReadOutput();
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