using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle12
    {
        

        public int SolvePuzzle(string input, bool overrideCValue)
        {
            List<string> instructions = ParseInput(input);
            
            Dictionary<char, int> registers = new Dictionary<char, int>();

            foreach(char c in "abcd")
            {
                registers[c] = 0;
                if (overrideCValue && c == 'c')
                    registers[c] = 1;
            }
            int processInstruction = 0;
            while (processInstruction <= instructions.Count - 1)
            {
                string[] instructionPortions = instructions[processInstruction].Split(' ');
                switch (instructionPortions[0])
                {
                    case "cpy":
                        ProcessCopy(registers, instructionPortions[1], instructionPortions[2]);
                        break;
                    case "inc":
                        registers[instructionPortions[1][0]]++;
                        break;
                    case "dec":
                        registers[instructionPortions[1][0]]--;
                        break;
                    case "jnz":
                        int jump = JumpInstructionResult(registers, instructionPortions[1], instructionPortions[2]);
                        processInstruction += jump;
                        if (jump != 0)
                            continue;
                        break;
                }
                processInstruction++;
            }

            return registers['a'];
        }

        private int JumpInstructionResult(Dictionary<char, int> registers, string x, string y)
        {
            // jumps to an instruction y away (positive means forward; negative means backward), but only if x is not zero.
            int jumpNumber;
            if (!int.TryParse(y, out jumpNumber))
            {
                if (y.Length != 1)
                    throw new ArgumentException("Received register number not 1 char long");
                jumpNumber = registers[y[0]];
            }
            int xValue;
            if (!int.TryParse(x, out xValue))
            {
                if (x.Length != 1)
                    throw new ArgumentException("Received register number not 1 char long");
                xValue = registers[x[0]];
            }

            if (xValue != 0)
                return jumpNumber;
            else
                return 0;
        }

        private void ProcessCopy(Dictionary<char, int> registers, string x, string y)
        {
            // copies x (either an integer or the value of a register) into register y.
            int copyValue;
            if (!int.TryParse(x, out copyValue))
            {
                if (x.Length != 1)
                    throw new ArgumentException("Received register number not 1 char long");
                copyValue = registers[x[0]];
            }
            if (y.Length != 1)
                throw new ArgumentException("Received register number not 1 char long");
            registers[y[0]] = copyValue;
        }

        private List<string> ParseInput(string input)
        {
            List<string> result = new List<string>();

            result.AddRange(input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            return result;
        }
    }
}
