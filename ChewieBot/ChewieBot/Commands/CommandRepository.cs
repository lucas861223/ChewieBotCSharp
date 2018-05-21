using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Commands
{
    public class CommandRepository
    {
        private Dictionary<string, ICommand> commands;

        public CommandRepository()
        {
            this.commands = new Dictionary<string, ICommand>();
            this.LoadCommands();
        }

        public void LoadCommands()
        {
            this.commands.Clear();

            var rootFolderName = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var commandFolders = Directory.GetDirectories($"{rootFolderName}//Commands//CommandData");

            foreach (var folder in commandFolders)
            {
                switch (Path.GetFileName(folder).ToLower())
                {
                    case "queries":
                        {
                            var queryCommands = this.ParseCommands<QueryCommand>(Directory.GetFiles(folder));
                            this.commands = this.commands.Concat(queryCommands).ToDictionary(x => x.Key, x => x.Value);
                            break;
                        }
                    case "text":
                        {
                            var textCommands = this.ParseCommands<TextCommand>(Directory.GetFiles(folder));
                            this.commands = this.commands.Concat(textCommands).ToDictionary(x => x.Key, x => x.Value);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                
            }
            
        }

        private Dictionary<string, ICommand> ParseCommands<T>(string[] files)
            where T : BaseCommand
        {

            var dict = new Dictionary<string, ICommand>();
            foreach (var file in files)
            {
                using (var sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    var command = JsonConvert.DeserializeObject<T>(json);
                    dict.Add(command.Name, command);
                }
            }

            return dict;
        }
    }
}
