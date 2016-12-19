using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class BuildingMove
    {
        private List<BuildingMove> _subsequentMoves;

        public BuildingMove()
        {
            _subsequentMoves = new List<BuildingMove>();
        }

        public Floor StartFloor { get; set; }
        public Floor EndFloor { get; set; }

        public List<BuildingMove> SubsequentMoves { get { return _subsequentMoves; } }

        public Building StateAfterMove { get; set; }

        public bool MoveSolvesBuilding { get; set; }

        public int MoveDepth { get; set; }

        public bool CommandTreeSolved()
        {
            if (MoveSolvesBuilding)
                return true;

            foreach(var command in SubsequentMoves)
            {
                if (command.CommandTreeSolved())
                    return true;
            }

            return false;
        }

        public int ShortestSolvedPath()
        {
            if (SubsequentMoves.Count == 0)
                return MoveSolvesBuilding ? 1 : 5000;

            int shortestPath = 5000;
            foreach(var move in SubsequentMoves)
            {
                int candidatePath = move.ShortestSolvedPath() + 1;
                if (candidatePath < shortestPath)
                    shortestPath = candidatePath; 
            }
            return shortestPath;
        }
    }
}
