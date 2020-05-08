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
            var cancellation = new CancellationTokenSource();

            var inputOutput = new FakeInputOutput(cancellation.Token);
            var game = new Player(inputOutput);

            var cancellationToken = cancellation.Token;
            var runner = Task.Run(() => game.Run(cancellationToken), cancellationToken);

            inputOutput.AddInput("31 13");
            inputOutput.AddInput("###############################");
            inputOutput.AddInput("### # ###   #     #   ### # ###");
            inputOutput.AddInput("### # ### ##### ##### ### # ###");
            inputOutput.AddInput("                               ");
            inputOutput.AddInput("### ### # # ### ### # # ### ###");
            inputOutput.AddInput("#   ###   #   # #   #   ###   #");
            inputOutput.AddInput("# # ### ##### # # ##### ### # #");
            inputOutput.AddInput("# #       #         #       # #");
            inputOutput.AddInput("# ### # # # # # # # # # # ### #");
            inputOutput.AddInput("#     #       # #       #     #");
            inputOutput.AddInput("### # ### # ### ### # ### # ###");
            inputOutput.AddInput("    # #   #         #   # #    ");
            inputOutput.AddInput("###############################");

            inputOutput.AddInput("0 0");
            inputOutput.AddInput("2");
            inputOutput.AddInput("0 1 9 1 NEUTRAL 0 0");
            inputOutput.AddInput("0 0 21 1 NEUTRAL 0 0");
            inputOutput.AddInput("185");
            inputOutput.AddInput("3 1 1");
            inputOutput.AddInput("5 1 1");
            inputOutput.AddInput("10 1 1");
            inputOutput.AddInput("11 1 1");
            inputOutput.AddInput("13 1 1");
            inputOutput.AddInput("14 1 1");
            inputOutput.AddInput("15 1 1");
            inputOutput.AddInput("16 1 1");
            inputOutput.AddInput("17 1 1");
            inputOutput.AddInput("19 1 1");
            inputOutput.AddInput("20 1 1");
            inputOutput.AddInput("25 1 1");
            inputOutput.AddInput("27 1 1");
            inputOutput.AddInput("3 2 1");
            inputOutput.AddInput("5 2 1");
            inputOutput.AddInput("9 2 1");
            inputOutput.AddInput("15 2 1");
            inputOutput.AddInput("21 2 1");
            inputOutput.AddInput("25 2 1");
            inputOutput.AddInput("27 2 1");
            inputOutput.AddInput("0 3 1");
            inputOutput.AddInput("1 3 1");
            inputOutput.AddInput("2 3 1");
            inputOutput.AddInput("3 3 1");
            inputOutput.AddInput("4 3 1");
            inputOutput.AddInput("5 3 1");
            inputOutput.AddInput("6 3 1");
            inputOutput.AddInput("7 3 1");
            inputOutput.AddInput("8 3 1");
            inputOutput.AddInput("9 3 1");
            inputOutput.AddInput("10 3 1");
            inputOutput.AddInput("11 3 1");
            inputOutput.AddInput("12 3 1");
            inputOutput.AddInput("13 3 1");
            inputOutput.AddInput("14 3 1");
            inputOutput.AddInput("15 3 1");
            inputOutput.AddInput("16 3 1");
            inputOutput.AddInput("17 3 1");
            inputOutput.AddInput("18 3 1");
            inputOutput.AddInput("19 3 1");
            inputOutput.AddInput("20 3 1");
            inputOutput.AddInput("21 3 1");
            inputOutput.AddInput("22 3 1");
            inputOutput.AddInput("23 3 1");
            inputOutput.AddInput("24 3 1");
            inputOutput.AddInput("25 3 1");
            inputOutput.AddInput("26 3 1");
            inputOutput.AddInput("27 3 1");
            inputOutput.AddInput("28 3 1");
            inputOutput.AddInput("29 3 1");
            inputOutput.AddInput("30 3 1");
            inputOutput.AddInput("3 4 1");
            inputOutput.AddInput("7 4 1");
            inputOutput.AddInput("9 4 1");
            inputOutput.AddInput("11 4 1");
            inputOutput.AddInput("15 4 1");
            inputOutput.AddInput("19 4 1");
            inputOutput.AddInput("21 4 1");
            inputOutput.AddInput("23 4 1");
            inputOutput.AddInput("27 4 1");
            inputOutput.AddInput("1 5 1");
            inputOutput.AddInput("2 5 1");
            inputOutput.AddInput("3 5 1");
            inputOutput.AddInput("7 5 1");
            inputOutput.AddInput("8 5 1");
            inputOutput.AddInput("9 5 1");
            inputOutput.AddInput("11 5 1");
            inputOutput.AddInput("12 5 1");
            inputOutput.AddInput("13 5 1");
            inputOutput.AddInput("15 5 1");
            inputOutput.AddInput("17 5 1");
            inputOutput.AddInput("18 5 1");
            inputOutput.AddInput("19 5 1");
            inputOutput.AddInput("21 5 1");
            inputOutput.AddInput("22 5 1");
            inputOutput.AddInput("23 5 1");
            inputOutput.AddInput("27 5 1");
            inputOutput.AddInput("28 5 1");
            inputOutput.AddInput("29 5 1");
            inputOutput.AddInput("1 6 1");
            inputOutput.AddInput("3 6 1");
            inputOutput.AddInput("7 6 1");
            inputOutput.AddInput("13 6 1");
            inputOutput.AddInput("15 6 1");
            inputOutput.AddInput("17 6 1");
            inputOutput.AddInput("23 6 1");
            inputOutput.AddInput("27 6 1");
            inputOutput.AddInput("29 6 1");
            inputOutput.AddInput("1 7 1");
            inputOutput.AddInput("3 7 1");
            inputOutput.AddInput("4 7 1");
            inputOutput.AddInput("5 7 1");
            inputOutput.AddInput("6 7 1");
            inputOutput.AddInput("7 7 1");
            inputOutput.AddInput("8 7 1");
            inputOutput.AddInput("9 7 1");
            inputOutput.AddInput("11 7 1");
            inputOutput.AddInput("13 7 1");
            inputOutput.AddInput("14 7 1");
            inputOutput.AddInput("15 7 1");
            inputOutput.AddInput("16 7 1");
            inputOutput.AddInput("17 7 1");
            inputOutput.AddInput("19 7 1");
            inputOutput.AddInput("21 7 1");
            inputOutput.AddInput("22 7 1");
            inputOutput.AddInput("23 7 1");
            inputOutput.AddInput("24 7 1");
            inputOutput.AddInput("25 7 1");
            inputOutput.AddInput("26 7 1");
            inputOutput.AddInput("27 7 1");
            inputOutput.AddInput("29 7 1");
            inputOutput.AddInput("1 8 1");
            inputOutput.AddInput("5 8 1");
            inputOutput.AddInput("7 8 1");
            inputOutput.AddInput("9 8 1");
            inputOutput.AddInput("11 8 1");
            inputOutput.AddInput("13 8 1");
            inputOutput.AddInput("15 8 1");
            inputOutput.AddInput("17 8 1");
            inputOutput.AddInput("19 8 1");
            inputOutput.AddInput("21 8 1");
            inputOutput.AddInput("23 8 1");
            inputOutput.AddInput("25 8 1");
            inputOutput.AddInput("29 8 1");
            inputOutput.AddInput("1 9 1");
            inputOutput.AddInput("2 9 1");
            inputOutput.AddInput("3 9 1");
            inputOutput.AddInput("4 9 1");
            inputOutput.AddInput("5 9 1");
            inputOutput.AddInput("8 9 1");
            inputOutput.AddInput("9 9 1");
            inputOutput.AddInput("10 9 1");
            inputOutput.AddInput("11 9 1");
            inputOutput.AddInput("12 9 1");
            inputOutput.AddInput("13 9 1");
            inputOutput.AddInput("15 9 1");
            inputOutput.AddInput("17 9 1");
            inputOutput.AddInput("18 9 1");
            inputOutput.AddInput("19 9 1");
            inputOutput.AddInput("20 9 1");
            inputOutput.AddInput("21 9 1");
            inputOutput.AddInput("22 9 1");
            inputOutput.AddInput("25 9 1");
            inputOutput.AddInput("26 9 1");
            inputOutput.AddInput("27 9 1");
            inputOutput.AddInput("28 9 1");
            inputOutput.AddInput("29 9 1");
            inputOutput.AddInput("3 10 1");
            inputOutput.AddInput("5 10 1");
            inputOutput.AddInput("9 10 1");
            inputOutput.AddInput("11 10 1");
            inputOutput.AddInput("15 10 1");
            inputOutput.AddInput("19 10 1");
            inputOutput.AddInput("21 10 1");
            inputOutput.AddInput("25 10 1");
            inputOutput.AddInput("27 10 1");
            inputOutput.AddInput("0 11 1");
            inputOutput.AddInput("1 11 1");
            inputOutput.AddInput("2 11 1");
            inputOutput.AddInput("3 11 1");
            inputOutput.AddInput("5 11 1");
            inputOutput.AddInput("7 11 1");
            inputOutput.AddInput("8 11 1");
            inputOutput.AddInput("9 11 1");
            inputOutput.AddInput("11 11 1");
            inputOutput.AddInput("12 11 1");
            inputOutput.AddInput("13 11 1");
            inputOutput.AddInput("14 11 1");
            inputOutput.AddInput("15 11 1");
            inputOutput.AddInput("16 11 1");
            inputOutput.AddInput("17 11 1");
            inputOutput.AddInput("18 11 1");
            inputOutput.AddInput("19 11 1");
            inputOutput.AddInput("21 11 1");
            inputOutput.AddInput("22 11 1");
            inputOutput.AddInput("23 11 1");
            inputOutput.AddInput("25 11 1");
            inputOutput.AddInput("27 11 1");
            inputOutput.AddInput("28 11 1");
            inputOutput.AddInput("29 11 1");
            inputOutput.AddInput("30 11 1");
            inputOutput.AddInput("12 7 10");
            inputOutput.AddInput("18 7 10");
            inputOutput.AddInput("7 9 10");
            inputOutput.AddInput("23 9 10");

            cancellation.CancelAfter(1500);
            await runner;
            inputOutput.CompleteOutput();

            while (!inputOutput.CanReadOutput)
            {
                var output = inputOutput.ReadOutput();
                _testOutput.WriteLine(output);
            }
        }
    }
}
