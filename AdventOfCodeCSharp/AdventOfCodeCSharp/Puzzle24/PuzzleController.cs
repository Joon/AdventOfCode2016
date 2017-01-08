using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle24Assets
{
    public class PuzzleController
    {

        public int SolvePuzzle(FloorPlan floorPlan, List<VisitNode> nodesToVisit, bool returnToOrigin = false)
        {
            List<NodeRoute> availableRoutes = new List<NodeRoute>();
            foreach(VisitNode fromNode in nodesToVisit)
            {
                foreach(VisitNode toNode in nodesToVisit)
                {
                    if (fromNode == toNode)
                        continue;

                    NodeRoute route = new NodeRoute();
                    route.FromNode = fromNode;
                    route.ToNode = toNode;
                    
                    RouteSolver calcDistance = new RouteSolver(floorPlan);
                    route.DistanceTravelled = calcDistance.DistanceBetween(fromNode, toNode);

                    Console.WriteLine("From {5}: ({0},{1}) To {6}: ({2},{3})  Distance: {4}", route.FromNode.XPosition,
                        route.FromNode.YPosition, route.ToNode.XPosition, route.ToNode.YPosition, route.DistanceTravelled,
                        route.FromNode.PositionNumber, route.ToNode.PositionNumber);
                    availableRoutes.Add(route);
                }
            }

            var startNode = nodesToVisit.Where(n => n.IsStartPosition);
            List<VisitNode> solveList = new List<VisitNode>(nodesToVisit.Except(startNode));
            if (returnToOrigin)
                solveList.Add(startNode.First());
            return ShortestDistanceToAll(startNode.First(), solveList, availableRoutes,
                returnToOrigin);
        }

        /// <summary>
        /// Recursively compares all possible routes and finds the shorted
        /// possible distance
        /// </summary>
        private int ShortestDistanceToAll(VisitNode fromNode, IEnumerable<VisitNode> nodesToVisit, 
            List<NodeRoute> availableRoutes, bool returnToOrigin)
        {
            int result = Int32.MaxValue;
            foreach(VisitNode n in nodesToVisit)
            {
                if (n == fromNode)
                    continue;
                VisitNode[] subtract = new VisitNode[] { n };
                var findRoute = from r in availableRoutes
                                where r.FromNode == fromNode && r.ToNode == n
                                select r;
                int distanceTravelled = findRoute.First().DistanceTravelled;
                if (nodesToVisit.Count() > 1)
                    distanceTravelled += ShortestDistanceToAll(n, nodesToVisit.Except(subtract), 
                        availableRoutes, returnToOrigin);
                else
                {
                    // Sabotage non-origin node scores 
                    if (returnToOrigin && !n.IsStartPosition)
                        return 10000;
                }
                if (distanceTravelled < result)
                    result = distanceTravelled;
            }
            return result;
        }
    }
}
