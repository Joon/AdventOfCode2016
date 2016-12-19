using AdventOfCodeCSharp.Puzzle11Assets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle11
    {

        public int CalcShortestSolution()
        {
            BuildingMaker maker = new BuildingMaker();
            Building startState = maker.BuildingForPuzzleInput();
            MoveMaker mover = new MoveMaker();
            List<BuildingMove> possibleSolutions = new List<BuildingMove>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int lowestSolution = mover.CalcMoveDepth(startState);
            watch.Stop();
            Console.WriteLine("It took " + watch.Elapsed.ToString() + " to process");
            /*foreach (BuildingMove bm in possibleSolutions)
            {
                if (bm.CommandTreeSolved())
                {
                    int solutionDepth = bm.ShortestSolvedPath();
                    if (solutionDepth < lowestSolution)
                        lowestSolution = solutionDepth;
                }
            }*/
            return lowestSolution;
        }

    }
}
