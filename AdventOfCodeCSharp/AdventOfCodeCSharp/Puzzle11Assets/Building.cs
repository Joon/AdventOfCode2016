using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class Building
    {
        private List<Floor> _floors;

        public int ElevatorOn { get; set; }

        public List<Floor> Floors { get { return _floors; } }

        public Building()
        {
            _floors = new List<Floor>();
        }

        public void ConstructBuilding(int floorcount)
        {
            for(int i = 1; i < floorcount; i++)
            {
                Floor f = new Floor();
                f.FloorNumber = i;
                Floors.Add(f);
            }
        }

        public Building Clone()
        {
            Building result = new Building();
            foreach(Floor f in Floors)
            {
                Floor copyF = f.Clone();
                result.Floors.Add(copyF);
            }
            result.ElevatorOn = ElevatorOn;
            return result;
        }

        /// <summary>
        /// Checks if the building is solved. A building is solved when all
        /// microchips and generators are on the top floor
        /// </summary>
        /// <returns></returns>
        public bool BuildingSolved()
        {
            bool result = false;

            if (_floors[_floors.Count - 1].Generators.Count > 0 &&
                _floors[_floors.Count - 1].MicroChips.Count > 0)
                result = true;

            for(int i = _floors.Count - 2; i >= 0; i--)
            {
                if (_floors[i].Generators.Count > 0 ||
                        _floors[i].MicroChips.Count > 0)
                    result = false;
            }

            return result;
        }

        int[] microchipHashKeys = { 1, 2, 4, 8, 16, 32, 64, 128 };
        int[] generatorHashKeys = { 256, 512, 1024, 2048, 4096, 8192, 16384 };
        int elevatorPresentKey = 32768;

        public long Hash()
        {
            long result = 0;
            // We hash the floors using two bytes per floor (one for generators, one for microchips) with one of the generator bits used
            // to store the elevator present flag
            // 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
            // |             f4               |               f3              |              f2                |              f1
            // e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m   
            for (int floor = 0; floor < 4; floor++)
            {
                long floorHash = 0;
                Floor floorToHash = Floors[floor];
                foreach (Microchip m in floorToHash.MicroChips)
                {
                    floorHash = floorHash + microchipHashKeys[m.MicrochipNumber - 1];
                }

                foreach (Generator g in floorToHash.Generators)
                {
                    floorHash = floorHash + generatorHashKeys[g.GeneratorNumber - 1];
                }
                if (ElevatorOn == floor + 1)
                    floorHash += elevatorPresentKey;
                long floorAdjustmentMultiplier = (long)Math.Pow(65536, floor);
                result = result + (floorHash * floorAdjustmentMultiplier);
            }
            return result;
        }

        public string HashString()
        {
            string result = ElevatorOn.ToString();
            foreach (Floor f in Floors)
            {
                result += f.Hash;
            }
            return result;
        }


    }
}
