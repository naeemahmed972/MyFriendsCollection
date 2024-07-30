using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using MyFriendsCollection.Enums;
using MyFriendsCollection.Interfaces;
using MyFriendsCollection.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyFriendsCollection.Services
{
    public class SqliteDataService : IDataService
    {
        //private IList<FriendObject> _friends;
        private IList<FriendType> _friendTypes;
        private IList<Medium> _mediums;
        private IList<LocationType> _locationTypes;
        private const string DbName = "friendsCollectionData.db";

        //public DataService()
        //{
        //    PopulateFriendTypes();
        //    PopulateMediums();
        //    PopulateLocationTypes();
        //    PopulateFriends();
        //}

        private async Task<SqliteConnection> GetOpenConnectionAsync()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(DbName, CreationCollisionOption.OpenIfExists).AsTask();

            string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);

            var cn = new SqliteConnection($"Filename={dbPath}");
            cn.Open();

            return cn;
        }

        public async Task InitializeDataAsync()
        {
            using (var db = await GetOpenConnectionAsync())
            {
                await CreateMediumTableAsync(db);
                await CreateFriendTableAsync(db);

                PopulateFriendTypes();

                await PopulateMediumsAsync(db);

                PopulateLocationTypes();
            }
        }

        private async Task CreateMediumTableAsync(SqliteConnection db)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS Mediums 
                                    (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name NVARCHAR(30) NOT NULL,
                                    MediumType INTEGER NOT NULL)";

            using var createTable = new SqliteCommand(tableCommand, db);

            await createTable.ExecuteNonQueryAsync();
        }

        private async Task CreateFriendTableAsync(SqliteConnection db)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS Friends
                                    (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name NVARCHAR(1000) NOT NULL,
                                    FriendType INTEGER NOT NULL,
                                    MediumId INTEGER NOT NULL,
                                    LocationType INTEGER,
                                    CONSTRAINT fk_mediums FOREIGN KEY(MediumId) REFERENCES Mediums(Id))";

            using var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteNonQueryAsync();
        }

        private async Task InsertMediumAsync(SqliteConnection db, Medium medium)
        {
            //using var insertCommand = new SqliteCommand
            //{
            //    Connection = db,
            //    CommandText = "INSERT INTO Mediums VALUES (NULL, @Name, @MediumType);"
            //};

            //insertCommand.Parameters.AddWithValue("@Name", medium.Name);
            //insertCommand.Parameters.AddWithValue("@MediumType", (int)medium.Type);

            //await insertCommand.ExecuteNonQueryAsync();


            // using Dapper
            var newIds = await db.QueryAsync<long>(
                $@"INSERT INTO Mediums ({nameof(medium.Name)}, MediumType)
                VALUES (@{nameof(medium.Name)}, @{nameof(medium.Type)});
                SELECT last_insert_rowid()", medium);

            medium.Id = (int)newIds.First();
        }

        private async Task<IList<Medium>> GetAllMediumsAsync(SqliteConnection db)
        {
            //IList<Medium> mediums = new List<Medium>();

            //using var selectCommand = new SqliteCommand("SELECT Id, Name, Type FROM Mediums", db);
            //using SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            //while (query.Read())
            //{
            //    var medium = new Medium()
            //    {
            //        Id = query.GetInt32(0),
            //        Name = query.GetString(1),
            //        Type = (FriendType)query.GetInt32(2)
            //    };

            //    mediums.Add(medium);
            //}

            //return mediums;


            // using Dapper
            var mediums = await db.QueryAsync<Medium>(
                @"SELECT Id, Name, MediumType as Type FROM Mediums");
            return mediums.ToList();
        }

        private async Task<List<FriendObject>> GetAllFriendsAsync(SqliteConnection db)
        {
            var friendsResult = await db.QueryAsync<FriendObject, Medium, FriendObject>
                (
                    @"SELECT
                        [Friends].[Id],
                        [Friends].[Name],
                        [Friends].[FriendType] AS Type,
                        [Friends].[LocationType] AS Location,
                        [Mediums].[Id],
                        [Mediums].[Name],
                        [Mediums].[MediumType] AS Type
                    FROM
                        [Friends]
                    JOIN
                        [Mediums]
                    ON
                        [Mediums].[Id] = [Friends].[MediumId]",
                    (friend, medium) =>
                    {
                        friend.MediumInfo = medium;
                        return friend;
                    }
                );

            foreach (var friend in friendsResult.ToList())
            {
                Console.WriteLine(friend.Name);
            }
            return friendsResult.ToList();
        }

        private async Task<int> InsertFriendAsync(SqliteConnection db, FriendObject friend)
        {
            var newIds = await db.QueryAsync<long>
                (
                    @"INSERT INTO Friends
                        (Name, FriendType, MediumId, LocationType)
                    VALUES
                        (@Name, @Type, @MediumId, @Location);
                    SELECT
                        last_insert_rowid()", friend);

            return (int)newIds.First();
        }

        private async Task UpdateFriendAsync(SqliteConnection db, FriendObject friend)
        {
            await db.QueryAsync(
                @"UPDATE Friends
                    SET Name = @Name,
                        FriendType = @Type,
                        MediumID = @MediumId,
                        LocationType = @Location
                    WHERE Id = @Id;", friend);
        }

        private async Task DeleteFriendAsync(SqliteConnection db, int id)
        {
            await db.DeleteAsync<FriendObject>(new FriendObject { Id = id });
        }

        private async Task PopulateMediumsAsync(SqliteConnection db)
        {
            _mediums = await GetAllMediumsAsync(db);

            if (_mediums.Count == 0)
            {
                var brother = new Medium
                {
                    Id = 1,
                    Name = "Brother",
                    Type = FriendType.Relative
                };

                var colleague = new Medium
                {
                    Id = 2,
                    Name = "Colleague",
                    Type = FriendType.Jigri
                };

                var socialMedia = new Medium
                {
                    Id = 3,
                    Name = "Social Media",
                    Type = FriendType.GupShup
                };

                var cousin = new Medium
                {
                    Id = 4,
                    Name = "Cousin",
                    Type = FriendType.Relative
                };

                var neighbor = new Medium
                {
                    Id = 5,
                    Name = "Neighbor",
                    Type = FriendType.Jigri
                };

                var guide = new Medium
                {
                    Id = 6,
                    Name = "Guide",
                    Type = FriendType.GupShup
                };

                var mediums = new List<Medium>
                {
                    brother, colleague, socialMedia, cousin, neighbor, guide
                };

                foreach (var medium in mediums)
                {
                    await InsertMediumAsync(db, medium);
                }

                _mediums = await GetAllMediumsAsync(db);
            }
        }

        private void PopulateFriendTypes()
        {
            _friendTypes = new List<FriendType>
            {
                FriendType.Relative,
                FriendType.Jigri,
                FriendType.GupShup
            };
        }

        private void PopulateLocationTypes()
        {
            _locationTypes = new List<LocationType>
            {
                LocationType.Local,
                LocationType.Abroad
            };
        }

        public async Task<int> AddFriendAsync(FriendObject friend)
        {
            using var db = await GetOpenConnectionAsync();
            return await InsertFriendAsync(db, friend);
        }

        public async Task<FriendObject> GetFriendAsync(int id)
        {
            IList<FriendObject> friends;

            using var db = await GetOpenConnectionAsync();
            friends = await GetAllFriendsAsync(db);

            return friends.FirstOrDefault(i => i.Id == id);
        }

        public async Task<IList<FriendObject>> GetFriendsAsync()
        {
            using var db = await GetOpenConnectionAsync();
            return await GetAllFriendsAsync(db);
        }

        public async Task UpdateFriendAsync(FriendObject friend)
        {
            using var db = await GetOpenConnectionAsync();
            await UpdateFriendAsync(db, friend);
        }

        public async Task DeleteFriendAsync(FriendObject friend)
        {
            using var db = await GetOpenConnectionAsync();
            await DeleteFriendAsync(db, friend.Id);
        }

        public IList<FriendType> GetFriendTypes()
        {
            return _friendTypes;
        }

        public IList<Medium> GetMediums()
        {
            return _mediums;
        }

        public IList<Medium> GetMediums(FriendType type)
        {
            return _mediums.Where(m => m.Type == type).ToList();
        }

        public Medium GetMedium(string name)
        {
            return _mediums.FirstOrDefault(m => m.Name == name);
        }

        public Medium GetMedium(int id)
        {
            return _mediums.FirstOrDefault(m => m.Id == id);
        }

        public IList<LocationType> GetLocationTypes()
        {
            return _locationTypes;
        }
    }
}
