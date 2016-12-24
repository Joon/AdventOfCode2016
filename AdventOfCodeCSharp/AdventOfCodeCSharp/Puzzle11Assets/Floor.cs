using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class Floor
    {
        private int _floorState;

        int[] microchipHashKeys = { 1, 2, 4, 8, 16, 32, 64, 128 };
        int[] generatorHashKeys = { 256, 512, 1024, 2048, 4096, 8192, 16384 };

        public IEnumerable<int> Generators
        {
            get
            {
                int[] result = new int[7];
                int lastPos = -1;
                for (int i = 1; i <= 7; i++)
                {
                    if ((_floorState & generatorHashKeys[i - 1]) == generatorHashKeys[i - 1])
                    {
                        result[++lastPos] = i;
                    }
                }
                return result.Take(lastPos >= 0 ? lastPos + 1 : 0);
            }
        }

        public void AddGenerator(int generator)
        {
            _floorState = _floorState | generatorHashKeys[generator - 1];
        }
        
        public void RemoveGenerator(int generator)
        {
            _floorState = _floorState ^ generatorHashKeys[generator - 1];
        }

        public IEnumerable<int> MicroChips {
            get
            {
                int[] result = new int[7];
                int lastPos = -1;
                for (int i = 1; i <= 7; i++)
                {
                    if ((_floorState & microchipHashKeys[i - 1]) == microchipHashKeys[i - 1])
                    {
                        result[++lastPos] = i;
                    }
                }
                return result.Take(lastPos >= 0 ? lastPos + 1 : 0);
            }
        }

        public void AddMicrochip(int microChip)
        {
            _floorState = _floorState + microchipHashKeys[microChip - 1];
        }

        public void RemoveMicrochip(int microChip)
        {
            _floorState = _floorState - microchipHashKeys[microChip - 1];
        }

        public int FloorNumber { get; set; }
        
        internal Floor Clone()
        {
            Floor result = new Floor();
            result._floorState = _floorState;
            result.FloorNumber = FloorNumber;
            return result;
        }

        public int Hash
        {
            get
            {
                return _floorState;
            }
        }

        internal bool ContainsGenerator(int mc1)
        {
            return (_floorState & generatorHashKeys[mc1 - 1]) == generatorHashKeys[mc1 - 1];
        }

        internal bool ContainsChip(int i)
        {
            return (_floorState & microchipHashKeys[i - 1]) == microchipHashKeys[i - 1];
        }
    }
}
