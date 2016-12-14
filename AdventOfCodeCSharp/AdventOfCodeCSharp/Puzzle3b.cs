using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    
    class Puzzle3b
    {

        public int ProcessPuzzle(string input)
        {
            List<TriangleCandidate> possibles = new List<TriangleCandidate>();
            int currentPos = 3;
            TriangleCandidate t1 = null;
            TriangleCandidate t2 = null;
            TriangleCandidate t3 = null;
            foreach (string s in input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                currentPos++;

                if (currentPos > 3)
                {
                    currentPos = 1;
                    if (t1 != null)
                    {
                        possibles.Add(t1);
                        possibles.Add(t2);
                        possibles.Add(t3);
                    }
                    t1 = new TriangleCandidate();
                    t2 = new TriangleCandidate();
                    t3 = new TriangleCandidate();
                }

                string[] sides = s.Split("\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                t1.SetSide(currentPos, Convert.ToInt32(sides[0]));
                t2.SetSide(currentPos, Convert.ToInt32(sides[1]));
                t3.SetSide(currentPos, Convert.ToInt32(sides[2]));            
            }
            if (t1 != null)
            {
                possibles.Add(t1);
                possibles.Add(t2);
                possibles.Add(t3);
            }

            return possibles.Count(o => o.IsValid());
        }
    }
}
