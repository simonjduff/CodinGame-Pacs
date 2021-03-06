﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using pacman;
using Xunit;
using Xunit.Abstractions;

namespace tests
{
    public class GameGridTests
    {
        private readonly ITestOutputHelper _output;

        public GameGridTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(9, 1, "10 1 1|11 1 1|9 3 1|9 4 1|9 5 1")]
        //[InlineData(8, 3, "0 3 1|1 3 1|2 3 1|3 3 1|4 3 1|5 3 1|6 3 1|7 3 1|9 3 1|10 3 1|11 3 1|12 3 1|13 3 1|14 3 1|15 3 1|16 3 1|17 3 1|18 3 1|19 3 1|20 3 1|21 3 1|22 3 1|23 3 1|24 3 1|25 3 1|26 3 1|27 3 1|28 3 1|29 3 1|30 3 1")]
        [InlineData(27, 11, "28 11 1|29 11 1|30 11 1|0 11 1|1 11 1|2 11 1|3 11 1|27 10 1|27 9 1")]
        public void LineOfSightFindsPellets(short pacX, short pacY, string visiblePelletsString)
        {
            string mapString = @"
###############################
###.#.### ..#.....#...###.#.###
###.#.### #####.#####.###.#.###
........ ......................
###.###.#.#.###.###.#.#.###.###
#...###...#...#.#...#...###...#
#.#.###.#####.#.#.#####.###.#.#
#.#.......#.X.....X.#.......#.#
#.###.#.#.#.#.#.#.#.#.#.#.###.#
#.....#X......#.#......X#.....#
###.#.###.#.###.###.#.###.#.###
....#.#...#.........#...#.# ...
###############################";

            var testGrid = new TestGrid(31, 13, mapString, new FakeInputOutput(new CancellationTokenSource(1500).Token));

            var grid = new GameGrid();
            grid.StoreGrid(mapString.Replace('.', ' ').Replace('X', ' ').Split(Environment.NewLine));
            grid.SetPellets(testGrid.Pellets);

            var visiblePellets = grid.VisiblePelletsFrom(new Location(pacX, pacY)).ToArray();
            var expected = visiblePelletsString.Split('|').Select(s =>
            {
                var split = s.Split(' ');
                return new Pellet(short.Parse(split[0]), short.Parse(split[1]), short.Parse(split[2]));
            }).ToArray();

            Assert.Equal(expected.Length, visiblePellets.Length);
            foreach (var pellet in expected)
            {
                Assert.Contains(pellet, visiblePellets);
            }
        }

        [Fact]
        public void GridParsesGridShape()
        {
            string grid = @"
###############################
### # ###   #     #   ### # ###
### # ### ##### ##### ### # ###
                               
### ### # # ### ### # # ### ###
#   ###   #   # #   #   ###   #
# # ### ##### # # ##### ### # #
# #       #         #       # #
# ### # # # # # # # # # # ### #
#     #       # #       #     #
### # ### # ### ### # ### # ###
    # #   #         #   # #    
###############################";
            var lines = (IEnumerable<string>) grid.Split(Environment.NewLine);
            GameGrid gameGrid = new GameGrid();
            gameGrid.StoreGrid(lines);

            Assert.False(gameGrid.Traversable(new Location(0,0)));
            Assert.True(gameGrid.Traversable(new Location(3, 1)));

            var location = new Location(3,1);
            var location2 = new Location(5, 1);
            gameGrid.SetPellets(new[] { new Pellet(location, 10), new Pellet(location2, 1) });
            Assert.Equal(10, gameGrid.FoodValue(location));
            Assert.Equal(1, gameGrid.FoodValue(location2));
        }

        [Fact]
        public void ClearsMissingVisiblePellets()
        {
            string mapString = @"
###############################
###.#.### ..#.....#...###.#.###
###.#.### #####.#####.###.#.###
........ ......................
###.###.#.#.###.###.#.#.###.###
#...###...#...#.#...#...###...#
#.#.###.#####.#.#.#####.###.#.#
#.#.......#.X.....X.#.... ..#.#
#.###.#.#.#.#.#.#.#.#.#.#.###.#
#.....#X......#.#......X#  .. #
###.#.###.#.###.###.#.### #.###
....#.#...#.........#...#.# ...
###############################";

            var testGrid = new TestGrid(31, 13, mapString, new FakeInputOutput(new CancellationTokenSource(1500).Token));

            var grid = new GameGrid();
            grid.StoreGrid(mapString.Replace('.', ' ').Replace('X', ' ').Split(Environment.NewLine));

            _output.WriteLine($"Base Grid");
            _output.WriteLine(grid.ToString());

            grid.SetPellets(testGrid.Pellets);

            _output.WriteLine($"Set pellets");
            _output.WriteLine(grid.ToString());

            grid.ClearPossiblePelletAt(new Location(25, 9));

            _output.WriteLine($"Clear at pac location");
            _output.WriteLine(grid.ToString());

            grid.ClearEmptyPelletsVisibleFromLocation(new Location(25, 9));
            _output.WriteLine($"Clear visible");
            _output.WriteLine(grid.ToString());

            Assert.Equal(0, grid.FoodValue(new Location(25, 9)));
            Assert.Equal(0, grid.FoodValue(new Location(25, 10)));
            Assert.Equal(0, grid.FoodValue(new Location(25, 7)));
            Assert.Equal(0, grid.FoodValue(new Location(26, 9)));
            Assert.Equal(0, grid.FoodValue(new Location(29, 9)));
            Assert.Equal(1, grid.FoodValue(new Location(25, 8)));
            Assert.Equal(1, grid.FoodValue(new Location(28, 9)));
            Assert.Equal(0, grid.FoodValue(new Location(29, 9)));
        }
    }
}