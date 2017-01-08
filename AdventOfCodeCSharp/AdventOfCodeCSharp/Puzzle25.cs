using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{



    public class Puzzle25
    {
        
        public int SolvePuzzle(string input)
        {
            for(int i = 1; i < 10000; i++)
            {
                List<int> broadcast = new List<int>();
                RunCode(input, i, delegate (int broadcastValue)
                {
                    broadcast.Add(broadcastValue);
                    int? previous = null;
                    foreach(int val in broadcast)
                    {
                        if (val != 0 && val != 1)
                        {
                            return false;
                        }
                        if (previous != null)
                        {
                            if (previous.Value == val)
                                return false;
                        }
                        previous = val;
                    }
                    if (broadcast.Count > 500)
                        return false;
                    return true;
                });
                if (broadcast.Count > 500)
                    return i;
            }
            return -1;
        }

        public void RunCode(string input, int initialValue, Func<int, bool> broadcastCallback)
        {
            List<string> instructions = ParseInput(input);

            Dictionary<char, int> registers = new Dictionary<char, int>();

            foreach (char c in "abcd")
            {
                registers[c] = 0;
            }

            registers['a'] = initialValue;

            int consoleTop = Console.CursorTop;
            int instructionCount = 0;
            int processInstruction = 0;
            while (processInstruction <= instructions.Count - 1)
            {
                instructionCount++;
                if (instructionCount % 50000 == 0)
                {
                    Console.SetCursorPosition(0, consoleTop);
                    Console.WriteLine("Instructions: (* is the next one to exec)");
                    for (int i = 0; i < instructions.Count; i++)
                    {
                        string instruction = instructions[i];
                        Console.WriteLine((i == processInstruction ? "*" : " ") + instruction);
                    }

                    Console.SetCursorPosition(42, consoleTop);
                    Console.Write("Registers:");
                    for (int i = 0; i < registers.Count; i++)
                    {
                        Console.SetCursorPosition(42, consoleTop + 1 + i);
                        Console.Write(registers.ElementAt(i).Key + ": " + registers.ElementAt(i).Value.ToString().PadRight(5));
                    }
                }
                //Console.ReadLine();

                string[] instructionPortions = instructions[processInstruction].Split(' ');
                switch (instructionPortions[0])
                {
                    case "tgl":
                        ProcessToggle(registers, instructionPortions[1], instructions, processInstruction);
                        break;
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
                    case "out":
                        int value = registers[instructionPortions[1][0]];
                        if (!broadcastCallback(value))
                            return;
                        break;
                }
                processInstruction++;
            }
        }

        private void ProcessToggle(Dictionary<char, int> registers, string v, List<string> instructions, int processInstruction)
        {
            int instructionToggleDex = 0;
            if (!int.TryParse(v, out instructionToggleDex))
                instructionToggleDex = registers[v[0]];
            int toggleInstructionAt = processInstruction + instructionToggleDex;
            if (toggleInstructionAt < 0 || toggleInstructionAt >= instructions.Count)
                return;

            string instructionToToggle = instructions[toggleInstructionAt];
            string[] toggleInstructionParts = instructionToToggle.Split(' ');
            // Assembunny jnz becomes cpy
            if (toggleInstructionParts[0] == "jnz")
            {
                string[] jnzInstructionParts = new string[] { "cpy", toggleInstructionParts[1], toggleInstructionParts[2] };
                instructions[toggleInstructionAt] = string.Join(" ", jnzInstructionParts);
                return;
            }
            // Assembunny cpy becomes jnz
            if (toggleInstructionParts[0] == "cpy")
            {
                string[] cpyInstructionParts = new string[] { "jnz", toggleInstructionParts[1], toggleInstructionParts[2] };
                instructions[toggleInstructionAt] = string.Join(" ", cpyInstructionParts);
                return;
            }
            string[] newInstructionParts = new string[] { "", toggleInstructionParts[1] };
            // inc and dec are swapped, tgl becomes inc
            switch (toggleInstructionParts[0])
            {
                case "inc":
                    newInstructionParts[0] = "dec";
                    break;
                case "tgl":
                case "dec":
                    newInstructionParts[0] = "inc";
                    break;
            }
            instructions[toggleInstructionAt] = string.Join(" ", newInstructionParts);
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
            int dummy;
            // Skip invald copy instructions
            if (int.TryParse(y, out dummy))
                return;
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
