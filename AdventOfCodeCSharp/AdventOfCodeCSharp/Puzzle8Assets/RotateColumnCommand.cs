using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle8Assets
{
    public class RotateColumnCommand : BaseCommand
    {
        private int _col;
        private int _moveBy;

        public RotateColumnCommand(string fullCommand) : base(fullCommand)
        {
        }

        public override void ApplyCommand(bool[,] matrix)
        {
            bool[] newCol = new bool[matrix.GetLength(1)];
            for (int i = 0; i < newCol.Length; i++)
            {
                // Assigning in the bounds
                if (i + _moveBy <= newCol.Length - 1)
                {
                    newCol[i + _moveBy] = matrix[_col, i];
                }
                else
                {
                    // Have wrapped = move relative to the left now
                    newCol[i + _moveBy - newCol.Length] = matrix[_col, i];
                }
            }
            for (int i = 0; i < newCol.Length; i++)
            {
                matrix[_col, i] = newCol[i];
            }
        }

        protected override void ParseCommandInput(string fullCommand)
        {
            string[] commandPortions = fullCommand.Split(' ');
            string rowIdent = commandPortions[2];
            _col = Convert.ToInt32(rowIdent.Split('=')[1]);
            _moveBy = Convert.ToInt32(commandPortions[4]);
        }
    }
}
