using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle8Assets
{
    public abstract class BaseCommand
    {

        public BaseCommand(string fullCommand)
        {
            ParseCommandInput(fullCommand);
        }

        public abstract void ApplyCommand(bool[,] matrix);

        protected abstract void ParseCommandInput(string fullCommand);

    }
}
