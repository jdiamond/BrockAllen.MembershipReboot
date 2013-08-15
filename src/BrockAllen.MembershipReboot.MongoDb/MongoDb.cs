﻿using System.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BrockAllen.MembershipReboot.MongoDb
{
    public class MongoDb
    {
        static MongoDb()
        {
            BsonClassMap.RegisterClassMap<UserAccount>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.ID);
            });
        }

        private readonly string _connectionStringName;

        public MongoDb(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public MongoCollection<MongoGroup> Groups()
        {
            return GetCollection<MongoGroup>("groups");
        }

        public MongoCollection<MongoUserAccount> Users()
        {
            return GetCollection<MongoUserAccount>("users");
        }

        public MongoCollection<T> GetCollection<T>(string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(databaseName);
            return database.GetCollection<T>(name);
        }
    }
}
