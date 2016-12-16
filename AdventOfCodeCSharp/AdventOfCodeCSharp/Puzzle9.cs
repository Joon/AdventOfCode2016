using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class FileCompressionInstruction
    {
        public string SourceText { get; set; }
        public int CharCount { get; set; }
        public int Repeats { get; set; }

        private List<FileCompressionInstruction> _unpackedInstructions;
        public List<FileCompressionInstruction>  UnpackedInstructions {get { return _unpackedInstructions; } }

        public FileCompressionInstruction(int charCount, int repeats)
        {
            _unpackedInstructions = new List<FileCompressionInstruction>();
            CharCount = charCount;
            Repeats = repeats;
        }

        public FileCompressionInstruction(string instruction)
        {
            _unpackedInstructions = new List<FileCompressionInstruction>();
            string[] dimensions = instruction.Split('x');
            CharCount = Convert.ToInt32(dimensions[0]);
            Repeats = Convert.ToInt32(dimensions[1]);
        }
    }

    class Puzzle9
    {

        public int CalculateFileLength(string input)
        {
            var instructions = new List<FileCompressionInstruction>();
            string cleanedInput;
            ParseFileInstructions(input, instructions, out cleanedInput);
            // First repeat is already in the string, so sum up any extra repeats
            var countExpansions = from i in instructions
                                  select i.CharCount * (i.Repeats);
            // Length is the cleaned string plus the additional expansions of text
            return cleanedInput.Length + countExpansions.Sum();
        }

        public long CalculateBetaProtocolFileLength(string input)
        {
            string cleanedInput;
            FileCompressionInstruction rootInstruction = new FileCompressionInstruction(1, 1);
            ParseFileInstructions(input, rootInstruction.UnpackedInstructions, out cleanedInput);
            rootInstruction.SourceText = cleanedInput;
            rootInstruction.CharCount = cleanedInput.Length;
            ExpandInstructions(rootInstruction);
            return CalcExpandedInstructionLength(rootInstruction); 
        }

        private long CalcExpandedInstructionLength(FileCompressionInstruction rootInstruction)
        {
            long result = rootInstruction.CharCount;

            foreach(var childInstruction in rootInstruction.UnpackedInstructions)
            {
                result += CalcExpandedInstructionLength(childInstruction);
            }

            return result * rootInstruction.Repeats;
        }

        private void ExpandInstructions(FileCompressionInstruction rootInstruction)
        {
            foreach(FileCompressionInstruction instruction in rootInstruction.UnpackedInstructions)
            {
                string cleanedSource;
                ParseFileInstructions(instruction.SourceText, instruction.UnpackedInstructions, out cleanedSource);
                instruction.SourceText = cleanedSource;
                instruction.CharCount = cleanedSource.Length;
                ExpandInstructions(instruction);
            }
        }

        private void ParseFileInstructions(string input, List<FileCompressionInstruction> instructions, 
            out string cleanedInput)
        {
            bool inInstruction = false;
            StringBuilder output = new StringBuilder();
            StringBuilder instruction = null;
            int currentPosition = -1;
            while (currentPosition < input.Length - 1)
            {
                currentPosition++;
                char c = input[currentPosition];
                if (c == '(')
                {
                    if (inInstruction)
                        throw new ApplicationException("in an instruction and got another ( char. WTF?");
                    instruction = new StringBuilder();
                    inInstruction = true;
                    continue;
                }
                if (c == ')')
                {
                    if (!inInstruction)
                        throw new ApplicationException("Not in an instruction and got a ) char. WTF?");
                    string instructionText = instruction.ToString();
                    FileCompressionInstruction parsedInstruction = new FileCompressionInstruction(instructionText);
                    parsedInstruction.SourceText = input.Substring(currentPosition + 1, 
                        parsedInstruction.CharCount);
                    currentPosition += parsedInstruction.CharCount;
                    instructions.Add(parsedInstruction);
                    inInstruction = false;
                    continue;
                }
                if (inInstruction)
                    instruction.Append(c);
                else
                    output.Append(c);                
            }
            cleanedInput = output.ToString();
        }
    }
}
