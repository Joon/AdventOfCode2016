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
                case "4b":
                    var puzzle4b = new Puzzle4b();
                    stringAnswer = puzzle4b.ProcessPuzzle(input);
                    break;
                case "5":
                    var puzzle5 = new Puzzle5();
                    stringAnswer = puzzle5.ProcessPuzzle(input);
                    break;
                case "5b":
                    var puzzle5b = new Puzzle5b();
                    stringAnswer = puzzle5b.ProcessPuzzle(input);
                    break;
                case "6":
                    var puzzle6 = new Puzzle6();
                    stringAnswer = puzzle6.ProcessPuzzle(input);
                    break;
                case "6b":
                    var puzzle6b = new Puzzle6();
                    stringAnswer = puzzle6b.ProcessPuzzleB(input);
                    break;
                case "7":
                    var puzzle7 = new Puzzle7();
                    intAnswer = puzzle7.ProcessPuzzle(input);
                    break;
                case "7b":
                    var puzzle7b = new Puzzle7();
                    intAnswer = puzzle7b.ProcessPuzzleB(input);
                    break;
                case "8":
                    var puzzle8 = new Puzzle8();
                    intAnswer = puzzle8.CalculatePuzzle(input);
                    break;
                case "8b":
                    var puzzle8b = new Puzzle8();
                    stringAnswer = puzzle8b.ParsePuzzle(input);
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
