using AdventOfCodeCSharp.Puzzle24Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle24
    {

        public int SolvePuzzle(string input)
        {
            string[] lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            FloorPlan floorPlan = new FloorPlan();
            List<VisitNode> placesToVisit = new List<VisitNode>();

            ParseInput(lines, floorPlan, placesToVisit);

            PuzzleController pc = new PuzzleController();
            return pc.SolvePuzzle(floorPlan, placesToVisit);
        }

        private static void ParseInput(string[] lines, FloorPlan floorPlan, List<VisitNode> placesToVisit)
        {
            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    switch (line[x])
                    {
                        case '.':
                            // do nothing
                            break;
                        case '#':
                            floorPlan.RecordObstruction(x, y);
                            break;
                        default:
                            int positionSeq = Convert.ToInt32(line[x].ToString());
                            VisitNode place = new VisitNode();
                            place.IsStartPosition = positionSeq == 0;
                            place.PositionNumber = positionSeq;
                            place.XPosition = x;
                            place.YPosition = y;
                            placesToVisit.Add(place);
                            break;
                    }
                }
            }
        }

        public int SolvePuzzlePart2(string input)
        {
            string[] lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            FloorPlan floorPlan = new FloorPlan();
            List<VisitNode> placesToVisit = new List<VisitNode>();

            ParseInput(lines, floorPlan, placesToVisit);

            PuzzleController pc = new PuzzleController();
            return pc.SolvePuzzle(floorPlan, placesToVisit, true);
        }
    }
}
