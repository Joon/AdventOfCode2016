using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle10Assets
{
    /// <summary>
    /// An individual bot within a bot net. Tracks linked bots and currently held values
    /// </summary>
    public class Bot : BaseNode
    {
        private int _botId;
        public int BotId {  get { return _botId; } }
       
        public Bot(BotNet workspace, int botId)
        {
            _botnet = workspace;
            _botId = botId;
            _values = new List<int>();
        }

        public override void TakeValue(int value)
        {
            Console.WriteLine("Value {0} is now in bot {1}", value, BotId);
            _values.Add(value);
        }

        public bool CompareAndDistributeValues()
        {
            if (_values.Count > 1)
            {
                // Sort values to get the lowest and highest
                _values.Sort();
                int lowerValue = _values[0];
                int higherValue = _values[1];

                if (SendHigherValueTo != null && SendLowerValueTo != null)
                {
                    // Notify the workspace of the comparison operation
                    _botnet.OnCompare(lowerValue, higherValue, BotId);

                    SendHigherValueTo.TakeValue(higherValue);
                    _values.Remove(higherValue);

                    SendLowerValueTo.TakeValue(lowerValue);
                    _values.Remove(lowerValue);

                    return true;
                } else
                {
                    Console.WriteLine(string.Format("Values {0} and {1} stuck in a dead end bot with no outputs {2}",
                        lowerValue, higherValue, BotId));
                    return false; 
                }
            }
            return false;
        }
    }
}
