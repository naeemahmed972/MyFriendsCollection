using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MyFriendsCollection.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyFriendsCollection.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FriendDetailsPage : Page
    {
        public FriendDetailsViewModel ViewModel;

        public FriendDetailsPage()
        {
            ViewModel = App.HostContainer.Services.GetService<FriendDetailsViewModel>();

            this.InitializeComponent();

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string haveExplainedSaveSetting = localSettings.Values[nameof(SavingTip)] as string;

            if (!bool.TryParse(haveExplainedSaveSetting, out bool result) || !result)
            {
                SavingTip.IsOpen = true;
                localSettings.Values[nameof(SavingTip)] = "true";
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var friendId = (int)e.Parameter;

            if (friendId > 0)
            {
                ViewModel.InitializeFriendDetailData(friendId);
            }
        }
    }
}
