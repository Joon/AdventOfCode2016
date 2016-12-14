using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class CryptoWord
    {
        public string Word { get; set; }
        public int SectorId { get; set; }
        public string Hash { get; set; }
        public bool Valid { get; set; }
    }

    public class Puzzle4
    {
        
        public int ProcessPuzzle(string input)
        {
            int validIDSum = 0;
            string[] lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach(string line in lines)
            {
                string[] linePortions = line.Split('-');
                string sectorIdAndhash = linePortions[linePortions.Length - 1];
                string[] hashBits = sectorIdAndhash.Split("[]".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                CryptoWord word = new CryptoWord();
                word.Hash = hashBits[1];
                word.SectorId = Convert.ToInt32(hashBits[0]);
                word.Word = String.Concat(linePortions.Take(linePortions.Count() - 1));
                var qry = from c in word.Word
                            group c by c into g
                            select new { Character = g.Key, Count = g.Count() };
                List<dynamic> list = qry.ToList<dynamic>();
                list.Sort((b, a) => a.Count.CompareTo(b.Count) == 0 ? b.Character.CompareTo(a.Character) : a.Count.CompareTo(b.Count));
                var qry2 = from v in list
                           select (char)v.Character;
                string compareHash = new string(qry2.Take(5).ToArray());

                word.Valid = compareHash == word.Hash;

                if (word.Valid)
                    validIDSum += word.SectorId;
            }

            return validIDSum;
        }

    }
}
