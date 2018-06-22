using ChewieBot.AppStart;
using ChewieBot.Commands;
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

        public CommandList(ICommandRepository commandRepository)
        {
            InitializeComponent();
            this.viewModel = new CommandListViewModel(commandRepository);
            this.Dispatcher.Invoke(() =>
            {
                DataContext = this.viewModel;
            });
        }
    }
}
