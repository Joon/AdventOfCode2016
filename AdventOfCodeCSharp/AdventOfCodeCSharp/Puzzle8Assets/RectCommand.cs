using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle8Assets
{
    public class RectCommand : BaseCommand
    {
        private int _width;
        private int _height;

        public RectCommand(string fullCommand) : base(fullCommand)
        {
        }

        public override void ApplyCommand(bool[,] matrix)
        {
            for(int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    matrix[x, y] = true;
                }
            }
        }

        protected override void ParseCommandInput(string fullCommand)
        {
            string[] commandParsed = fullCommand.Split(' ');
            string dimensions = commandParsed[1];
            string[] intDimensions = dimensions.Split('x');
            _width = Convert.ToInt32(intDimensions[0]);
            _height = Convert.ToInt32(intDimensions[1]);
        }
    }
}
