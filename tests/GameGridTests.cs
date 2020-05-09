using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using pacman;
using Xunit;

namespace tests
{
    public class GameGridTests
    {
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

            Assert.False(gameGrid[new Location(0,0)].Traversable);
            Assert.True(gameGrid[new Location(3, 1)].Traversable);

            var location = new Location(3,1);
            Assert.Equal(0, gameGrid.FoodValue(location));
            gameGrid.SetPellets(new []{new Pellet(location, 10)});
            Assert.Equal(10, gameGrid.FoodValue(location));
            gameGrid.EatFood(location);
            Assert.Equal(0, gameGrid.FoodValue(location));
        }
    }
}