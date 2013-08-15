using System.Configuration;
using MongoDB.Driver;

namespace BrockAllen.MembershipReboot.MongoDb
{
    internal static class MongoDb
    {
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
