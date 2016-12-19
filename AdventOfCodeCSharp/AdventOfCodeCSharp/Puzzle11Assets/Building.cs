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

        public string Hash()
        {
            string result = ElevatorOn.ToString();
            foreach(Floor f in Floors)
            {
                result += f.Hash;
            }
            return result;
        }
        
    }
}
