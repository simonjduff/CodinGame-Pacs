namespace pacman
{
    class Player
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            player.Run(new ConsoleInputOutput());
        }

        public void Run(IInputOutput inputOutput)
        {
            string[] inputs;
            inputs = inputOutput.ReadLine().Split(' ');
            int width = int.Parse(inputs[0]); // size of the grid
            int height = int.Parse(inputs[1]); // top left corner is (x=0, y=0)
            for (int i = 0; i < height; i++)
            {
                string row = inputOutput.ReadLine(); // one line of the grid: space " " is floor, pound "#" is wall
            }

            // game loop
            var loop = new GameLoop(inputOutput);
            loop.Run();
        }
    }
}
