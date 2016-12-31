using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Puzzle20
    {
        public uint SolvePuzzle(string input)
        {
            string[] filterStrings = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<Tuple<uint, uint>> filters = new List<Tuple<uint, uint>>();
            foreach (string filterS in filterStrings)
            {
                string[] filterParts = filterS.Split('-');
                Tuple<uint, uint> filter = new Tuple<uint, uint>(Convert.ToUInt32(filterParts[0]), Convert.ToUInt32(filterParts[1]));
                filters.Add(filter);
            }

            filters.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            bool solved = false;
            uint answer = 0;

            while (!solved)
            {
                // is answer in a boundary
                var findContainingFilter = from f in filters
                                           where f.Item1 <= answer && f.Item2 >= answer
                                           select f;
                var container = findContainingFilter.OrderByDescending(o => o.Item2).FirstOrDefault();
                if (container == null)
                {
                    solved = true;
                }
                else
                {
                    answer = container.Item2 + 1;
                }
            }

            return answer;
        }

        public uint SolvePuzzle2(string input)
        {
            string[] filterStrings = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<Tuple<uint, uint>> filters = new List<Tuple<uint, uint>>();
            foreach (string filterS in filterStrings)
            {
                string[] filterParts = filterS.Split('-');
                Tuple<uint, uint> filter = new Tuple<uint, uint>(Convert.ToUInt32(filterParts[0]), Convert.ToUInt32(filterParts[1]));
                filters.Add(filter);
            }

            filters.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            uint maxIPs = 4294967295;
            uint result = 0;
            uint currentIP = 0;
            Console.CursorVisible = false;
            
            while (currentIP < maxIPs)
            {
                var findfilter = from f in filters
                                 where f.Item1 <= currentIP && f.Item2 >= currentIP
                                 select f;
                var largestFilter = findfilter.OrderByDescending(o => o.Item2).FirstOrDefault();
                if (largestFilter == null)
                {
                    result++;
                }
                else
                {
                    if (currentIP < largestFilter.Item2)
                        currentIP = largestFilter.Item2;
                }
                if (currentIP < maxIPs)
                    currentIP++;
            }
            return result;
        }
        
    }
}