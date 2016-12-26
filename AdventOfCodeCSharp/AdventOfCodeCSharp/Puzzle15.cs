using AdventOfCodeCSharp.Puzzle15Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle15
    {

        public int SolvePuzzle(string input)
        {
            KineticSculpture sculpture = new KineticSculpture();
            InitSculpture(input, sculpture);

            int time = 0;
            while (true)
            {
                if (sculpture.CurrentPositionSolvesPuzzle())
                    return time;

                time++;
                sculpture.Rotate();
            }
        }

        public int SolvePuzzlePart2(string input)
        {
            KineticSculpture sculpture = new KineticSculpture();
            InitSculpture(input, sculpture);
            sculpture.InitialiseDisc(sculpture.Discs.Count + 1, 11, 0);

            int time = 0;
            while (true)
            {
                if (sculpture.CurrentPositionSolvesPuzzle())
                    return time;

                time++;
                sculpture.Rotate();
            }
        }

        private static void InitSculpture(string input, KineticSculpture sculpture)
        {
            int discCounter = 0;
            foreach (string instruction in input.Split(Environment.NewLine.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries))
            {
                discCounter++;
                // e.g. "Disc #1 has 5 positions; at time=0, it is at position 4"
                string[] instructionPortions = instruction.Split(' ');
                int positions = Convert.ToInt32(instructionPortions[3]);
                int currentPosition = Convert.ToInt32(instructionPortions[11]);
                sculpture.InitialiseDisc(discCounter, positions, currentPosition);
            }
        }
    }
}
