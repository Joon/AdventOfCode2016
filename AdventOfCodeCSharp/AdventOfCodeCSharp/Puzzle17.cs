using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle17
    {

        private string HashTuple(Tuple<int, int, string> toHash)
        {
            return toHash.Item1.ToString() + "," + toHash.Item2.ToString() + "," + toHash.Item3;
        }

        private Tuple<int, int, string> ParseTupleHash(string hash)
        {
            string[] parts = hash.Split(',');
            return new Tuple<int, int, string>(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), parts[2]);
        }

        public string SolvePuzzle(string accessCode)
        {
            int goalX = 4; 
            int goalY = 4;

            int consoleTop = Console.CursorTop;
            Tuple<int, int, string> start = new Tuple<int, int, string>(1, 1, "");
            Tuple<int, int, string> goal = new Tuple<int, int, string>(goalX, goalY, "");

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
                VisualizePuzzle(consoleTop, goalX, goalY, cameFrom);
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

                if (current.Item1 == goalX && current.Item2 == goalY)
                    return current.Item3;

                openSet.Remove(HashTuple(current));
                closedSet.Add(HashTuple(current));

                List<Tuple<int, int, string>> neighbours = CalcNeighbours(current, accessCode);

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

            return "dunno";
        }

        public int SolvePuzzlePart2(string accessCode)
        {
            int goalX = 4;
            int goalY = 4;

            int consoleTop = Console.CursorTop;
            Tuple<int, int, string> start = new Tuple<int, int, string>(1, 1, "");
            Tuple<int, int, string> goal = new Tuple<int, int, string>(goalX, goalY, "");

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

            Dictionary<int, string> solutions = new Dictionary<int, string>();
            int counter = 0;
            while (openSet.Count > 0)
            {
                counter++;
                if (counter == 500)
                {
                    VisualizePuzzle(consoleTop, goalX, goalY, cameFrom);
                    Console.WriteLine("Current highest key: " + solutions.Keys.Max().ToString());
                    Console.WriteLine("Open set: " + openSet.Count.ToString());
                    counter = 0;
                }
                foreach (var open in openSet)
                {
                    if (!fScore.ContainsKey(open))
                        Console.WriteLine("Value (" + open + ") is in the open set but doesn't have a score");
                }

                var findBestScoreInOpenSet = from o in openSet
                                             join n in fScore
                                             on o equals n.Key
                                             select n;
                var current = ParseTupleHash(findBestScoreInOpenSet.OrderBy(o => o.Value).Last().Key);
                                
                openSet.Remove(HashTuple(current));
                closedSet.Add(HashTuple(current));

                if (current.Item1 == goalX && current.Item2 == goalY)
                {
                    solutions[current.Item3.Length] = current.Item3;
                    continue;
                }

                List<Tuple<int, int, string>> neighbours = CalcNeighbours(current, accessCode);

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
                    //else
                    //// This is not a better path.
                    //if (tentative_gScore >= gScore[HashTuple(neighbour)])
                    //    continue;

                    // This path is the best until now. Record it!
                    cameFrom[HashTuple(neighbour)] = HashTuple(current);
                    gScore[HashTuple(neighbour)] = tentative_gScore;
                    fScore[HashTuple(neighbour)] = gScore[HashTuple(neighbour)] +
                        EstimateMoveCost(neighbour, goal);
                }
            }

            return solutions.Keys.Max();
        }

        private void VisualizePuzzle(int consoleTop, int goalX, int goalY, Dictionary<string, string> cameFrom)
        {
            Console.SetCursorPosition(0, consoleTop);
            Console.CursorVisible = false;
            int itemWidth = 1;
            if (goalX >= 8)
                itemWidth = 2;
            Console.Write("".PadLeft(itemWidth));
            for (int i = 0; i < goalX; i++)
            {
                Console.Write(i.ToString().PadLeft(itemWidth));
            }
            Console.WriteLine();
            for (int y = 0; y <= goalY; y++)
            {
                Console.Write(y.ToString().PadLeft(itemWidth));
                for (int x = 0; x < goalX + 10; x++)
                {
                    if (x == goalX && y == goalY)
                        Console.Write('X');
                    else
                    {
                        var findVisits = from i in cameFrom
                                         where i.Key.StartsWith(x.ToString() + "," + y.ToString())
                                         select i;
                        int timesVisited = findVisits.Count();
                        if (timesVisited == 0)
                            Console.Write(".".PadLeft(itemWidth));
                        else
                            Console.Write(timesVisited.ToString().PadLeft(itemWidth));
                    }
                }
                Console.WriteLine();
            }
            Console.CursorVisible = true;
        }

        private int EstimateMoveCost(Tuple<int, int, string> start, Tuple<int, int, string> goal)
        {
            return Math.Abs(goal.Item1 - start.Item1) + Math.Abs(goal.Item2 - start.Item2);
        }

        private List<Tuple<int, int, string>> CalcNeighbours(Tuple<int, int, string> current, string accessCode)
        {
            List<Tuple<int, int, string>> movesToTry = new List<Tuple<int, int, string>>();
            // Move up
            movesToTry.Add(new Tuple<int, int, string>(0, -1, "U"));
            // Move down
            movesToTry.Add(new Tuple<int, int, string>(0, 1, "D"));
            // Move left
            movesToTry.Add(new Tuple<int, int, string>(-1, 0, "L"));
            // Move right
            movesToTry.Add(new Tuple<int, int, string>(1, 0, "R"));

            List<Tuple<int, int, string>> result = new List<Tuple<int, int, string>>();
            foreach(var move in movesToTry)
            {
                Tuple<int, int, string> candidate = new Tuple<int, int, string>(current.Item1 + move.Item1, 
                    current.Item2 + move.Item2, move.Item3);
                if (MoveIsValid(candidate, accessCode, current))
                {
                    result.Add(new Tuple<int, int, string>(candidate.Item1, candidate.Item2, 
                        current.Item3 + candidate.Item3));
                }
            }
            return result;
        }

        private bool MoveIsValid(Tuple<int, int, string> candidate, string accessCode, 
            Tuple<int, int, string> currentPosition)
        {
            // First check that it is in bounds
            if (candidate.Item1 < 1 || candidate.Item2 < 1)
                return false;
            if (candidate.Item1 > 4 || candidate.Item2 > 4)
                return false;

            int x = candidate.Item1;
            int y = candidate.Item2;

            List<char> unlockedValues = new List<char>();
            unlockedValues.Add('b');
            unlockedValues.Add('c');
            unlockedValues.Add('d');
            unlockedValues.Add('e');
            unlockedValues.Add('f');

            // Now check if the door to the room can be opened
            string hash = HashValue(accessCode + currentPosition.Item3);
            switch(candidate.Item3)
            {
                case "U":
                    return unlockedValues.Contains(hash[0]);
                case "D":
                    return unlockedValues.Contains(hash[1]);
                case "L":
                    return unlockedValues.Contains(hash[2]);
                case "R":
                    return unlockedValues.Contains(hash[3]);
            }
            throw new ApplicationException("Invalid move direction supplied");
        }

        private MD5 _hasher = MD5.Create();

        private string HashValue(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = _hasher.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        
        private string MoveDirection(string to, string from)
        {
            Tuple<int, int, string> fromCoord = ParseTupleHash(from);
            Tuple<int, int, string> toCoord = ParseTupleHash(to);

            if (fromCoord.Item1 < toCoord.Item1)
                return "R";
            if (fromCoord.Item1 > toCoord.Item1)
                return "L";
            if (fromCoord.Item2 < toCoord.Item2)
                return "D";
            if (fromCoord.Item2 > toCoord.Item2)
                return "U";
            // Its the same node - return blank
            return "";
        }
    }
}
