using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Puzzle2b
    {

        char[][] buttons;

        public Puzzle2b()
        {
            buttons = new char[5][];
            buttons[0] = new char[]           { '1' };
            buttons[1] = new char[]      { '2', '3', '4' };
            buttons[2] = new char[] { '5', '6', '7', '8', '9' };
            buttons[3] = new char[]      { 'A', 'B', 'C' };
            buttons[4] = new char[]           { 'D' };
        }
        
        public string ProcessPuzzle(string input)
        {
            int x = 0;
            int y = 2;
            StringBuilder result = new StringBuilder();

            foreach (string line in input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                foreach(var instruction in line)
                {
                    int move = 0;
                    switch(instruction)
                    {
                        case 'U':
                            move = VerticalNavigationValue(x, y, true);
                            y = Math.Max(y + move, 0);
                            if (move != 0)
                                x = x + (y >= 2 ? 1 : -1);
                            break;
                        case 'D':
                            move = VerticalNavigationValue(x, y, false);
                            y = Math.Min(y + move, 4);
                            if (move != 0)
                                x = x + (y > 2 ? -1 : 1);
                            break;
                        case 'L':
                            x = Math.Max(x - 1, 0);
                            break;
                        case 'R':
                            x = Math.Min(x + 1, buttons[y].Length - 1);
                            break;                        
                    }
                }
                result.Append(buttons[y][x]);
            }
            return result.ToString();
        }

        private int VerticalNavigationValue(int x, int y, bool up)
        {
            var result = up ? -1 : 1;
            // Can't navigate up along top left edge
            if (x == 0 && y <= 2 && up)
                return 0;
            // Cant navigate up along top right edge
            if (up && ((x == 2 && y == 1) || (x == 4 && y == 2)))
                return 0;
            // Can't navigate down along bottom right edge
            if (!up && ((x == 4) || (x == 2 && y == 3) || (x == 0 && y == 4)))
                return 0;
            // Can't navigate down along bottom left edge
            if (!up && (x == 0 && y > 2))
                return 0;

            return result;
        }
    }
}
