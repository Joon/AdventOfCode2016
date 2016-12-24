using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class Building : IEquatable<Building>
    {
        private List<Floor> _floors;

        public int ElevatorOn { get; set; }

        public List<Floor> Floors { get { return _floors; } }

        public Building()
        {
            _floors = new List<Floor>();
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

            if (_floors[_floors.Count - 1].Generators.Count() > 0 &&
                _floors[_floors.Count - 1].MicroChips.Count() > 0)
                result = true;

            for(int i = _floors.Count - 2; i >= 0; i--)
            {
                if (_floors[i].Generators.Count() > 0 ||
                        _floors[i].MicroChips.Count() > 0)
                    result = false;
            }

            return result;
        }

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
                long floorHash = Floors[floor].Hash;                
                if (ElevatorOn == floor + 1)
                    floorHash += elevatorPresentKey;
                long floorAdjustmentMultiplier = (long)Math.Pow(65536, floor);
                result = result + (floorHash * floorAdjustmentMultiplier);
            }
            return result;
        }

        /// <summary>
        /// Important optimisation according to some dude on reddit:
        /// https://www.reddit.com/r/adventofcode/comments/5hoia9/2016_day_11_solutions/db1v1ws/
        /// </summary>
        internal long[] EquivalentHashes()
        {
            Dictionary<int, int> floorPairs = new Dictionary<int, int>();
            foreach(Floor f in Floors)
            {
                for(int i = 1; i < 7; i++)
                {
                    if (f.ContainsGenerator(i) && f.ContainsChip(i))
                        floorPairs[i] = f.FloorNumber;
                }
            }

            List<long> possibleAlternatives = new List<long>();
            // Substitute every pair for every other pair in the building
            foreach (var x in floorPairs)
            {
                foreach(var y in floorPairs)
                {
                    if (x.Key != y.Key && x.Value != y.Value)
                    {
                        Building alternate = Clone();
                        alternate.Floors[x.Value - 1].RemoveGenerator(x.Key);
                        alternate.Floors[x.Value - 1].RemoveMicrochip(x.Key);
                        alternate.Floors[x.Value - 1].AddGenerator(y.Key);
                        alternate.Floors[x.Value - 1].AddMicrochip(y.Key);

                        alternate.Floors[y.Value - 1].RemoveGenerator(y.Key);
                        alternate.Floors[y.Value - 1].RemoveMicrochip(y.Key);
                        alternate.Floors[y.Value - 1].AddGenerator(x.Key);
                        alternate.Floors[y.Value - 1].AddMicrochip(x.Key);
                        possibleAlternatives.Add(alternate.Hash());
                    }
                }
            }

            return possibleAlternatives.ToArray();
        }
        
        public bool Equals(Building other)
        {
            return Hash() == other.Hash();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Building);
        }

        public override int GetHashCode()
        {
            return Hash().GetHashCode();
        }
    }
}
