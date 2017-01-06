using AdventOfCodeCSharp.Puzzle22Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle22
    {

        public int SolvePuzzle(string input)
        {
            /*root @ebhq-gridcenter# df -h
            Filesystem               Size Used  Avail  Use %
            /dev/grid/node-x0-y0     86T   73T    13T   84%
            /dev/grid/node-x0-y1     88T   65T    23T   73% */

            List<StorageNode> nodes = new List<StorageNode>();
            ParseInputToNodes(input, nodes);

            int viableNodeCount = 0;
            HashSet<string> viableNodes = new HashSet<string>();
            foreach (StorageNode n in nodes)
            {
                foreach (StorageNode n2 in nodes)
                {
                    if (n == n2)
                        continue;

                    // Have we maybe found this combination before?
                    if (viableNodes.Contains(n.NodeName + n2.NodeName))
                        continue;
                    if (viableNodes.Contains(n2.NodeName + n.NodeName))
                        continue;

                    if (n.StartSpaceUsed > 0 && n.StartSpaceUsed <= n2.StartSpaceAvailable)
                    {
                        viableNodes.Add(n.NodeName + n2.NodeName);
                        viableNodeCount++;
                    }
                }
            }
            return viableNodeCount;
        }

        private static void ParseInputToNodes(string input, List<StorageNode> nodes)
        {
            foreach (string nodeLine in input.Split(Environment.NewLine.ToCharArray(),
                            StringSplitOptions.RemoveEmptyEntries))
            {
                string[] nodeComponents = nodeLine.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                StorageNode node = new StorageNode();
                node.NodeName = nodeComponents[0];
                string[] nameParts = nodeComponents[0].Split('-');
                node.X = Convert.ToInt32(nameParts[1].Substring(1));
                node.Y = Convert.ToInt32(nameParts[2].Substring(1));
                node.Size = Convert.ToInt32(nodeComponents[1].Substring(0, nodeComponents[1].Length - 1));
                node.StartSpaceUsed = Convert.ToInt32(nodeComponents[2].Substring(0, nodeComponents[2].Length - 1));
                node.StartSpaceAvailable = Convert.ToInt32(nodeComponents[3].Substring(0, nodeComponents[3].Length - 1));
                nodes.Add(node);
            }

            foreach(StorageNode n in nodes)
            {
                foreach(StorageNode n2 in nodes)
                {
                    if (n2.X == (n.X + 1) && n.Y == n2.Y)
                        n.Right = n2;
                    if (n2.X == (n.X - 1) && n.Y == n2.Y)
                        n.Left= n2;

                    if (n2.Y == (n.Y + 1) && n.X == n2.X)
                        n.Bottom = n2;
                    if (n2.Y == (n.Y - 1) && n.X == n2.X)
                        n.Top = n2;
                }
            }
        }

        public int SolvePuzzlePart2(string input)
        {
            List<StorageNode> nodes = new List<StorageNode>();
            ParseInputToNodes(input, nodes);
            PuzzleSolver solver = new PuzzleSolver(nodes);
            return solver.ShortestPath();
        }
    }
}
