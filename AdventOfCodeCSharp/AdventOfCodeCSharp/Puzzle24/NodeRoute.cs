using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle24Assets
{
    public class NodeRoute
    {

        public VisitNode FromNode { get; set; }

        public VisitNode ToNode { get; set; }

        public int DistanceTravelled { get; set; }

    }
}
