using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Puzzle number: ");
            string puzzle = Console.ReadLine();

            string input;
            if (args.Length > 0 && args[0] == "i")
            {
                Console.WriteLine("Interactive puzzle input: ");
                input = Console.ReadLine().Replace("|", Environment.NewLine);
            }
            else
            {
                if (puzzle.EndsWith("b"))
                    input = File.ReadAllText(string.Format(@"c:\Work\AdventOfCode\Inputs\Input{0}.txt", puzzle.Substring(0, puzzle.Length - 1)));
                else
                    input = File.ReadAllText(string.Format(@"c:\Work\AdventOfCode\Inputs\Input{0}.txt", puzzle));
            }

            int intAnswer = -9999;
            string stringAnswer = "";
            switch (puzzle)
            {
                case "1":
                    var puzzleImpl = new Puzzle1();
                    intAnswer = puzzleImpl.ProcessPuzzle(input);
                    break;
                case "1b":
                    var puzzleImpl1b = new Puzzle1b();
                    intAnswer = puzzleImpl1b.ProcessPuzzle(input);
                    break;
                case "2":
                    var puzzleImpl2 = new Puzzle2();
                    stringAnswer = puzzleImpl2.ProcessPuzzle(input);
                    break;
                case "2b":
                    var puzzleImpl2b = new Puzzle2b();
                    stringAnswer = puzzleImpl2b.ProcessPuzzle(input);
                    break;
                case "3":
                    var puzzle3 = new Puzzle3();
                    intAnswer = puzzle3.ProcessPuzzle(input);
                    break;
                case "3b":
                    var puzzle3b = new Puzzle3b();
                    intAnswer = puzzle3b.ProcessPuzzle(input);
                    break;
                case "4":
                    var puzzle4 = new Puzzle4();
                    intAnswer = puzzle4.ProcessPuzzle(input);
                    break;
            }
            if (!string.IsNullOrEmpty(stringAnswer))
            {
                Console.WriteLine("Processing complete. Answer: " + stringAnswer);
            }
            else
            {
                Console.WriteLine("Processing complete. Answer: " + intAnswer.ToString());
            }

            Console.ReadLine();
        }
    }
}
