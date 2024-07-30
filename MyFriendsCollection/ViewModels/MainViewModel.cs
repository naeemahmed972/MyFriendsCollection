using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Input;
using System.Collections.ObjectModel;
using MyFriendsCollection.Interfaces;
using MyFriendsCollection.Model;
using System.Threading.Tasks;
using System;

namespace MyFriendsCollection.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string selectedMedium;

        [ObservableProperty]
        private ObservableCollection<FriendObject> friends = new ObservableCollection<FriendObject>();

        [ObservableProperty]
        private ObservableCollection<string> mediums;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private FriendObject selectedFriend;

        private ObservableCollection<FriendObject> allFriends;

        private INavigationService _navigationService;
        private IDataService _dataService;
        private const string AllMediums = "All";

        public MainViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            //PopulateData();
            PopulateDataAsync();
        }

        //public void PopulateData()
        //{
        //    Friends.Clear();

        //    foreach(var friend in _dataService.GetFriends())
        //    {
        //        Friends.Add(friend);
        //    }

        //    allFriends = new ObservableCollection<FriendObject>(Friends);

        //    Mediums = new ObservableCollection<string>
        //    {
        //        AllMediums
        //    };

        //    foreach (var friendType in _dataService.GetFriendTypes())
        //    {
        //        Mediums.Add(friendType.ToString());
        //    }

        //    SelectedMedium = Mediums[0];
        //}

        public async Task PopulateDataAsync()
        {
            Friends.Clear();

            foreach (var friend in await _dataService.GetFriendsAsync())
            {
                Friends.Add(friend);
            }

            allFriends = new ObservableCollection<FriendObject>(Friends);

            Mediums = new ObservableCollection<string>
            {
                AllMediums
            };

            foreach (var friendType in _dataService.GetFriendTypes())
            {
                Mediums.Add(friendType.ToString());
            }

            SelectedMedium = Mediums[0];
        }

        partial void OnSelectedMediumChanged(string value)
        {
            Friends.Clear();

            foreach(var friend in allFriends)
            {
                if (
                    string.IsNullOrWhiteSpace(value) ||
                    value == "All" ||
                    value == friend.Type.ToString()
                   )
                {
                    Friends.Add(friend);
                }
            }
        }

        [RelayCommand]
        public void AddEdit()
        {
            var selectedFriendId = -1;

            if (SelectedFriend != null)
            {
                selectedFriendId = SelectedFriend.Id;
            }

            _navigationService.NavigateTo("FriendDetailsPage", selectedFriendId);
        }

        public void ListViewDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            AddEdit();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteFriend))]
        private async Task DeleteAsync()
        {
            await _dataService.DeleteFriendAsync(SelectedFriend);
            allFriends.Remove(SelectedFriend);
            Friends.Remove(SelectedFriend);
        }
        //public void Delete()
        //{
        //    allFriends.Remove(SelectedFriend);
        //    Friends.Remove(SelectedFriend);
        //}

        private bool CanDeleteFriend() => SelectedFriend != null;
    }
}
