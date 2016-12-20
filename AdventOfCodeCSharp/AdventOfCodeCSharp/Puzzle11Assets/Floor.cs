using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class Floor
    {
        private List<Generator> _generators;
        public List<Generator> Generators { get { return _generators; } }

        private List<Microchip> _microChips;
        public List<Microchip> MicroChips { get { return _microChips; } }

        public int FloorNumber { get; set; }

        public Floor()
        {
            _generators = new List<Generator>();
            _microChips = new List<Microchip>();
        }

        internal Floor Clone()
        {
            Floor result = new Floor();
            result.Generators.AddRange(Generators);
            result.MicroChips.AddRange(MicroChips);
            result.FloorNumber = FloorNumber;
            return result;
        }

        public string Hash
        {
            get
            {
                string result = "F" + FloorNumber.ToString();
                MicroChips.Sort((a, b) => a.MicrochipNumber.CompareTo(b.MicrochipNumber));
                foreach (var chip in MicroChips)
                {
                    result += "C" + chip.MicrochipNumber;
                }
                Generators.Sort((a, b) => a.GeneratorNumber.CompareTo(b.GeneratorNumber));
                foreach (var gen in Generators)
                {
                    result += "G" + gen.GeneratorNumber;
                }
                return result;
            } }
    }
}
