using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle8Assets
{
    public class RotateRowCommand : BaseCommand
    {

        private int _row;
        private int _moveBy;

        public RotateRowCommand(string fullCommand) : base(fullCommand)
        {
        }

        public override void ApplyCommand(bool[,] matrix)
        {
            bool[] newRow = new bool[matrix.GetLength(0)];
            for (int i = 0; i < newRow.Length; i++)
            {
                // Assigning in the bounds
                if (i + _moveBy <= newRow.Length - 1)
                {
                    newRow[i + _moveBy] = matrix[i, _row];
                }
                else
                {
                    // Have wrapped = move relative to the left now
                    newRow[i + _moveBy - newRow.Length] = matrix[i, _row];
                }
            }
            for (int i = 0; i < newRow.Length; i++)
            {
                matrix[i, _row] = newRow[i];
            }
        }

        protected override void ParseCommandInput(string fullCommand)
        {
            string[] commandPortions = fullCommand.Split(' ');
            string rowIdent = commandPortions[2];
            _row = Convert.ToInt32(rowIdent.Split('=')[1]);
            _moveBy = Convert.ToInt32(commandPortions[4]);
        }
    }
}
