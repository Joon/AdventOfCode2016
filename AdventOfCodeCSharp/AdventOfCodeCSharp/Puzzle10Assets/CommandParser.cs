using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle10Assets
{
    /// <summary>
    /// Parses the command string and creates the appropriate command class
    /// to execute it
    /// </summary>
    public class CommandParser
    {
                
        public void ProcessCommands(string commands, BotNet workspace)
        {
            Dictionary<int, int> valuesToDistribute= new Dictionary<int, int>();
            foreach (string command in commands.Split(Environment.NewLine.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries))
            {
                if (command.StartsWith("value"))
                {
                    int botId;
                    int value;
                    DecodeValueCommand(command, out botId, out value);
                    workspace.EnsureBot(botId);
                    workspace.GetBot(botId).TakeValue(value);
                }   
                if (command.StartsWith("bot"))
                    SetBotStructure(command, workspace);
            }
        }

        private void SetBotStructure(string command, BotNet workspace)
        {
            // example command: "bot 149 gives low to bot 17 and high to output 5"
            string[] commandWords = command.Split(' ');

            int botId = Convert.ToInt32(commandWords[1]);
            bool giveLowToBot = commandWords[5] == "bot";
            int giveLowTo = Convert.ToInt32(commandWords[6]);
            bool giveHighToBot = commandWords[10] == "bot";
            int giveHighTo = Convert.ToInt32(commandWords[11]);

            workspace.EnsureBot(botId, giveLowToBot ? giveLowTo : botId, 
                giveHighToBot ? giveHighTo : botId);
            if (!giveLowToBot)
                workspace.EnsureOutput(giveLowTo);
            if (!giveHighToBot)
                workspace.EnsureOutput(giveHighTo);

            workspace.GetBot(botId).SendHigherValueTo = giveHighToBot ? (BaseNode)workspace.GetBot(giveHighTo) : 
                workspace.GetOutput(giveHighTo);
            workspace.GetBot(botId).SendLowerValueTo = giveLowToBot ? (BaseNode)workspace.GetBot(giveLowTo) : 
                workspace.GetOutput(giveLowTo);
        }

        private void DecodeValueCommand(string command, out int botId, out int value)
        {
            // example command: "value 11 goes to bot 43"
            string[] commandWords = command.Split(' ');
            
            value = Convert.ToInt32(commandWords[1]);
            botId = Convert.ToInt32(commandWords[5]);
        }
    }
}
