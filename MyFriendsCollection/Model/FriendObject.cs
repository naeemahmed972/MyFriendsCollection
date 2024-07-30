using Dapper.Contrib.Extensions;
using MyFriendsCollection.Enums;

namespace MyFriendsCollection.Model
{
    public class FriendObject
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public FriendType Type { get; set; }

        [Computed]
        public Medium MediumInfo { get; set; }

        public LocationType Location {  get; set; }
        public int MediumId => MediumInfo.Id;
    }
}
