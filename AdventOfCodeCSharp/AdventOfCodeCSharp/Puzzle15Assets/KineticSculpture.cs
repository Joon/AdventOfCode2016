using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle15Assets
{
    public class KineticSculpture
    {

        private List<Disc> _discs;

        public List<Disc> Discs { get { return _discs; } }

        public KineticSculpture()
        {
            _discs = new List<Disc>();
        }

        public void InitialiseDisc(int discNumber, int discSlots, int discPosition)
        {
            if (discNumber > _discs.Count)
            {
                for(int i = _discs.Count; i < discNumber; i++)
                {
                    Disc newDisc = new Disc();
                    _discs.Add(newDisc);
                }
            }
            _discs[discNumber - 1].AvailablePositions = discSlots;
            _discs[discNumber - 1].CurrentPosition = discPosition;
        }


        public void Rotate()
        {
            foreach (Disc d in _discs)
                d.Rotate();
        }

        public bool CurrentPositionSolvesPuzzle()
        {
            for(int i = 1; i <= _discs.Count; i++)
            {
                if (_discs[i - 1].ProjectMovement(i) != 0)
                    return false;
            }
            return true;
        }
    }
}
