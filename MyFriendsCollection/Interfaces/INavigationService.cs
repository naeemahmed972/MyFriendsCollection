using Windows.Media.AppBroadcasting;

namespace MyFriendsCollection.Interfaces
{
    public interface INavigationService
    {
        string CurrentPage { get; }
        void NavigateTo(string page);
        void NavigateTo(string page, object parameter);
        void GoBack();
    }
}
