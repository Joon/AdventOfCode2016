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

            string input = "";
            if (args.Length > 0 && args[0] == "i")
            {
                Console.WriteLine("Interactive puzzle input: ");
                input = Console.ReadLine().Replace("|", Environment.NewLine);
            }
            else
            {
                string fileName;
                if (puzzle.EndsWith("b") || puzzle.EndsWith("a"))
                    fileName = string.Format(@"c:\Work\AdventOfCode\Inputs\Input{0}.txt", puzzle.Substring(0, puzzle.Length - 1));
                else
                    fileName = string.Format(@"c:\Work\AdventOfCode\Inputs\Input{0}.txt", puzzle);
                if (File.Exists(fileName))
                    input = File.ReadAllText(fileName);
            }

            int intAnswer = -9999;
            uint uintAnswer = 0;
            long reallyBigIntAnswer = -9999;
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
                case "9":
                    var puzzle9 = new Puzzle9();
                    intAnswer = puzzle9.CalculateFileLength(input);
                    break;
                case "9b":
                    var puzzle9b = new Puzzle9();
                    reallyBigIntAnswer = puzzle9b.CalculateBetaProtocolFileLength(input);
                    break;
                case "10":
                    var puzzle10 = new Puzzle10();
                    intAnswer = puzzle10.ProcessPuzzle(input);
                    break;
                case "10b":
                    var puzzle10b = new Puzzle10();
                    intAnswer = puzzle10b.ProcessPuzzleOutputs(input);
                    break;
                case "11":
                    var puzzle11 = new Puzzle11();
                    intAnswer = puzzle11.CalcShortestSolution();
                    break;
                case "11a":
                    var puzzle11a = new Puzzle11();
                    intAnswer = puzzle11a.CalcAStarSolution();
                    break;
                case "11b":
                    var puzzle11b = new Puzzle11();
                    intAnswer = puzzle11b.CalcShortestSolutionWithExtra();
                    break;
                case "12":
                    var puzzle12 = new Puzzle12();
                    intAnswer = puzzle12.SolvePuzzle(input, false);
                    break;
                case "12b":
                    var puzzle12b = new Puzzle12();
                    intAnswer = puzzle12b.SolvePuzzle(input, true);
                    break;
                case "13":
                    var puzzle13 = new Puzzle13();
                    //intAnswer = puzzle13.SolvePuzzle(10, 7, 4);
                    intAnswer = puzzle13.SolvePuzzle(1352, 31, 39);
                    break;
                case "13b":
                    var puzzle13b = new Puzzle13();
                    //intAnswer = puzzle13.SolvePuzzle(10, 7, 4);
                    intAnswer = puzzle13b.SolvePart2(50, 1352);
                    break;
                case "14":
                    var puzzle14 = new Puzzle14();
                    intAnswer = puzzle14.SolvePuzzle("jlmsuwbz");
                    break;
                case "14b":
                    var puzzle14b = new Puzzle14();
                    intAnswer = puzzle14b.SolvePuzzlePart2("jlmsuwbz");
                    break;
                case "15":
                    var puzzle15 = new Puzzle15();
                    intAnswer = puzzle15.SolvePuzzle(input);
                    break;
                case "15b":
                    var puzzle15b = new Puzzle15();
                    intAnswer = puzzle15b.SolvePuzzlePart2(input);
                    break;
                case "16":
                    var puzzle16 = new Puzzle16();
                    stringAnswer = puzzle16.SolvePuzzle(input, 272);
                    break;
                case "16b":
                    var puzzle16b = new Puzzle16();
                    stringAnswer = puzzle16b.SolvePuzzle(input, 35651584);
                    break;
                case "17":
                    var puzzle17 = new Puzzle17();
                    stringAnswer = puzzle17.SolvePuzzle(input);
                    break;
                case "17b":
                    var puzzle17b = new Puzzle17();
                    intAnswer = puzzle17b.SolvePuzzlePart2(input);
                    break;
                case "18":
                    var puzzle18 = new Puzzle18();
                    intAnswer = puzzle18.SolvePuzzle(input);
                    break;
                case "18b":
                    var puzzle18b = new Puzzle18();
                    intAnswer = puzzle18b.SolvePuzzlePart2(input);
                    break;
                case "19":
                    var puzzle19 = new Puzzle19();
                    intAnswer = puzzle19.SolvePuzzle("3014603");
                    break;
                case "19b":
                    var puzzle19b = new Puzzle19();
                    intAnswer = puzzle19b.SolvePuzzle2ByFormula("3014603");
                    break;
                case "20":
                    var puzzle20 = new Puzzle20();
                    uintAnswer = puzzle20.SolvePuzzle(input);
                    break;
                case "20b":
                    var puzzle20b = new Puzzle20();
                    uintAnswer = puzzle20b.SolvePuzzle2(input);
                    break;
                case "21":
                    var puzzle21 = new Puzzle21();
                    stringAnswer = puzzle21.SolvePuzzle(input, "abcdefgh");
                    break;
                case "21b":
                    var puzzle21b = new Puzzle21();
                    stringAnswer = puzzle21b.SolvePuzzlePart2(input, "fbgdceah");
                    break;
                case "22":
                    var puzzle22 = new Puzzle22();
                    intAnswer = puzzle22.SolvePuzzle(input);
                    break;
                case "22b":
                    var puzzle22b = new Puzzle22();
                    intAnswer = puzzle22b.SolvePuzzlePart2(input);
                    break;
            }
            if (!string.IsNullOrEmpty(stringAnswer))
            {
                Console.WriteLine("Processing complete. Answer: " + stringAnswer);
            }
            else if (reallyBigIntAnswer > 0)
            {
                Console.WriteLine("Processing complete. Answer: " + reallyBigIntAnswer.ToString());
            } else if (uintAnswer > 0)
            {
                Console.WriteLine("Processing complete. Answer: " + uintAnswer.ToString());
            } else
            {
                Console.WriteLine("Processing complete. Answer: " + intAnswer.ToString());
            }

            Console.ReadLine();
        }
    }
}
