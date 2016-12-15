using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class IPV7Entry
    {
        public string Original { get; set; }

        public List<string> Hypernets { get; set; }

        public List<string> SuperNets { get; set; }

        public IPV7Entry()
        {
            Hypernets = new List<string>();
            SuperNets = new List<string>();
        }
    }

    public class Puzzle7
    {
        public int ProcessPuzzle(string input)
        {
            int supportTLSCount = 0;
            List<IPV7Entry> entries = ParseIPs(input);
            foreach(IPV7Entry entry in entries)
            {
                bool hyperNetHadABBA = false;
                foreach(string hypernet in entry.Hypernets)
                {
                    if (ContainsABBA(hypernet))
                    {
                        hyperNetHadABBA = true;
                        break;
                    }
                }
                if (!hyperNetHadABBA)
                {
                    foreach(string nonHypernet in entry.SuperNets)
                    {
                        if (ContainsABBA(nonHypernet))
                        {
                            supportTLSCount++;
                            break;
                        }
                    }
                }
            }
            return supportTLSCount;
        }

        public int ProcessPuzzleB(string input)
        {
            int supportSSLCount = 0;
            List<IPV7Entry> entries = ParseIPs(input);
            foreach (IPV7Entry entry in entries)
            {
                List<string> BABs = PossibleBABs(entry.SuperNets);
                if (ABAExists(entry.Hypernets, BABs))
                    supportSSLCount++;
            }
            return supportSSLCount;
        }

        private bool ABAExists(List<string> hypernets, List<string> bABs)
        {
            foreach(string bab in bABs)
            {
                string aba;
                aba = bab[1].ToString();
                aba += bab[0];
                aba += bab[1];
                foreach(string hypernet in hypernets)
                {
                    if (hypernet.Contains(aba))
                        return true;
                }
            }
            return false;
        }

        private List<string> PossibleBABs(List<string> superNets)
        {
            List<string> BABs = new List<string>();
            foreach(string net in superNets)
            {
                for(int i = 1; i < net.Length - 1; i++)
                {
                    if (net[i] != net[i - 1] && net[i - 1] == net[i + 1])
                        BABs.Add(net.Substring(i - 1, 3));
                }
            }
            return BABs;
        }

        private bool ContainsABBA(string checkString)
        {
            for(int i = 1; i < checkString.Length - 2; i++)
            {
                if (checkString[i] == checkString[i + 1] && checkString[i - 1] == checkString[i + 2] &&
                    checkString[i] != checkString[i - 1])
                    return true;
            }
            return false;
        }

        private List<IPV7Entry> ParseIPs(string input)
        {
            List<IPV7Entry> result = new List<IPV7Entry>();

            string[] lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in lines)
            {
                IPV7Entry item = new IPV7Entry();
                item.Original = s;
                StringBuilder builder = new StringBuilder();
                bool inHypernet = false;
                foreach (char c in s)
                {
                    if (c == '[')
                    {
                        if (builder.Length > 0)
                        {
                            item.SuperNets.Add(builder.ToString());
                            builder.Clear();
                        }
                        if (inHypernet)
                            throw new ApplicationException("Already in hypernet and got another [ char. WTF?");
                        inHypernet = true;
                    }
                    if (c == ']')
                    {
                        if (builder.Length > 0)
                        {
                            item.Hypernets.Add(builder.ToString());
                            builder.Clear();
                        }
                        if (!inHypernet)
                            throw new ApplicationException("Outside of hypernet and got a ] char. WTF?");
                        inHypernet = false;
                    }
                    builder.Append(c);
                }
                if (builder.Length > 0 && inHypernet)
                    item.Hypernets.Add(builder.ToString());
                if (builder.Length > 0 && !inHypernet)
                    item.SuperNets.Add(builder.ToString());
                result.Add(item); 
            }
            return result;
        }
    }
}
