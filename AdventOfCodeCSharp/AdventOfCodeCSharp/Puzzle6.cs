using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Puzzle6
    {

        public string ProcessPuzzle(string input)
        {
            string[] lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<StringBuilder> columns = CalculateColumns(lines);
            string result = "";
            foreach (StringBuilder sb in columns)
            {
                string col = sb.ToString();
                var qry = from c in col
                          group c by c into g
                          select new { Character = g.Key, Count = g.Count() };
                List<dynamic> charCounts = qry.ToList<dynamic>();
                charCounts.Sort((b, a) => a.Count.CompareTo(b.Count));
                result += charCounts[0].Character;
            }
            return result;
        }

        public string ProcessPuzzleB(string input)
        {
            string[] lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<StringBuilder> columns = CalculateColumns(lines);
            string result = "";
            foreach (StringBuilder sb in columns)
            {
                string col = sb.ToString();
                var qry = from c in col
                          group c by c into g
                          select new { Character = g.Key, Count = g.Count() };
                List<dynamic> charCounts = qry.ToList<dynamic>();
                charCounts.Sort((a, b) => a.Count.CompareTo(b.Count));
                result += charCounts[0].Character;
            }
            return result;
        }

        private static List<StringBuilder> CalculateColumns(string[] lines)
        {
            List<StringBuilder> columns = new List<StringBuilder>();

            foreach (string line in lines)
            {
                int col = -1;
                foreach (char c in line)
                {
                    col++;
                    if (columns.Count < line.Length)
                    {
                        columns.Add(new StringBuilder());
                    }
                    columns[col].Append(c);
                }
            }

            return columns;
        }
    }
}
