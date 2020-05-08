﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using pacman;

namespace tests
{
    public class GameTestHarness
    {
        private static CancellationToken _token = new CancellationTokenSource(1500).Token;
        private string _gridInput;
        private int _gridWidth;
        private int _gridHeight;
        private readonly List<Pac> _pacs = new List<Pac>();

        public GameTestHarness WithCancellationToken(CancellationToken token)
        {
            _token = token;
            return this;
        }

        public GameTestHarness WithTestGrid(int width, int height, string gridInput)
        {
            _gridHeight = height;
            _gridWidth = width;
            _gridInput = gridInput;
            return this;
        }

        public GameTestHarness WithPac(Pac pac)
        {
            _pacs.Add(pac);
            return this;
        }

        public Task RunAsync(out FakeInputOutput inputOutput)
        {
            var localInputOutput = new FakeInputOutput(_token);
            var testGrid = new TestGrid(_gridWidth, _gridHeight, _gridInput, localInputOutput);

            localInputOutput.AddInput($"{_gridWidth} {_gridHeight}");
            testGrid.WriteGrid();
            testGrid.WriteScores();
            testGrid.WritePacs();
            testGrid.WritePellets();

            var game = new Player(localInputOutput);
            var task = Task.Run(() => game.Run(_token), _token).ContinueWith(t => localInputOutput.CompleteOutput());
            inputOutput = localInputOutput;
            return task;
        }
    }
}