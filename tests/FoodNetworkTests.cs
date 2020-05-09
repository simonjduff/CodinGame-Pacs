using System;
using System.Collections.Generic;
using System.Linq;
using pacman;
using Xunit;

namespace tests
{
    public class FoodNetworkTests
    {
        [Fact]
        public void FoodNetworkCountsFoot()
        {
            // Given food
            List<Pellet> pellets = new List<Pellet>{
                new Pellet(1,2, 1),
                new Pellet(1,3, 1),
                new Pellet(3,4, 1)
            };

            // When I create the food networks
            FoodNetworksBuilder builder = new FoodNetworksBuilder()
                .AddPellets(pellets);
            List<FoodNetwork> networks = builder.Build();

            // Then there are two shapes
            Assert.Equal(2, networks.Count);
            var ordered = networks.OrderByDescending(n => n.Value).ToArray();
            Assert.Equal(2, ordered[0].Value);
            Assert.Equal(1, ordered[1].Value);
        }
    }

    public class FoodNetworksBuilder
    {
        private Dictionary<Location, FoodNetwork> _networks = new Dictionary<Location, FoodNetwork>();

        public FoodNetworksBuilder AddPellets(IEnumerable<Pellet> pellets)
        {
            foreach (var pellet in pellets)
            {
                //_networks[pellet.Location] = pellet;

            }


            return this;
        }

        public List<FoodNetwork> Build()
        {
            throw new NotImplementedException();
        }
    }

    public struct FoodNetwork
    {
        public int Value { get; set; }
    }
}