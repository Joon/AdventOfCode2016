using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Puzzle2
    {

        int[,] buttons = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
        
        public string ProcessPuzzle(string input)
        {
            int x = 1;
            int y = 1;
            StringBuilder result = new StringBuilder();

            foreach (string line in input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                foreach(var instruction in line)
                {
                    switch(instruction)
                    {
                        case 'U':
                            y = Math.Max(y - 1, 0);
                            break;
                        case 'L':
                            x = Math.Max(x - 1, 0);
                            break;
                        case 'R':
                            x = Math.Min(x + 1, 2);
                            break;
                        case 'D':
                            y = Math.Min(y + 1, 2);
                            break;
                    }
                }
                result.Append(buttons[y, x].ToString());
            }
            return result.ToString();
        }

    }
}
