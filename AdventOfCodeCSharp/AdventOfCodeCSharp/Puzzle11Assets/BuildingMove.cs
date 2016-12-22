using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class BuildingMove
    {
        public Building StateAfterMove { get; set; }

        public bool MoveSolvesBuilding { get; set; }
        
        public int MoveDepth { get; set; }
    }
}
