using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Puzzle16
    {
        public string SolvePuzzle(string input, int fillLength)
        {
            int calculatedTo = input.Length;
            string a = input;
            while (calculatedTo < fillLength)
            {
                string b = a;
                b = new string(b.Reverse().ToArray());
                b = b.Replace('0', 'y');
                b = b.Replace('1', '0');
                b = b.Replace('y', '1');
                a = a + '0' + b;
                calculatedTo = a.Length;
            }

            string calculatedFill = a.Substring(0, fillLength);
            string calcCheckSumOn = calculatedFill;

            StringBuilder checksum = new StringBuilder();
            while (checksum.Length % 2 != 1)
            {
                checksum.Clear();
                int checkIndex = 0;
                while (checkIndex < calcCheckSumOn.Length)
                {
                    char first = calcCheckSumOn[checkIndex];
                    char second = calcCheckSumOn[checkIndex + 1];

                    if (first == second)
                        checksum.Append('1');
                    else
                        checksum.Append('0');

                    checkIndex += 2;
                }
                calcCheckSumOn = checksum.ToString();
            }

            return checksum.ToString();
        }

    }
}
