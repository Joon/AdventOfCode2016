using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle24Assets
{
    public class FloorPlan
    {

        List<Tuple<int, int>> _obstructedSquares = new List<Tuple<int, int>>();
        

        public void RecordObstruction(int x, int y)
        {
            _obstructedSquares.Add(new Tuple<int, int>(x, y));
        }

        public bool CanModeTo(int x, int y)
        {
            var findObstruction = from square in _obstructedSquares
                                  where square.Item1 == x && square.Item2 == y
                                  select square;
            return findObstruction.Count() == 0;
        }

    }
}
