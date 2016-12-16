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
        /// <summary>
        /// Calculate one level of expansion instructions
        /// </summary>
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

        /// <summary>
        /// Calculates all expanded text. Instead of performing the expansion, a 
        /// tree of expansion instructions is built. Then we calculate the actual
        /// file length by calculating the effect of all the expansions
        /// </summary>
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

        /// <summary>
        /// Recursively calculates the instruction length
        /// </summary>
        private long CalcExpandedInstructionLength(FileCompressionInstruction rootInstruction)
        {
            // Get the length of the clean text in this instruction
            long result = rootInstruction.CharCount;

            // recurse down the hierarchy, adding up all expanded values as we go
            foreach(var childInstruction in rootInstruction.UnpackedInstructions)
            {
                result += CalcExpandedInstructionLength(childInstruction);
            }

            // Now take the hierarchy length, and multiply it with this item's count of repeats
            return result * rootInstruction.Repeats;
        }

        /// <summary>
        /// Recursively expands any instructions left in the sourcetext of the supplied instruction.
        /// </summary>
        private void ExpandInstructions(FileCompressionInstruction rootInstruction)
        {
            foreach(FileCompressionInstruction instruction in rootInstruction.UnpackedInstructions)
            {
                string cleanedSource;
                ParseFileInstructions(instruction.SourceText, instruction.UnpackedInstructions, out cleanedSource);
                // Update the source text, child instructions are now kept. note that repeats stay the same
                instruction.SourceText = cleanedSource;
                // Character count has to be kept up to date
                instruction.CharCount = cleanedSource.Length;
                ExpandInstructions(instruction);
            }
        }

        /// <summary>
        /// Take a string that may contain expanding instructions 
        /// Example 1: "(1x3)a" becomes "", and an instruction is returned that indicates that A 
        /// should be multipled 3 times
        /// Example 2: "pp(5x2)(1x3)f" becomes "pp", and an instruction is returned that indicates 
        ///            that "(1x3)f should be repeated 2 times
        ///            
        /// Note that we don't attempt to unpack all the files, instead we reprocess our recursive data structure 
        /// in a controlling routine to unpack the child instructions
        /// </summary>
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
