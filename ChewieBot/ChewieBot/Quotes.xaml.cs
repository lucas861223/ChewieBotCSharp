using ChewieBot.AppStart;
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
    /// Interaction logic for Quotes.xaml
    /// </summary>
    public partial class Quotes : UserControl
    {
        private QuotesViewModel viewModel;
        public Quotes(IQuoteService quoteService)
        {
            InitializeComponent();

            quoteService.QuoteAddedEvent += (o, e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.viewModel.QuoteList = e.QuoteList;
                    DataContext = null;
                    DataContext = this.viewModel;
                });
            };

            this.viewModel = new QuotesViewModel(quoteService);
            DataContext = this.viewModel;
        }
    }
}
