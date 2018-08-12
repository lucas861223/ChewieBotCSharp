using ChewieBot.Commands;
using ChewieBot.ViewModels;
using MahApps.Metro.Controls;
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
    /// Interaction logic for CommandEditor.xaml
    /// </summary>
    public partial class CommandEditor : MetroWindow
    {
        private CommandEditorViewModel viewModel;
        private ICommandRepository commandRepository;

        public CommandEditor(CommandEditorViewModel viewModel, ICommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            InitializeComponent();
        }

        public void SaveCommand()
        {
            var source = viewModel.CommandSource;
            this.commandRepository.UpdateCommandSource(viewModel.CommandName, viewModel.CommandSource);
        }
    }
}
