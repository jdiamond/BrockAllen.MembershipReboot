using System.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BrockAllen.MembershipReboot.MongoDb
{
    internal static class MongoDb
    {
        static MongoDb()
        {
            BsonClassMap.RegisterClassMap<UserAccount>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.ID);
            });
        }

        public static MongoCollection<MongoGroup> Groups()
        {
            return GetCollection<MongoGroup>("groups");
        }

        public static MongoCollection<MongoUserAccount> Users()
        {
            return GetCollection<MongoUserAccount>("users");
        }

        public static MongoCollection<T> GetCollection<T>(string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(databaseName);
            return database.GetCollection<T>(name);
        }
    }
}
