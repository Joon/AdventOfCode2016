using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle10Assets
{
    public class Output : BaseNode
    {
        public int OutputId { get; set; }

        public List<int> Values { get { return _values; } }

        public Output(BotNet workspace, int outputId)
        {
            _values = new List<int>();
            OutputId = outputId;
            _botnet = workspace;
        }

        public override void TakeValue(int value)
        {
            Console.WriteLine("Value {0} is now in output {1}", value, OutputId);
            _values.Add(value);
        }
    }
}
