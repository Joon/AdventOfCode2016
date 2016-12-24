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
            //Building startState = maker.TestPuzzleInput();
            Building startState = maker.BuildingForPuzzleInput();
            Building endState = maker.SolvedBuilding(startState);
            MoveMaker mover = new MoveMaker();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            
            int lowestLongSolution = mover.CalcMoveDepth(startState, endState);
            watch.Stop();
            Console.WriteLine("It took " + watch.Elapsed.ToString() + " to process, with result " + lowestLongSolution);
            
            return lowestLongSolution;
        }

        public int CalcAStarSolution()
        {
            BuildingMaker maker = new BuildingMaker();
            //Building startState = maker.TestPuzzleInput();
            Building startState = maker.BuildingForPuzzleInput();
            Building endState = maker.SolvedBuilding(startState);
            MoveMaker mover = new MoveMaker();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            int lowestLongSolution = mover.CalcMoveDepthAStar(startState, endState);
            watch.Stop();
            Console.WriteLine("It took " + watch.Elapsed.ToString() + " to process, with result " + lowestLongSolution);

            return lowestLongSolution;
        }

        public int CalcShortestSolutionWithExtra()
        {
            BuildingMaker maker = new BuildingMaker();
            Building startState = maker.BuildingForPuzzle2Input();
            Building endState = maker.SolvedBuilding(startState);
            MoveMaker mover = new MoveMaker();
            
            List<BuildingMove> possibleSolutions = new List<BuildingMove>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int lowestSolution = mover.CalcMoveDepthAStar(startState, endState);
            watch.Stop();
            Console.WriteLine("It took " + watch.Elapsed.ToString() + " to process");
            return lowestSolution;
        }

    }
}
