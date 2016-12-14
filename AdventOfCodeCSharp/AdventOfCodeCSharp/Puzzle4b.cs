using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle4b
    {
        
        public string ProcessPuzzle(string input)
        {
            string[] lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<CryptoWord> toDecrypt = GetValidWords(lines);
            List<string> possibleRooms = new List<string>();

            foreach(var encrypted in toDecrypt)
            {
                int actualCharsToMove = encrypted.SectorId % 26;
                var convertQry = from c in encrypted.Word
                                 select (char)
                                 // Special case 1: the hyphen between words
                                 (c == '-' ? c :
                                 // Special case 2: the plus wraps past z
                                 (c + actualCharsToMove > 'z' ? c - (26 - actualCharsToMove) : 
                                 // normal case - just move the char along
                                 (c + actualCharsToMove)));

                string decrypted = new string(convertQry.ToArray());
                Console.WriteLine(decrypted);
                if (decrypted.ToLower().Contains("north"))
                    possibleRooms.Add(decrypted + ": " + encrypted.SectorId.ToString());
            }

            return string.Join(Environment.NewLine, possibleRooms.ToArray());
        }

        private List<CryptoWord> GetValidWords(string[] lines)
        {
            List<CryptoWord> validWords = new List<CryptoWord>();
            foreach (string line in lines)
            {
                string[] linePortions = line.Split('-');
                string sectorIdAndhash = linePortions[linePortions.Length - 1];
                string[] hashBits = sectorIdAndhash.Split("[]".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                CryptoWord word = new CryptoWord();
                word.Hash = hashBits[1];
                word.SectorId = Convert.ToInt32(hashBits[0]);
                word.Word = String.Join("-", linePortions.Take(linePortions.Count() - 1));
                string wordChars = string.Concat(linePortions.Take(linePortions.Count() - 1));
                var qry = from c in wordChars
                          group c by c into g
                          select new { Character = g.Key, Count = g.Count() };
                List<dynamic> list = qry.ToList<dynamic>();
                list.Sort((b, a) => a.Count.CompareTo(b.Count) == 0 ? b.Character.CompareTo(a.Character) : a.Count.CompareTo(b.Count));

                var qry2 = from v in list
                           select (char)v.Character;
                string compareHash = new string(qry2.Take(5).ToArray());

                word.Valid = compareHash == word.Hash;

                if (word.Valid)
                    validWords.Add(word);
            }
            return validWords;
        }
    }
}
