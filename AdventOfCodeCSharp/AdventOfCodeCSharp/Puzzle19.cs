using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle19
    {

        /// <summary>
        /// Josephus problem - can be solved by manipulating the binary value because
        /// the problem itself has a binary nature. See https://www.youtube.com/watch?v=uCsD3ZGzMgE
        /// </summary>
        public int SolvePuzzle(string input)
        {
            // 3014603
            int elfCount = Convert.ToInt32(input);

            string binaryRepresentation = Convert.ToString(elfCount, 2);
            string answerBinary = binaryRepresentation.Substring(1) + "1";
            return Convert.ToInt32(answerBinary, 2);
        }

    }
}
