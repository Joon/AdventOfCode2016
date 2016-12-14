using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class TriangleCandidate
    {
        public int s1 { get; set; }
        public int s2 { get; set; }
        public int s3 { get; set; }

        public void SetSide(int side, int value)
        {
            switch (side)
            {
                case 1:
                    s1 = value;
                    break;
                case 2:
                    s2 = value;
                    break;
                case 3:
                    s3 = value;
                    break;
            }
        }

        internal bool IsValid()
        {
            return (s1 + s2 > s3) && (s1 + s3 > s2) && (s2 + s3 > s1);
        }
    }

    class Puzzle3
    {
        
        public int ProcessPuzzle(string input)
        {
            List<TriangleCandidate> possibles = new List<TriangleCandidate>();     
            foreach(string s in input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                string[] sides = s.Split("\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                TriangleCandidate t = new TriangleCandidate();
                t.s1 = Convert.ToInt32(sides[0]);
                t.s2 = Convert.ToInt32(sides[1]);
                t.s3 = Convert.ToInt32(sides[2]);
                possibles.Add(t);
            }

            return possibles.Count(o => o.IsValid());
        }
    }
}
