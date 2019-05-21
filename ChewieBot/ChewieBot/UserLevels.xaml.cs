using ChewieBot.Services;
using ChewieBot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private IUserLevelService userLevelService;

        public UserLevels(IUserLevelService userLevelService)
        {
            InitializeComponent();

            this.userLevelService = userLevelService;
            this.viewModel = new UserLevelsViewModel(userLevelService);
            DataContext = this.viewModel;
        }

        public void SaveLevels()
        {
            this.userLevelService.Set(this.viewModel.LevelList);
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            this.SaveLevels();
            this.SavedLabel.Visibility = Visibility.Visible;
            var timer = new Timer(5000);
            timer.Elapsed += (ts, te) =>
            {
                this.SavedLabel.Dispatcher.Invoke(() =>
                {
                    this.SavedLabel.Visibility = Visibility.Hidden;
                });
                timer.Stop();
            };
            timer.Start();
        }
    }
}
