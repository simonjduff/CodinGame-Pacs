namespace pacman
{
    using System;
    public class Player
    {
        private readonly IInputOutput _consoleInputOutput;

        public Player(IInputOutput consoleInputOutput)
        {
            _consoleInputOutput = consoleInputOutput;
        }

        static void Main(string[] args)
        {
            //var inputOutput = new LoggingConsoleInputOutput();
            var inputOutput = new ConsoleInputOutput();
            Player player = new Player(inputOutput);
            player.Run();
        }

        public void Run()
        {
            string[] inputs;
            var gridSizeInput = _consoleInputOutput.ReadLine();

            inputs = gridSizeInput.Split(' ');
            int width = int.Parse(inputs[0]); // size of the grid
            int height = int.Parse(inputs[1]); // top left corner is (x=0, y=0)
            for (int i = 0; i < height; i++)
            {
                string row = _consoleInputOutput.ReadLine(); // one line of the grid: space " " is floor, pound "#" is wall
            }

            // game loop
            var loop = new GameLoop(_consoleInputOutput);
            loop.Run();
        }
    }
}
