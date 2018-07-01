using ChewieBot.Services;
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
    /// Interaction logic for UserLevels.xaml
    /// </summary>
    public partial class UserLevels : UserControl
    {
        private UserLevelsViewModel viewModel;

        public UserLevels(IUserLevelService userLevelService)
        {
            InitializeComponent();

            this.viewModel = new UserLevelsViewModel(userLevelService);
            DataContext = this.viewModel;
        }
    }
}
