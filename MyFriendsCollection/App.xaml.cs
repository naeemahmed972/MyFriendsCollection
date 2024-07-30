using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MyFriendsCollection.Interfaces;
using MyFriendsCollection.Services;
using MyFriendsCollection.ViewModels;
using MyFriendsCollection.Views;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyFriendsCollection
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window m_window;
        public static IHost HostContainer { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        

        //protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        //{
        //    m_window = new MainWindow();

        //    var rootFrame = new Frame();
        //    RegisterComponents(rootFrame);
        //    rootFrame.NavigationFailed += RootFrame_NavigationFailed;
        //    rootFrame.Navigate(typeof(MainPage), args);

        //    m_window.Content = rootFrame;
        //    m_window.Activate();
        //}

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            var rootFrame = new Frame();
            await RegisterComponentsAsync(rootFrame);
            rootFrame.NavigationFailed += RootFrame_NavigationFailed;
            rootFrame.Navigate(typeof(MainPage), args);
            m_window.Content = rootFrame;
            m_window.Activate();
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new System.Exception($"Error loading page {e.SourcePageType.FullName}");
        }

        //private void RegisterComponents(Frame rootFrame)
        //{
        //    var navigationService = new NavigationService(rootFrame);
        //    navigationService.Configure(nameof(MainPage), typeof(MainPage));
        //    navigationService.Configure(nameof(FriendDetailsPage), typeof(FriendDetailsPage));

        //    HostContainer = Host.CreateDefaultBuilder()
        //        .ConfigureServices(services =>
        //        {
        //            services.AddSingleton<INavigationService>(navigationService);
        //            services.AddSingleton<IDataService, DataService>();
        //            services.AddTransient<MainViewModel>();
        //            services.AddTransient<FriendDetailsViewModel>();
        //        }).Build();
        //}

        private async Task RegisterComponentsAsync(Frame rootFrame)
        {
            var navigationService = new NavigationService(rootFrame);
            navigationService.Configure(nameof(MainPage), typeof(MainPage));
            navigationService.Configure(nameof(FriendDetailsPage), typeof(FriendDetailsPage));
            var dataService = new SqliteDataService();
            await dataService.InitializeDataAsync();

            HostContainer = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<INavigationService>(navigationService);
                    services.AddSingleton<IDataService>(dataService);
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<FriendDetailsViewModel>();
                }).Build();
        }
    }
}
