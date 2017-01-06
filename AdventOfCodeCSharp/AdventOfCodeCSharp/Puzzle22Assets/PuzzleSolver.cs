using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle22Assets
{
    public class PuzzleSolver
    {

        private List<StorageNode> _nodes;
        private int _lastDistanceFromRoot;
        private int _lastDistanceFromOpen;

        public PuzzleSolver(List<StorageNode> nodes)
        {
            _nodes = nodes;
        }
        
        public int ShortestPath()
        {
            int consoleY = Console.CursorTop;

            StorageState startState = CalcStartState();
                
            // The set of moves already evaluated.
            HashSet<string> closedSet = new HashSet<string>();

            // The set of currently discovered nodes still to be evaluated.
            // Initially, only the start node is known.
            List<StorageState> openSet = new List<StorageState>();
            openSet.Add(startState);

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            Dictionary<string, StorageState> cameFrom = new Dictionary<string, StorageState>();

            // For each node, the cost of getting from the start node to that node.
            Dictionary<string, int> gScore = new Dictionary<string, int>();
            // The cost of going from start to start is zero.
            gScore[startState.HashState()] = 0;

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            Dictionary<string, int> fScore = new Dictionary<string, int>();

            // For the first node, that value is completely heuristic.
            fScore[startState.HashState()] = SolveCostEstimate(startState);
            _lastDistanceFromOpen = 0;
            _lastDistanceFromRoot = 0;
            while (openSet.Count > 0)
            {
                Console.SetCursorPosition(0, consoleY);
                Console.WriteLine("Queue depth: " + openSet.Count + " Hashes processed: " + closedSet.Count + 
                    " Last Open Distance: " + _lastDistanceFromOpen.ToString() + " Last Distance from Solved: " + 
                    _lastDistanceFromRoot.ToString());
                
                var v = from f in openSet
                        join j in fScore
                        on f.HashState() equals j.Key
                        select new { score = j.Value, storageState = f };
                var scores = v.ToList();
                int minValue = scores.Min(o => o.score);

                // the node in openSet having the lowest fScore[] value
                StorageState current = scores.First(o => o.score == minValue).storageState;

                //Console.WriteLine("Chose this state as the best one: " + current.ID);
                //PrintState(current);

                if (current.DesiredDataOnX == 0 && current.DesiredDataOnY == 0)
                    return reconstruct_move_depth(cameFrom, current);

                openSet.Remove(current);
                closedSet.Add(current.HashState());
                ConcurrentQueue<StorageState> neighbours = new ConcurrentQueue<StorageState>();

                //int moveCount = 0;

                CalcAllPossibleValidMoves(current, neighbours);
                foreach (StorageState bm in neighbours)
                {
                    //Console.WriteLine("Possible move #" + (++moveCount).ToString() + " id: " + bm.ID);
                    //PrintState(bm);
                    // Ignore the neighbor which is already evaluated.
                    if (closedSet.Contains(bm.HashState()))
                        continue;
                    string hash = current.HashState();
                    // The distance from start to a neighbor
                    int tentative_gScore = gScore[hash] + 1;
                    // Discover a new node
                    if (!openSet.Contains(bm))
                        openSet.Add(bm);
                    else if (tentative_gScore >= gScore[bm.HashState()])
                        continue; // This is not a better path.

                    // This path is the best until now. Record it!
                    cameFrom[bm.HashState()] = current;
                    gScore[bm.HashState()] = tentative_gScore;
                    fScore[bm.HashState()] = gScore[bm.HashState()] + SolveCostEstimate(bm);
                }
            }
            return -1;
        }

        private void CalcAllPossibleValidMoves(StorageState current, ConcurrentQueue<StorageState> neighbours)
        {
            for (int x = 0; x <= MaxX(); x++)
            {
                for (int y = 0; y <= MaxY(); y++)
                {
                    StorageNode node = FindNode(x, y);
                    // Can we move the node's data left?
                    CheckNodeMove(current, node, node.Left, neighbours);
                    // Can we move the node's data right?
                    CheckNodeMove(current, node, node.Right, neighbours);
                    // Can we move the node's data up?
                    CheckNodeMove(current, node, node.Top, neighbours);
                    // Can we move the node's data down?
                    CheckNodeMove(current, node, node.Bottom, neighbours);
                }
            }
        }

        private void CheckNodeMove(StorageState current, StorageNode fromNode, StorageNode toNode,
            ConcurrentQueue<StorageState> neighbours)
        {
            if (toNode != null)
            {
                if (toNode.SpaceAvailable(current.State) >= fromNode.SpaceUsed(current.State))
                {
                    StorageState moveResult = current.Clone();
                    // Are we moving the goal data?
                    if (fromNode.X == current.DesiredDataOnX && fromNode.Y == current.DesiredDataOnY)
                    {
                        moveResult.DesiredDataOnX = toNode.X;
                        moveResult.DesiredDataOnY = toNode.Y;
                    }
                    if (toNode.X == current.DesiredDataOnX && toNode.Y == current.DesiredDataOnY)
                    {
                        moveResult.DesiredDataOnX = fromNode.X;
                        moveResult.DesiredDataOnY = fromNode.Y;
                    }
                    int tempStorage = fromNode.SpaceUsed(moveResult.State);
                    moveResult.State[fromNode.NodeName] = moveResult.State[toNode.NodeName];
                    moveResult.State[toNode.NodeName] = tempStorage;
                    neighbours.Enqueue(moveResult);
                }
            }
        }

        private int reconstruct_move_depth(Dictionary<string, StorageState> cameFrom, StorageState current)
        {
            Console.WriteLine();

            int result = 0;
            StorageState tempCurrent = current;
            // PrintState(tempCurrent);
            while (cameFrom.ContainsKey(tempCurrent.HashState()))
            {
                Console.WriteLine();
                tempCurrent = cameFrom[tempCurrent.HashState()];
                // PrintState(tempCurrent);
                result++;
            }
            return result;
        }

        private void PrintState(StorageState tempCurrent)
        {
            for (int y = 0; y <= MaxY(); y++)
            {
                StringBuilder line = new StringBuilder();
                for (int x = 0; x <= MaxX(); x++)
                {
                    StorageNode node = FindNode(x, y);
                    string display = "";
                    display = node.SpaceUsed(tempCurrent.State).ToString() + "/" +
                            node.SpaceAvailable(tempCurrent.State).ToString();
                    if (tempCurrent.DesiredDataOnX == x && tempCurrent.DesiredDataOnY == y)
                        display += "(G)";
                        
                    line.Append(display.PadRight(8));
                }
                Console.WriteLine(line.ToString());
            }
        }

        private int SolveCostEstimate(StorageState startState)
        {
            // Number of spots that an open node is away from the current desired state
            int openDistance = 200;
            string openDirection = "";
            for (int x = 0; x < MaxX(); x++)
            {
                if (Math.Abs(startState.DesiredDataOnX) - x > openDistance)
                    break;
                for(int y = 0; y < MaxY(); y++)
                {
                    if (Math.Abs(startState.DesiredDataOnX - x) + Math.Abs(startState.DesiredDataOnY - y) > openDistance)
                        break;
                    StorageNode checkNode = FindNode(x, y);
                    StorageNode sourceNode = FindNode(startState.DesiredDataOnX, startState.DesiredDataOnY);
                    if (checkNode.SpaceAvailable(startState.State) > sourceNode.SpaceUsed(startState.State))
                    {
                        openDirection = "";
                        if (checkNode.X < sourceNode.X)
                            openDirection += "L";
                        if (checkNode.X > sourceNode.X)
                            openDirection += "R";
                        if (checkNode.Y < sourceNode.Y)
                            openDirection += "U";
                        if (checkNode.Y > sourceNode.Y)
                            openDirection += "D";
                        openDistance = Math.Abs(startState.DesiredDataOnX - x) + Math.Abs(startState.DesiredDataOnY - y);
                        if (openDistance < 2)
                        {
                            // Double open distance value if it is close to the node and in the wrong direction. Prevents false positives
                            // from being processed
                            if (openDirection.Contains("R") || openDirection.Contains("D"))
                                openDistance = openDistance + openDistance;
                        }
                    }
                }
            }

            // number of spots that the desired state is away from the root note
            int distanceFromRoot = startState.DesiredDataOnX + startState.DesiredDataOnY;
            _lastDistanceFromOpen = openDistance;
            _lastDistanceFromRoot = distanceFromRoot;            

            // score increases exponentialy with distance
            return (openDistance * openDistance) + (distanceFromRoot * distanceFromRoot);
        }

        private StorageNode FindNode(int x, int y)
        {
            var findNode = from n in _nodes
                           where n.X == x && n.Y == y
                           select n;
            return findNode.First();
        }

        private int MaxY()
        {
            var findMaxY = from n in _nodes
                           select n.Y;
            return findMaxY.Max();
        }

        private int MaxX()
        {
            var findMaxX = from n in _nodes
                           select n.X;
            return findMaxX.Max();
        }

        private StorageState CalcStartState()
        {
            StorageState result = new StorageState(_nodes);
            foreach (var n in _nodes)
            {
                result.State[n.NodeName] = n.StartSpaceUsed;
            }
            
            result.DesiredDataOnX = MaxX();
            result.DesiredDataOnY = 0;

            return result;
        }
    }
}
