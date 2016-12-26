using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle15Assets
{
    public class Disc
    {
        public int CurrentPosition { get; set; }

        public int AvailablePositions { get; set; }

        public void Rotate()
        {
            CurrentPosition++;
            if (CurrentPosition > (AvailablePositions - 1))
                CurrentPosition = 0;
        }

        public int ProjectMovement(int i)
        {
            int leftOverRotations = i % AvailablePositions;
            int applyLeftOver = CurrentPosition + leftOverRotations;
            if (applyLeftOver >= AvailablePositions)
                return applyLeftOver - AvailablePositions;
            else
                return applyLeftOver;
        }
    }
}
