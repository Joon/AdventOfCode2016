﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle24Assets
{
    public class RouteSolver
    {
        private FloorPlan _floorPlan;

        public RouteSolver(FloorPlan floorPlan)
        {
            _floorPlan = floorPlan;
        }

        private string HashTuple(Tuple<int, int> toHash)
        {
            return toHash.Item1.ToString() + "," + toHash.Item2.ToString();
        }

        private Tuple<int, int> ParseTupleHash(string hash)
        {
            string[] parts = hash.Split(',');
            return new Tuple<int, int>(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
        }

        private void VisualizePuzzle(int consoleTop, int goalX, int goalY, Dictionary<string, string> cameFrom)
        {
            Console.SetCursorPosition(0, consoleTop);
            Console.CursorVisible = false;
            int itemWidth = 1;
            if (goalX >= 8)
                itemWidth = 2;
            Console.Write("".PadLeft(itemWidth));
            for (int i = 0; i < goalX + 2; i++)
            {
                Console.Write(i.ToString().PadLeft(itemWidth));
            }
            Console.WriteLine();
            for (int y = 0; y <= goalY + 5; y++)
            {
                Console.Write(y.ToString().PadLeft(itemWidth));
                for (int x = 0; x < goalX + 10; x++)
                {
                    if (x == goalX && y == goalY)
                        Console.Write('X');
                    else
                    {
                        Tuple<int, int> thisSquare = new Tuple<int, int>(x, y);
                        if (MoveIsValid(new Tuple<int, int>(x, y)))
                        {
                            if (cameFrom.ContainsKey(HashTuple(thisSquare)))
                                Console.Write("o".PadLeft(itemWidth));
                            else
                                Console.Write(".".PadLeft(itemWidth));
                        }
                        else
                        {
                            if (cameFrom.ContainsKey(HashTuple(thisSquare)))
                                Console.Write("?".PadLeft(itemWidth));
                            else
                                Console.Write("#".PadLeft(itemWidth));
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.CursorVisible = true;
        }

        private int EstimateMoveCost(Tuple<int, int> start, Tuple<int, int> goal)
        {
            return Math.Abs(goal.Item1 - start.Item1) + Math.Abs(goal.Item2 - start.Item2);
        }

        private List<Tuple<int, int>> CalcNeighbours(Tuple<int, int> current)
        {
            List<Tuple<int, int>> movesToTry = new List<Tuple<int, int>>();
            // Move up
            movesToTry.Add(new Tuple<int, int>(0, -1));
            // Move down
            movesToTry.Add(new Tuple<int, int>(0, 1));
            // Move left
            movesToTry.Add(new Tuple<int, int>(-1, 0));
            // Move right
            movesToTry.Add(new Tuple<int, int>(1, 0));

            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            foreach (var move in movesToTry)
            {
                Tuple<int, int> candidate = new Tuple<int, int>(current.Item1 + move.Item1,
                    current.Item2 + move.Item2);
                if (MoveIsValid(candidate))
                {
                    result.Add(candidate);
                }
            }
            return result;
        }

        private bool MoveIsValid(Tuple<int, int> candidate)
        {
            if (candidate.Item1 < 0 || candidate.Item2 < 0)
                return false;

            int x = candidate.Item1;
            int y = candidate.Item2;

            return _floorPlan.CanModeTo(x, y);
        }

        private List<string> reconstruct_path(Dictionary<string, string> cameFrom, string current)
        {
            List<string> result = new List<string>();
            result.Add(current);
            string tempCurrent = current;
            while (cameFrom.ContainsKey(tempCurrent))
            {
                tempCurrent = cameFrom[tempCurrent];
                result.Add(tempCurrent);
            }
            return result;
        }

        public int DistanceBetween(VisitNode startNode, VisitNode endNode)
        {
            int consoleTop = Console.CursorTop;
            Tuple<int, int> start = new Tuple<int, int>(startNode.XPosition, startNode.YPosition);
            Tuple<int, int> goal = new Tuple<int, int>(endNode.XPosition, endNode.YPosition);

            List<string> closedSet = new List<string>();
            // The set of currently discovered nodes still to be evaluated.
            // Initially, only the start node is known.
            List<string> openSet = new List<string>();
            openSet.Add(HashTuple(start));

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            Dictionary<string, string> cameFrom = new Dictionary<string, string>();

            // For each node, the cost of getting from the start node to that node.
            Dictionary<string, int> gScore = new Dictionary<string, int>();

            // The cost of going from start to start is zero.
            gScore[HashTuple(start)] = 0;

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            Dictionary<string, int> fScore = new Dictionary<string, int>();
            // For the first node, that value is completely heuristic.
            fScore[HashTuple(start)] = EstimateMoveCost(start, goal);

            while (openSet.Count > 0)
            {
               // VisualizePuzzle(consoleTop, goal.Item1, goal.Item2, cameFrom);

                foreach (var open in openSet)
                {
                    if (!fScore.ContainsKey(open))
                        Console.WriteLine("Value (" + open + ") is in the open set but doesn't have a score");
                }

                var findBestScoreInOpenSet = from o in openSet
                                             join n in fScore
                                             on o equals n.Key
                                             select n;
                var current = ParseTupleHash(findBestScoreInOpenSet.OrderBy(o => o.Value).First().Key);

                if (HashTuple(current) == HashTuple(goal))
                    return reconstruct_path(cameFrom, HashTuple(current)).Count - 1;

                openSet.Remove(HashTuple(current));
                closedSet.Add(HashTuple(current));

                List<Tuple<int, int>> neighbours = CalcNeighbours(current);

                foreach (var neighbour in neighbours)
                {
                    // Ignore the neighbor which is already evaluated.
                    if (closedSet.Contains(HashTuple(neighbour)))
                        continue;
                    // The distance from start to a neighbor
                    int tentative_gScore = gScore[HashTuple(current)] + 1;
                    // Discover a new node
                    if (!openSet.Contains(HashTuple(neighbour)))
                        openSet.Add(HashTuple(neighbour));
                    else
                    // This is not a better path.
                    if (tentative_gScore >= gScore[HashTuple(neighbour)])
                        continue;

                    // This path is the best until now. Record it!
                    cameFrom[HashTuple(neighbour)] = HashTuple(current);
                    gScore[HashTuple(neighbour)] = tentative_gScore;
                    fScore[HashTuple(neighbour)] = gScore[HashTuple(neighbour)] +
                        EstimateMoveCost(neighbour, goal);
                }
            }

            return -1;
        }

    }
}
