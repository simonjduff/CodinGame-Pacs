namespace pacman
{
    public class GameLoop
    {
        private readonly IInputOutput _inputOutput;

        public GameLoop(IInputOutput inputOutput)
        {
            _inputOutput = inputOutput;
        }

        public void Run()
        {
            while (true)
            {
                string[] inputs = _inputOutput.ReadLine().Split(' ');
                int myScore = int.Parse(inputs[0]);
                int opponentScore = int.Parse(inputs[1]);
                int visiblePacCount = int.Parse(_inputOutput.ReadLine()); // all your pacs and enemy pacs in sight
                for (int i = 0; i < visiblePacCount; i++)
                {
                    inputs = _inputOutput.ReadLine().Split(' ');
                    int pacId = int.Parse(inputs[0]); // pac number (unique within a team)
                    bool mine = inputs[1] != "0"; // true if this pac is yours
                    int x = int.Parse(inputs[2]); // position in the grid
                    int y = int.Parse(inputs[3]); // position in the grid
                    string typeId = inputs[4]; // unused in wood leagues
                    int speedTurnsLeft = int.Parse(inputs[5]); // unused in wood leagues
                    int abilityCooldown = int.Parse(inputs[6]); // unused in wood leagues
                }
                int visiblePelletCount = int.Parse(_inputOutput.ReadLine()); // all pellets in sight
                for (int i = 0; i < visiblePelletCount; i++)
                {
                    inputs = _inputOutput.ReadLine().Split(' ');
                    int x = int.Parse(inputs[0]);
                    int y = int.Parse(inputs[1]);
                    int value = int.Parse(inputs[2]); // amount of points this pellet is worth
                }

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                _inputOutput.WriteLine("MOVE 0 15 10"); // MOVE <pacId> <x> <y>

            }
        }
    }
}