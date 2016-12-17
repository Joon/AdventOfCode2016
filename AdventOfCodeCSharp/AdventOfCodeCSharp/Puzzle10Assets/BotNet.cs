using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle10Assets
{
    public delegate void ValueCompared(int lowerValue, int higherValue, int botId);
    
    /// <summary>
    /// Workspace to hold all the bots and allows bots to communicate with one another
    /// </summary>
    public class BotNet
    {

        Dictionary<int, Bot> _bots;
        Dictionary<int, Output> _outputs;

        public BotNet()
        {
            _bots = new Dictionary<int, Bot>();
            _outputs = new Dictionary<int, Output>();
        }

        public Bot GetBot(int botId)
        {
            if (_bots.ContainsKey(botId))
                return _bots[botId];
            return null;
        }

        internal void RunProcessing()
        {
            bool keepOnProcessing = true;
            while (keepOnProcessing)
            {
                keepOnProcessing = false;
                foreach (Bot b in _bots.Values)
                {
                    keepOnProcessing = keepOnProcessing || b.CompareAndDistributeValues();
                }
            }
        }

        public void EnsureBot(params int[] botIds)
        {
            foreach (int botId in botIds)
            {
                if (!_bots.ContainsKey(botId))
                    _bots[botId] = new Bot(this, botId);
            }
        }

        public ValueCompared OnValueCompared;

        internal void OnCompare(int lowerValue, int higherValue, int botId)
        {
            OnValueCompared?.Invoke(lowerValue, higherValue, botId);
        }

        internal void EnsureOutput(int outputId)
        {
            if (!_outputs.ContainsKey(outputId))
                _outputs[outputId] = new Output(this, outputId);
        }

        internal Output GetOutput(int outputId)
        {
            if (_outputs.ContainsKey(outputId))
                return _outputs[outputId];
            return null;
        }
    }
}
