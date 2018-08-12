using ChewieBot.AppStart;
using ChewieBot.Commands;
using ChewieBot.Models;
using ChewieBot.ScriptingEngine;
using ChewieBot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChewieBot
{
    /// <summary>
    /// Interaction logic for CommandList.xaml
    /// </summary>
    public partial class CommandList : UserControl
    {
        private CommandListViewModel viewModel;
        private CommandEditor commandEditor;
        private ICommandRepository commandRepository;

        public CommandList(ICommandRepository commandRepository)
        {
            InitializeComponent();
            this.commandRepository = commandRepository;
            this.viewModel = new CommandListViewModel(commandRepository);
            this.Dispatcher.Invoke(() =>
            {
                DataContext = this.viewModel;
            });
        }

        private void EditCommand(object sender, RoutedEventArgs e)
        {
            var commandItem = (e.Source as MenuItem).DataContext as CommandListItem;
            this.OpenCommandEditor(commandItem.CommandName);
        }

        public void OpenCommandEditor(string commandName)
        {
            if (commandEditor == null)
            {
                var commandSource = this.commandRepository.GetCommandSource(commandName);
                this.commandEditor = new CommandEditor(new CommandEditorViewModel(commandName, commandSource), this.commandRepository);
                this.commandEditor.Show();

                this.commandEditor.Closed += (o, args) =>
                {
                    this.commandEditor.SaveCommand();
                    this.commandEditor = null;
                    this.Dispatcher.Invoke(() =>
                    {
                        DataContext = null;
                        this.viewModel.ReloadCommands();
                        DataContext = this.viewModel;
                    });
                };
            }
        }
    }
}
