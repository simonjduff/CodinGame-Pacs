using System;
using pacman;
using Xunit;

namespace tests
{
    public class RpsTests
    {
        [Theory]
        [InlineData("ROCK", "PAPER", 'L')]
        [InlineData("PAPER", "ROCK", 'W')]
        [InlineData("ROCK", "ROCK", 'D')]
        [InlineData("SCISSORS", "PAPER", 'W')]
        [InlineData("SCISSORS", "ROCK", 'L')]
        public void GamesAreCorrect(string p1, string p2, char outcome)
        {
            PacType player1 = new PacType(p1);
            PacType player2 = new PacType(p2);
            PacType.Outcome expected = (PacType.Outcome)outcome;

            Assert.Equal(expected, player1.Play(player2));
        }
    }
}