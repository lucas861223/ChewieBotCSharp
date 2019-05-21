using ChewieBot.AppStart;
using ChewieBot.Constants;
using ChewieBot.Constants.SettingsConstants;
using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.Models;
using ChewieBot.ScriptingEngine;
using ChewieBot.Services;
using ChewieBot.Twitch;
using ChewieBot.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Events;

namespace ChewieBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ITwitchService twitchService = UnityConfig.Resolve<ITwitchService>();
        private IBotSettingService botSettingService = UnityConfig.Resolve<IBotSettingService>();
        private IDeepbotService deepbotService = UnityConfig.Resolve<IDeepbotService>();
        private IConfigService configService = UnityConfig.Resolve<IConfigService>();

        private MainWindowViewModel viewModel;
        private OAuthWebpage popoutOAuth;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSetup();
        }

        private void InitializeSetup()
        {
            UnityConfig.Setup();
            DatabaseSetup.Setup();

            //this.deepbotService.LoadDeepbotUsersFromFile();

            this.twitchService.InitializeClient();
            this.twitchService.OnConnectedEvent += OnConnected;
            this.twitchService.OnDisconnectedEvent += OnDisconnected;
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            if (!String.IsNullOrEmpty(this.configService.Get(AppConstants.ConfigKeys.StreamlabsToken)))
            {
                viewModel.StreamlabsAuthButton = AppConstants.StreamlabsAuthButton.Connected;
                this.StreamlabsAuthBtn.IsEnabled = false;
            }
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        private void MenuItemClicked(object sender, MouseButtonEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                viewModel.Title = ((e.Source as ListBox).SelectedItem as MenuLink).Name;
            });
        }

        private void ConnectButtonClicked(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (this.twitchService.IsConnected)
                {
                    this.twitchService.Disconnect();                    
                }
                else
                {
                    this.twitchService.Connect();
                    this.viewModel.ConnectStatus = AppConstants.ConnectStatus.Connecting;
                }
            });
        }

        private void StreamlabsAuthButtonClicked(object sender, EventArgs e)
        {
            popoutOAuth = new OAuthWebpage(UnityConfig.Resolve<IKeyValueData>());
            popoutOAuth.Show();

            popoutOAuth.Closed += (cs, ce) =>
            {
                popoutOAuth = null;
            };
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            this.viewModel.ConnectButton = AppConstants.ConnectButton.Disconnect;
            this.viewModel.ConnectStatus = AppConstants.ConnectStatus.Connected;
            this.viewModel.ConnectColour = AppConstants.ConnectStatus.ConnectedColourHex;
        }

        private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            this.viewModel.ConnectButton = AppConstants.ConnectButton.Connect;
            this.viewModel.ConnectStatus = AppConstants.ConnectStatus.NotConnected;
            this.viewModel.ConnectColour = AppConstants.ConnectStatus.NotConnectedColourHex;
        }

        private void InitializeTwitchClient()
        {
            if (twitchService != null)
            {
                twitchService.Connect();
            }
        }

        public static T FindAncestor<T>(DependencyObject obj)
            where T : DependencyObject
        {
            while (obj != null)
            {
                T o = obj as T;
                if (o != null)
                {
                    return o;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            return default(T);
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
