using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Puzzle5b
    {

        public string ProcessPuzzle(string input)
        {
            Console.WriteLine();

            StringBuilder result = new StringBuilder();
            result.Append('0');
            result.Append('0');
            result.Append('0');
            result.Append('0');
            result.Append('0');
            result.Append('0');
            result.Append('0');
            result.Append('0');
            MD5 hasher = MD5.Create();
            bool[] touched = new bool[8] { false, false, false, false, false, false, false, false };
            bool done = false;
            int currentCount = 0;
            // Brute force babby!!
            while (!done)
            {
                if (currentCount % 10000 == 0)
                    WriteProgress(currentCount, result, touched);
                currentCount++;
                string hash = HashValue(input, currentCount, hasher);
                if (hash.StartsWith("00000") && hash[5] >= '0' && hash[5] <= '7')
                {
                    int index = Convert.ToInt32(hash[5].ToString());
                    if (!touched[index])
                    {
                        result[index] = hash[6];
                        touched[index] = true;
                    }
                }
                done = true;
                for(int i = 0; i < 8; i++)
                {
                    done &= touched[i];
                }
            }
            Console.Write('\r');
            Console.WriteLine(currentCount.ToString() + " fecking iterations!!");
            return result.ToString().ToLower();
        }
        private int animationPos = 0;
        private static string animationchars = "+_)(*&^%$£!¬`\\|/<>,.";

        private void WriteProgress(int currentCount, StringBuilder result, bool[] touched)
        {
            Console.Write('\r');
            for(int i = 0; i < 8; i++)
            {
                Console.Write(touched[i] ? result[i] : animationchars[animationPos]);
                animationPos++;
                if (animationPos > animationchars.Length - 1)
                    animationPos = 0;
            }
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
