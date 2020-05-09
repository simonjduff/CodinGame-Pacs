using System;
using System.Collections.Generic;
using System.Linq;
using pacman;

namespace tests
{
    public class TestGrid
    {
        private readonly string[] _lines;
        public List<Pellet> Pellets { get; } = new List<Pellet>();
        private readonly FakeInputOutput _inputOutput;
        private readonly List<Pac> _pacs = new List<Pac>();

        public TestGrid(int width, 
            int height, 
            string mapString, 
            FakeInputOutput inputOutput)
        {
            _inputOutput = inputOutput;
            _lines = new string[height];
            var lines = mapString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
            for (short line = 0; line < height; line++)
            {
                char[] finalLine = new char[width];
                
                for (short c = 0; c < width; c++)
                {
                    Location location = new Location(c, line);
                    switch (lines[line][c])
                    {
                        case '.':
                            Pellets.Add(new Pellet(location, 1));
                            finalLine[c] = ' ';
                            break;
                        case 'X':
                            Pellets.Add(new Pellet(location, 10));
                            finalLine[c] = ' ';
                            break;
                        case '#':
                            finalLine[c] = '#';
                            break;
                        default:
                            finalLine[c] = ' ';
                            break;
                    }
                }
                _lines[line] = new string(finalLine);
            }
        }

        public IEnumerable<string> GridLines => _lines.Select(l => new string(l));
        public IEnumerable<string> PelletLines => Pellets.Select(p => $"{p.Location.X} {p.Location.Y} {p.Value}");

        public void AddPac(Pac pac)
        {
            _pacs.Add(pac);
        }

        public void WriteGrid()
        {
            foreach (var line in GridLines)
            {
                _inputOutput.AddInput(line);
            }
        }

        public void WritePellets()
        {
            _inputOutput.AddInput(Pellets.Count.ToString());
            foreach (var pellet in PelletLines)
            {
                _inputOutput.AddInput(pellet);
            }
        }

        public void WriteScores()
        {
            _inputOutput.AddInput("0 0");
        }

        public void WritePacs()
        {
            _inputOutput.AddInput(_pacs.Count.ToString());
            foreach (var pac in _pacs)
            {
                _inputOutput.AddInput($"{pac.Id} {(pac.Mine ? 1 : 0)} {pac.Location.X} {pac.Location.Y} NEUTRAL 0 0");
            }
        }
    }
}