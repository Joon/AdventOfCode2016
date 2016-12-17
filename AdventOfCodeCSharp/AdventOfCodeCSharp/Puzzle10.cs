using AdventOfCodeCSharp.Puzzle10Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle10
    {
        public int ProcessPuzzle(string input)
        {
            BotNet workspace = PrepareWorkspace(input); int comparedBotId = 0;
            workspace.OnValueCompared += delegate (int low, int high, int id)
            {
                Console.WriteLine("Bot {0} compared low {1} and high {2}", id, low, high);
                if (low == 17 && high == 61)
                    comparedBotId = id;
            };

            workspace.RunProcessing();

            return comparedBotId;
        }

        public int ProcessPuzzleOutputs(string input)
        {
            BotNet workspace = PrepareWorkspace(input); 

            workspace.RunProcessing();

            return workspace.GetOutput(0).Values[0] *
                workspace.GetOutput(1).Values[0] *
                workspace.GetOutput(2).Values[0];
        }

        private static BotNet PrepareWorkspace(string input)
        {
            BotNet workspace = new BotNet();
            CommandParser parser = new CommandParser();
            parser.ProcessCommands(input, workspace);
            return workspace;
        }
    }
}
