using System.Threading.Tasks;
using pacman;
using Xunit;

namespace tests
{
    public class BasicRunnerTests
    {
        [Fact]
        public async Task Test1()
        {
            var inputOutput = new FakeInputOutput();
            var game = new Player(inputOutput);

            var runner = Task.Run(() => game.Run());

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

            await runner;
        }
    }
}
