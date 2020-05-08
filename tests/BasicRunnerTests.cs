using System;
using System.Threading;
using System.Threading.Tasks;
using pacman;
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
            var harness = new GameTestHarness();

            await harness.RunAsync();

            while (harness.InputOutput.CanReadOutput)
            {
                var output = harness.InputOutput.ReadOutput();
                _testOutput.WriteLine(output);
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

    };
}