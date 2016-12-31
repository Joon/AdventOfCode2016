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

        public void VisualizePuzzle2()
        {
            for (int i = 1; i <= 1000; i++)
            {
                int bruteAnswer = SolvePuzzle2BruteForce(i.ToString());
                int formulaAnswer = SolvePuzzle2ByFormula(i.ToString());
                Console.WriteLine(i.ToString() + '\t' + bruteAnswer.ToString() + '\t' + formulaAnswer.ToString());
            }
        }

        /// <summary>
        /// After visualizing the puzzle output from 1 to 1000, a bit of excel magic came up with
        /// this formula (valid for any integer >= 2):
        /// let numberToSolve equal the input value to solve
        /// let largestPower equal the largest power of 3 less than or equal to numberToSolve
        /// when largestPower == numberToSolve, solution = largestPower
        /// let diffToLargestPower = numberToSolve - largestPower
        /// when diffToLargestPower <= largestPower, solution = diffToLargestPower
        /// Once the diffToLargestPower is greater than the largestPower, the values start to count in 2's. The data looks like this:
        /// numberToSolve largestPower  solution 
        /// 15	          9              6	       
        /// 16	          9              7        
        /// 17            9              8        
        /// 18            9              9        
        /// 19      	  9             11        
        /// 20      	  9             13        
        /// 21      	  9             15        
        /// The formula to count in 2's once past the largest power looks like this:
        /// when diffToLargestPower > largestPower, solution = largestPower+(numberToSolve-(largestPower*2))*2
        /// </summary>
        public int SolvePuzzle2ByFormula(string input)
        {
            int numberToSolve = Convert.ToInt32(input);

            if (numberToSolve == 1)
                return 1;

            List<int> powersOf3 = new List<int>();
            int largestPower = 1;
            int candidatePowerOfThree = 1;
            while (candidatePowerOfThree <= numberToSolve)
            {
                candidatePowerOfThree = candidatePowerOfThree * 3;
                if (candidatePowerOfThree <= numberToSolve)
                    largestPower = candidatePowerOfThree;
            }

            if (numberToSolve == largestPower)
                return numberToSolve;

            int diffToLargestPower = numberToSolve - largestPower;

            if (diffToLargestPower <= largestPower)
                return diffToLargestPower;

            return largestPower + (numberToSolve - (largestPower * 2)) * 2;
        }

        public int SolvePuzzle2BruteForce(string input)
        {
            int elfCount = Convert.ToInt32(input);

            List<int> elves = new List<int>();
            for(int i = 1; i <= elfCount; i++)
            {
                elves.Add(i);
            }

            int consoleTop = Console.CursorTop;

            while (elves.Count > 1)
            {
                if (elves.Count % 1000 == 0)
                {
                    Console.SetCursorPosition(0, consoleTop);
                    Console.Write(elves.Count.ToString().PadRight(10));
                }
                int i = 0;
                while (i < elves.Count)
                {
                    // integer division forces the bias to the left when there isn't a directly "opposite" elf
                    int middleIndex = (elves.Count / 2) ;
                    if (i + middleIndex > elves.Count - 1)
                    {
                        int removeIndex = i + middleIndex - elves.Count;
                        //Console.WriteLine(elves[i].ToString() + " removes " + elves[removeIndex].ToString());
                        elves.RemoveAt(removeIndex);
                    }
                    else
                    {
                        //Console.WriteLine(elves[i].ToString() + " removes " + elves[i + middleIndex].ToString());
                        elves.RemoveAt(i + middleIndex);
                        i++;
                    }                    
                }
            }
            return elves[0];
        }

    }
}
