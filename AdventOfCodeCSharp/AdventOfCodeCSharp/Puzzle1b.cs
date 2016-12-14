using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    
    public class Puzzle1b
    {
        public string PuzzleInput { get; set; }

        Tuple<int, int> currentPosition;
        Direction currentOrientation;

        List<Tuple<int, int>> movementHistory;

        public Puzzle1b()
        {
            currentPosition = new Tuple<int, int>(0, 0);
            currentOrientation = Direction.Up;
            movementHistory = new List<Tuple<int, int>>();
        }
        public int ProcessPuzzle(string input)
        {
            string[] instructions = input.Split(',');
            foreach (string instruction in instructions)
            {
                ApplyInstructionOrientation(instruction.Trim());
                if (ApplyInstructionMovement(instruction.Trim().Substring(1)))
                    break; 
            }
            return Math.Abs(currentPosition.Item1) + Math.Abs(currentPosition.Item2);
        }

        private bool ApplyInstructionMovement(string v)
        {
            int movementValue = Convert.ToInt32(v);
            for (int i = 1; i <= movementValue; i++)
            {
                switch (currentOrientation)
                {
                    case Direction.Up:
                        currentPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + 1);
                        break;
                    case Direction.Down:
                        currentPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 - 1);
                        break;
                    case Direction.Right:
                        currentPosition = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2);
                        break;
                    case Direction.Left:
                        currentPosition = new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2);
                        break;
                }
                foreach(var previousLocation in movementHistory)
                {
                    if (previousLocation.Item1 == currentPosition.Item1 && 
                        previousLocation.Item2 == currentPosition.Item2)
                    {
                        return true;
                    }
                }
                movementHistory.Add(currentPosition);
            }
            return false;
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
