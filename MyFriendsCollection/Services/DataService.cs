//using MyFriendsCollection.Interfaces;
//using MyFriendsCollection.Enums;
//using MyFriendsCollection.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.UI.Xaml.Controls.Primitives;

//namespace MyFriendsCollection.Services
//{
//    public class DataService : IDataService
//    {
//        private IList<FriendObject> _friends;
//        private IList<FriendType> _friendTypes;
//        private IList<Medium> _mediums;
//        private IList<LocationType> _locationTypes;

//        public DataService()
//        {
//            PopulateFriendTypes();
//            PopulateMediums();
//            PopulateLocationTypes();
//            PopulateFriends();
//        }

//        private void PopulateFriends()
//        {
//            var abid = new FriendObject
//            {
//                Id = 1,
//                Name = "Abid Jameel",
//                Type = FriendType.Jigri,
//                MediumInfo = _mediums.FirstOrDefault(m => m.Name == "Colleague"),
//                Location = LocationType.Local
//            };

//            var mushtaq = new FriendObject
//            {
//                Id = 2,
//                Name = "Mushtaq Ahmed",
//                Type = FriendType.Relative,
//                MediumInfo = _mediums.FirstOrDefault(m => m.Name == "Brother"),
//                Location = LocationType.Local
//            };

//            var arshad = new FriendObject
//            {
//                Id = 3,
//                Name = "Muhammad Arshad",
//                Type = FriendType.Jigri,
//                MediumInfo = _mediums.FirstOrDefault(m => m.Name == "Colleague"),
//                Location = LocationType.Abroad
//            };

//            _friends = new List<FriendObject>
//            {
//                abid, mushtaq, arshad
//            };
//        }

//        private void PopulateMediums()
//        {
//            var brother = new Medium
//            {
//                Id = 1,
//                Name = "Brother",
//                Type = FriendType.Relative
//            };

//            var colleague = new Medium
//            {
//                Id = 2,
//                Name = "Colleague",
//                Type = FriendType.Jigri
//            };

//            var socialMedia = new Medium
//            {
//                Id = 3,
//                Name = "Social Media",
//                Type = FriendType.GupShup
//            };

//            var cousin = new Medium
//            {
//                Id = 4,
//                Name = "Cousin",
//                Type = FriendType.Relative
//            };

//            var neighbor = new Medium
//            {
//                Id = 5,
//                Name = "Neighbor",
//                Type = FriendType.Jigri
//            };

//            var guide = new Medium
//            {
//                Id = 6,
//                Name = "Guide",
//                Type = FriendType.GupShup
//            };

//            _mediums = new List<Medium>
//            {
//                brother, colleague, socialMedia, cousin, neighbor, guide
//            };
//        }

//        private void PopulateFriendTypes()
//        {
//            _friendTypes = new List<FriendType>
//            {
//                FriendType.Relative,
//                FriendType.Jigri,
//                FriendType.GupShup
//            };
//        }

//        private void PopulateLocationTypes()
//        {
//            _locationTypes = new List<LocationType>
//            {
//                LocationType.Local,
//                LocationType.Abroad
//            };
//        }

//        public int AddFriend(FriendObject friend)
//        {
//            friend.Id = _friends.Max(i => i.Id) + 1;
//            _friends.Add(friend);

//            return friend.Id;
//        }

//        public FriendObject GetFriend(int id)
//        {
//            return _friends.FirstOrDefault(i => i.Id == id);
//        }

//        public IList<FriendObject> GetFriends()
//        {
//            return _friends;
//        }

//        public IList<FriendType> GetFriendTypes()
//        {
//            return _friendTypes;
//        }

//        public IList<Medium> GetMediums()
//        {
//            return _mediums;
//        }

//        public IList<Medium> GetMediums(FriendType type)
//        {
//            return _mediums.Where(m => m.Type == type).ToList();
//        }

//        public IList<LocationType> GetLocationTypes()
//        {
//            return _locationTypes;
//        }

//        public void UpdateFriend(FriendObject friend)
//        {
//            var index = -1;

//            var matchedFriend = (
//                from x in _friends
//                let friendIndex = index++
//                where x.Id == friend.Id
//                select friendIndex).FirstOrDefault();

//            if (index == -1)
//            {
//                throw new Exception("Unable to update friend. Friend not found in the collection.");
//            }

//            _friends[index] = friend;
//        }

//        public Medium GetMedium(string name)
//        {
//            return _mediums.FirstOrDefault(m => m.Name == name);
//        }
//    }
//}
