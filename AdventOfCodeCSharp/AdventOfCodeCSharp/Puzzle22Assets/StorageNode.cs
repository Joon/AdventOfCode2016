using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle22Assets
{
    public class StorageNode
    {
        public string NodeName { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Size { get; set; }

        public int StartSpaceUsed { get; set; }

        public int StartSpaceAvailable { get; set; }

        public StorageNode Left { get; set; }

        public StorageNode Top { get; set; }

        public StorageNode Right { get; set; }

        public StorageNode Bottom { get; set; }

        public int SpaceAvailable(Dictionary<string, int> storageState)
        {
            int used = storageState[NodeName];
            return Size - used;
        }

        public int SpaceUsed(Dictionary<string, int> storageState)
        {
            int used = storageState[NodeName];
            return used;
        }
        
    }
}
