﻿namespace pacman
{
    using System.Collections.Generic;
    using System.Threading;
    using pacman.ActionStrategies;
    public class Player
    {
        private readonly IInputOutput _consoleInputOutput;
        private readonly IActionStrategy _actionStrategy;

        public Player(IInputOutput consoleInputOutput,
            IActionStrategy actionStrategy)
        {
            _actionStrategy = actionStrategy;
            _consoleInputOutput = consoleInputOutput;
        }

        static void Main(string[] args)
        {
            //var inputOutput = new LoggingConsoleInputOutput();
            var inputOutput = new ConsoleInputOutput();
            //Player player = new Player(inputOutput, new ClosestFoodMovementStrategy());
            Player player = new Player(inputOutput, new LineOfSightMovementStrategy());
            var cancellation = new CancellationTokenSource();
            player.Run(cancellation.Token);
        }

        public void Run(CancellationToken cancellation)
        {
            string[] inputs;
            var gridSizeInput = _consoleInputOutput.ReadLine();

            inputs = gridSizeInput.Split(' ');
            int width = int.Parse(inputs[0]); // size of the grid
            int height = int.Parse(inputs[1]); // top left corner is (x=0, y=0)
            
            var grid = new GameGrid();
            grid.StoreGrid(ReadGrid(height));

            // game loop
            var loop = new GameLoop(_consoleInputOutput, 
                cancellation, 
                _actionStrategy,
                grid);
            loop.Run();
        }

        public IEnumerable<string> ReadGrid(int height)
        {
            for (int i = 0; i < height; i++)
            {
                yield return _consoleInputOutput.ReadLine(); // one line of the grid: space " " is floor, pound "#" is wall
            }
        }
    }
}
