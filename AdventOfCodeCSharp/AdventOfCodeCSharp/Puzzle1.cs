using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public enum Direction
    {
        Up,
        Left, 
        Down,
        Right
    }
    public class Puzzle1
    {
        public string PuzzleInput { get; set; }

        Tuple<int, int> currentPosition;
        Direction currentOrientation;

        public Puzzle1()
        {
            currentPosition = new Tuple<int, int>(0, 0);
            currentOrientation = Direction.Up;            
        }
        public int ProcessPuzzle(string input)
        {
            string[] instructions = input.Split(',');
            foreach (string instruction in instructions)
            {
                ApplyInstructionOrientation(instruction.Trim());
                ApplyInstructionMovement(instruction.Trim().Substring(1));
            }
            return Math.Abs(currentPosition.Item1) + Math.Abs(currentPosition.Item2);
        }

        private void ApplyInstructionMovement(string v)
        {
            int movementValue = Convert.ToInt32(v);
            switch (currentOrientation)
            {
                case Direction.Up:
                    currentPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + movementValue);
                    break;
                case Direction.Down:
                    currentPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 - movementValue);
                    break;
                case Direction.Right:
                    currentPosition = new Tuple<int, int>(currentPosition.Item1 + movementValue, currentPosition.Item2);
                    break;
                case Direction.Left:
                    currentPosition = new Tuple<int, int>(currentPosition.Item1 - movementValue, currentPosition.Item2);
                    break;
            }
        }

        private void ApplyInstructionOrientation(string instruction)
        {
            int orientInt = (int)currentOrientation;
            if (instruction.StartsWith("L"))
                orientInt++;
            else
                orientInt--;
            if (orientInt < 0)
                orientInt = 3;
            if (orientInt > 3)
                orientInt = 0;
            currentOrientation = (Direction)orientInt;
        }

    }
}
