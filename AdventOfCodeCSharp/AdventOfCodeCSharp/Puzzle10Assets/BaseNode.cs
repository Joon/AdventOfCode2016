using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle10Assets
{
    /// <summary>
    /// Base class for bots and output bins
    /// </summary>
    public abstract class BaseNode
    {

        public BaseNode SendHigherValueTo { get; set; }

        public BaseNode SendLowerValueTo { get; set; }

        protected BotNet _botnet;

        protected List<int> _values;

        public abstract void TakeValue(int value);

    }
}
