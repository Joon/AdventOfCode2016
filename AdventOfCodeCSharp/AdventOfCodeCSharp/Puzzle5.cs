using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Puzzle5
    {

        public string ProcessPuzzle(string input)
        {
            string result = "";
            int currentCount = 0;
            MD5 hasher = MD5.Create();
            // Brute force babby!!
            while (result.Length < 8)
            {
                currentCount++;
                string hash = HashValue(input, currentCount, hasher);
                if (hash.StartsWith("00000"))
                    result += hash[5];
            }
            return result.ToLower();
        }

        private static string HashValue(string input, int currentCount, MD5 hasher)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input + currentCount.ToString());
            byte[] hash = hasher.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
