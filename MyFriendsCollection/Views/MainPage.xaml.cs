using Microsoft.UI.Xaml.Controls;
using MyFriendsCollection.ViewModels;
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyFriendsCollection.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel;

        public MainPage()
        {
            ViewModel = App.HostContainer.Services.GetService<MainViewModel>();
            this.InitializeComponent();
        }
    }
}
