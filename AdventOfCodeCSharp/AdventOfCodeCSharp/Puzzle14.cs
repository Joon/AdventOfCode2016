using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle14
    {

        public int SolvePuzzle(string salt)
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            List<string> OTP = new List<string>();

            MD5 hasher = MD5.Create();
            int hashPosition = 1;
            while (OTP.Count < 64)
            {
                hashPosition++;
                string hashCandidate;
                hashCandidate = GetHash(salt + hashPosition.ToString(), hashes, hasher);                
                char repeater;
                bool hadRepeatingChar = HashContainsRepeatingChar(hashCandidate, 3, out repeater);
                // Is it a possible key?
                if (hadRepeatingChar)
                {
                    for(int i = 1; i <= 1000; i++)
                    {
                        string hashToTest = GetHash(salt + (hashPosition + i).ToString(), hashes, hasher);
                        if (HashContainsSpecificRepeat(hashToTest, repeater, 5))
                        {
                            OTP.Add(hashCandidate);
                            break;
                        }
                    }
                }
            }
            return hashPosition;
        }
        
        public int SolvePuzzlePart2(string salt)
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            Dictionary<int, string> OTP = new Dictionary<int, string>();

            int consoleY = Console.CursorTop;
                        
            int calculations = 0;
            List<Task> processors = new List<Task>();
            for (int offset = 0; offset < 4; offset++)
            {
                int hashPosition = offset;
                Task calcer = new Task(() =>
                {
                    int foundCount = 0;
                    MD5 hasher = MD5.Create();
                    while (foundCount < 64)
                    {
                        lock (OTP)
                            foundCount = OTP.Count;
                        Interlocked.Increment(ref calculations);
                        
                        string tempHash = GetHash(salt + hashPosition.ToString(), hashes, hasher);
                        string hashCandidate = StretchKey(tempHash, hashes, hasher);
                        char repeater;
                        bool hadRepeatingChar = HashContainsRepeatingChar(hashCandidate, 3, out repeater);
                        // Is it a possible key?
                        if (hadRepeatingChar)
                        {
                            for (int i = 1; i <= 1000; i++)
                            {
                                string tempHash2 = GetHash(salt + (hashPosition + i).ToString(), hashes, hasher);
                                string hashToTest = StretchKey(tempHash2, hashes, hasher);
                                if (HashContainsSpecificRepeat(hashToTest, repeater, 5))
                                {
                                    lock (OTP)
                                    {
                                        OTP[hashPosition] = hashCandidate;
                                    }
                                    break;
                                }
                            }
                        }

                        hashPosition = hashPosition + 4;
                    }
                });
                processors.Add(calcer);
            }

            foreach (Task t in processors)
                t.Start();

            while (true)
            {
                Console.SetCursorPosition(0, consoleY);
                lock (OTP)
                    Console.WriteLine("Have calculated a total of " + calculations.ToString() + " hashes. Keys found: " + OTP.Count.ToString());

                bool allDone = true;
                foreach (Task t in processors)
                {
                    if (!t.IsCompleted && !t.IsCanceled && !t.IsFaulted)
                    {
                        allDone = false;
                    }
                }
                if (allDone)
                    break;
            }
            return OTP.Last().Key;
        }

        private string StretchKey(string tempHash, Dictionary<string, string> hashes, MD5 hasher)
        {
            string workingKey = tempHash;
            for (int i = 1; i <= 2016; i++)
            {
                workingKey = GetHash(workingKey, hashes, hasher);
            }
            return workingKey;
        }

        private bool HashContainsSpecificRepeat(string hash, char toTest, int numberOfRepeats)
        {
            string test = "".PadLeft(numberOfRepeats, toTest);
            return hash.Contains(test);
        }

        private bool HashContainsRepeatingChar(string hashCandidate, int numberOfRepeats, out char repeater)
        {
            for(int i = 0; i < hashCandidate.Length - (numberOfRepeats - 1); i++)
            {
                if (hashCandidate[i] == hashCandidate[i + 1])
                {
                    bool hasRequiredRepeats = true;
                    for (int diff = 1; diff < numberOfRepeats; diff++)
                    {
                        if (hashCandidate[i] != hashCandidate[i + diff])
                            hasRequiredRepeats = false;
                    }
                    if (hasRequiredRepeats)
                    {
                        repeater = hashCandidate[i];
                        return true;
                    }
                }
            }
            repeater = '\0';
            return false;
        }

        private static string GetHash(string valueToHash, Dictionary<string, string> hashes, MD5 hasher)
        {
            string hashCandidate;
            bool hasBeenCalculated;
            lock (hashes)
                hasBeenCalculated = hashes.ContainsKey(valueToHash);
            
            if (!hasBeenCalculated)
            {
                hashCandidate = HashValue(valueToHash, hasher);
                lock (hashes)
                {
                    hashes[valueToHash] = hashCandidate;
                }
            }
            else
            {
                lock (hashes)
                {
                    hashCandidate = hashes[valueToHash];
                }
            }
            return hashCandidate;
        }

        private static string HashValue(string input, MD5 hasher)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = hasher.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        
    }
}
