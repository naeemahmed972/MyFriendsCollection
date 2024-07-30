using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using MyFriendsCollection.Enums;
using MyFriendsCollection.Interfaces;
using MyFriendsCollection.Model;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;

namespace MyFriendsCollection.ViewModels
{
    public partial class FriendDetailsViewModel : ObservableObject
    {
        private int _friendId;
        private int _selectedFriendId = -1;
        protected INavigationService _navigationService;
        protected IDataService _dataService;

        [ObservableProperty]
        private ObservableCollection<string> locationTypes = new();

        [ObservableProperty]
        private ObservableCollection<string> mediums = new();

        [ObservableProperty]
        private ObservableCollection<string> friendTypes = new();

        [ObservableProperty]
        private string friendName;

        [ObservableProperty]
        private string selectedMedium;

        [ObservableProperty]
        private string selectedFriendType;

        [ObservableProperty]
        private string selectedLocation;

        [ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private bool isDirty;

        public FriendDetailsViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            PopulateLists();
        }

        public async Task InitializeFriendDetailData(int friendId)
        {
            _selectedFriendId = friendId;

            await PopulateExistingFriend(_dataService);
            IsDirty = false;
        }

        private async Task PopulateExistingFriend(IDataService dataService)
        {
            if (_selectedFriendId > 0)
            {
                var friend = await _dataService.GetFriendAsync(_selectedFriendId);

                Mediums.Clear();

                foreach (string medium in dataService.GetMediums(friend.Type).Select(m => m.Name))
                {
                    Mediums.Add(medium);
                }

                _friendId = friend.Id;
                FriendName = friend.Name;
                SelectedMedium = friend.MediumInfo.Name;
                SelectedLocation = friend.Location.ToString();
                SelectedFriendType = friend.Type.ToString();
            }
        }

        private void PopulateLists()
        {
            FriendTypes.Clear();

            foreach (string friendType in Enum.GetNames(typeof(FriendType)))
            {
                FriendTypes.Add(friendType);
            }

            LocationTypes.Clear();

            foreach (string locationType in Enum.GetNames(typeof(LocationType)))
            {
                LocationTypes.Add(locationType);
            }

            Mediums = new ObservableCollection<string>();
        }

        private async Task SaveAsync()
        {
            FriendObject friend;

            if (_friendId > 0)
            {
                friend = await _dataService.GetFriendAsync(_friendId);

                friend.Name = FriendName;
                friend.Location = (LocationType)Enum.Parse(typeof(LocationType), SelectedLocation);
                friend.Type = (FriendType)Enum.Parse(typeof(FriendType), SelectedFriendType);
                friend.MediumInfo = _dataService.GetMedium(SelectedMedium);

                await _dataService.UpdateFriendAsync(friend);
            }
            else
            {
                friend = new FriendObject
                {
                    Name = FriendName,
                    Location = (LocationType)Enum.Parse(typeof(LocationType), SelectedLocation),
                    Type = (FriendType)Enum.Parse(typeof(FriendType), SelectedFriendType),
                    MediumInfo = _dataService.GetMedium(SelectedMedium)
                };

                await _dataService.AddFriendAsync(friend);
            }
        }

        public async Task SaveFriendAndContinueAsync()
        {
            await SaveAsync();

            _friendId = 0;
            FriendName = string.Empty;
            SelectedMedium = null;
            SelectedLocation = null;
            SelectedFriendType = null;
            IsDirty = false;
        }

        public async Task SaveFriendAndReturnAsync()
        {
            await SaveAsync();
            _navigationService.GoBack();
        }

        //[RelayCommand(CanExecute = nameof(CanSaveFriend))]
        //private void Save()
        //{
        //    FriendObject friend;

        //    if (_friendId > 0)
        //    {
        //        friend = _dataService.GetFriend(_friendId);

        //        friend.Name = FriendName;
        //        friend.Location = (LocationType)Enum.Parse(typeof(LocationType), SelectedLocation);
        //        friend.Type = (FriendType)Enum.Parse(typeof(FriendType), SelectedFriendType);
        //        friend.MediumInfo = _dataService.GetMedium(SelectedMedium);

        //        _dataService.UpdateFriend(friend);
        //    }
        //    else
        //    {
        //        friend = new FriendObject
        //        {
        //            Name = FriendName,
        //            Location = (LocationType)Enum.Parse(typeof(LocationType), SelectedLocation),
        //            Type = (FriendType)Enum.Parse(typeof(FriendType), SelectedFriendType),
        //            MediumInfo = _dataService.GetMedium(SelectedMedium)
        //        };
        //        _dataService.AddFriend(friend);
        //    }
        //    //_navigationService.GoBack(); // new save method created
        //}

        //public void SaveFriendAndContinue()
        //{
        //    Save();
        //    _friendId = 0;
        //    FriendName = string.Empty;
        //    SelectedMedium = null;
        //    SelectedLocation = null;
        //    SelectedFriendType = null;
        //    IsDirty = false;
        //}

        //public void SaveFriendAndReturn()
        //{
        //    Save();
        //    _navigationService.GoBack();
        //}

        //private bool CanSaveFriend()
        //{
        //    return IsDirty;
        //}

        partial void OnFriendNameChanged(string value)
        {
            IsDirty = true;
        }

        partial void OnSelectedMediumChanged(string value)
        {
            IsDirty = true;
        }

        partial void OnSelectedFriendTypeChanged(string value)
        {
            IsDirty = true;
            Mediums.Clear();

            if (!string.IsNullOrWhiteSpace(value))
            {
                foreach (string med in _dataService.GetMediums((FriendType)Enum.Parse(typeof(FriendType), SelectedFriendType)).Select(m => m.Name))
                {
                    Mediums.Add(med);
                }
            }
        }

        partial void OnSelectedLocationChanged(string value)
        {
            IsDirty = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            _navigationService.GoBack();
        }
    }
}
