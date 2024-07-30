using MyFriendsCollection.Enums;
using MyFriendsCollection.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFriendsCollection.Interfaces
{
    public interface IDataService
    {
        Task<IList<FriendObject>> GetFriendsAsync();
        Task<FriendObject> GetFriendAsync(int id);
        Task<int> AddFriendAsync(FriendObject friend);
        Task UpdateFriendAsync(FriendObject friend);
        Task DeleteFriendAsync(FriendObject friend);
        IList<FriendType> GetFriendTypes();
        Medium GetMedium(string name);
        IList<Medium> GetMediums();
        IList<Medium> GetMediums(FriendType friendType);
        IList<LocationType> GetLocationTypes();
        Task InitializeDataAsync();



        //IList<FriendObject> GetFriends();
        //FriendObject GetFriend(int id);
        //int AddFriend(FriendObject friend);
        //void UpdateFriend(FriendObject friend);
        //IList<FriendType> GetFriendTypes();
        //Medium GetMedium(string name);
        //IList<Medium> GetMediums();
        //IList<Medium> GetMediums(FriendType friendType);
        //IList<LocationType> GetLocationTypes();
    }
}
