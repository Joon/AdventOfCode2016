using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle8Assets
{
    public static class CommandFactory
    {

        public static BaseCommand CreateCommand(string commandText)
        {
            if (commandText.StartsWith("rect"))
                return new RectCommand(commandText);
            if (commandText.StartsWith("rotate column"))
                return new RotateColumnCommand(commandText);
            if (commandText.StartsWith("rotate row"))
                return new RotateRowCommand(commandText);
            throw new ApplicationException("Did not understand command: " + commandText);
        }

    }
}
