using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle21
    {

        public string SolvePuzzle(string input, string applyInputTo, bool quietMode = false)
        {
            string result = applyInputTo;
            foreach (string instruction in input.Split(Environment.NewLine.ToCharArray(), 
                StringSplitOptions.RemoveEmptyEntries))
            {
                if (!quietMode)
                    Console.WriteLine(result + ": '" + instruction + "'");
                string[] instructionComponents = instruction.Split();
                switch(instructionComponents[0])
                {
                    case "swap":
                        result = ApplySwapInstruction(instructionComponents, result.ToCharArray());
                        break;
                    case "reverse":
                        result = ApplyReverseInstruction(instructionComponents, result.ToCharArray());
                        break;
                    case "rotate":
                        result = ApplyRotateInstruction(instructionComponents, result.ToCharArray());
                        break;
                    case "move":
                        result = ApplyMoveInstruction(instructionComponents, result.ToCharArray());
                        break;
                }
                if (!quietMode)
                    Console.WriteLine("Result is now: " + result);
            }

            return result;
        }

        public string SolvePuzzlePart2(string input, string scrambledPassword)
        {
            List<string> allPossibleInputs = CartesianProduct(scrambledPassword);
            foreach(string possibleInput in allPossibleInputs)
            {
                string solution = SolvePuzzle(input, possibleInput, true);
                if (solution == scrambledPassword)
                    return possibleInput;
            }
            return "??";
        }

        private List<string> CartesianProduct(string applyInputTo)
        {
            List<string> permutations = new List<string>();
            GetPer(applyInputTo.ToCharArray(), permutations);
            return permutations;
        }

        /// <summary>
        /// Lifted this code for permutations from SO: http://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
        /// </summary>
        private static void Swap(ref char a, ref char b)
        {
            if (a == b) return;

            a ^= b;
            b ^= a;
            a ^= b;
        }

        /// <summary>
        /// Lifted this code for permutations from SO: 
        /// http://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
        /// </summary>
        private void GetPer(char[] list, List<string> output)
        {
            GetPer(list, 0, list.Length - 1, output);
        }

        /// <summary>
        /// Lifted this code for permutations from SO: 
        /// http://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
        /// </summary>
        private static void GetPer(char[] list, int currentDepth, int maxDepth, List<string> output)
        {
            if (currentDepth == maxDepth)
            {
                output.Add(new string(list));
            }
            else
            {
                for (int i = currentDepth; i <= maxDepth; i++)
                {
                    Swap(ref list[currentDepth], ref list[i]);
                    GetPer(list, currentDepth + 1, maxDepth, output);
                    Swap(ref list[currentDepth], ref list[i]);
                }
            }
        }


        public string ApplySwapInstruction(string[] instructionComponents, char[] input)
        {
            switch(instructionComponents[1])
            {
                // swap position X with position Y
                case "position":
                    int firstLocation = Math.Min(Convert.ToInt32(instructionComponents[2]), 
                        Convert.ToInt32(instructionComponents[5]));
                    int secondLocation = Math.Max(Convert.ToInt32(instructionComponents[2]), 
                        Convert.ToInt32(instructionComponents[5]));
                    char temp = input[firstLocation];
                    input[firstLocation] = input[secondLocation];
                    input[secondLocation] = temp;
                    break;
                // swap letter X with letter Y
                case "letter":
                    string swapper = new string(input);
                    swapper = swapper.Replace(instructionComponents[2], "\t");
                    swapper = swapper.Replace(instructionComponents[5], instructionComponents[2]);
                    swapper = swapper.Replace("\t", instructionComponents[5]);
                    return swapper;
            }
            return new string(input);
        }

        public string ApplyReverseInstruction(string[] instructionComponents, char[] input)
        {
            // reverse positions x through y
            int x = Convert.ToInt32(instructionComponents[2]);
            int y = Convert.ToInt32(instructionComponents[4]);
            // Loop halfway through the range
            for (int i = x; i <= x + ((y - x) / 2); i++)
            {
                // Then swap the character at the index with one the corresponding distance
                // from the other side of the range
                char temp = input[i];
                input[i] = input[y - (i - x)];
                input[y -  (i - x)] = temp;
            }
            return new string(input);
        }

        public string ApplyRotateInstruction(string[] instructionComponents, char[] input)
        {
            StringBuilder result = new StringBuilder();
            int rotateSteps = 0;
            bool rotateToLeft = false;
            switch(instructionComponents[1])
            {
                // rotate based on position of letter X
                case "based":
                    char letter = instructionComponents[6][0];
                    rotateToLeft = false;
                    int indexOfLetter = -1;
                    for(int i = 0; i < input.Length; i++)
                    {
                        if (input[i] == letter)
                        {
                            indexOfLetter = i;
                            break;
                        }
                    }
                    rotateSteps = indexOfLetter + 1;
                    if (indexOfLetter >= 4)
                        rotateSteps++;
                    break;
                // rotate left/right X steps
                case "left":
                    rotateToLeft = true;
                    rotateSteps = Convert.ToInt32(instructionComponents[2]);
                    break;
                // rotate right X steps
                case "right":
                    rotateToLeft = false;
                    rotateSteps = Convert.ToInt32(instructionComponents[2]);
                    break;
            }

            rotateSteps = rotateSteps % input.Length;

            if (rotateToLeft)
            {
                //12345 rotate left by 1 becomes
                //23451
                for (int i = rotateSteps; i < input.Length; i++)
                {
                    result.Append(input[i]);
                }
                for (int i = 0; i < rotateSteps; i++)
                {
                    result.Append(input[i]);
                }
            } else
            {
                //12345 rotate right by 1 becomes
                //51234
                for (int i = (input.Length - rotateSteps); i < input.Length; i++)
                {
                    result.Append(input[i]);
                }
                for (int i = 0; i < input.Length - rotateSteps; i++)
                {
                    result.Append(input[i]);
                }
            }

            return result.ToString();
        }

        public string ApplyMoveInstruction(string[] instructionComponents, char[] input)
        {
            StringBuilder result = new StringBuilder();
            
            //move position X to position Y
            int moveFromPosition = Convert.ToInt32(instructionComponents[2]);
            int moveToPosition = Convert.ToInt32(instructionComponents[5]);

            for(int i = 0; i < input.Length; i++)
            {
                if (i != moveFromPosition)
                    result.Append(input[i]);
            }
            result.Insert(moveToPosition, input[moveFromPosition]);

            return result.ToString();
        }
    }
}
