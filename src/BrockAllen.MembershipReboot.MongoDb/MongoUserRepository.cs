using System;
using System.Linq;
using MongoDB.Driver.Builders;

namespace BrockAllen.MembershipReboot.MongoDb
{
    public class MongoUserRepository : IUserAccountRepository
    {
        private MongoDb _db;

        public MongoUserRepository(MongoDb db)
        {
            _db = db;
        }

        public UserAccount Create()
        {
            return new MongoUserAccount();
        }
        
        public IQueryable<UserAccount> GetAll()
        {
            return _db.Users().FindAll().AsQueryable();
        }

        public UserAccount Get(params object[] keys)
        {
            return _db.Users().FindOne(Query<MongoUserAccount>.EQ(e => e.ID, (Guid)keys[0]));
        }

        public void Add(UserAccount item)
        {
            _db.Users().Insert((MongoUserAccount)item);
        }

        public void Update(UserAccount item)
        {
            _db.Users().Save((MongoUserAccount)item);
        }

        public void Remove(UserAccount item)
        {
            _db.Users().Remove(Query<MongoUserAccount>.EQ(e => e.ID, item.ID));
        }

        public void Dispose()
        {
        }
    }
}
