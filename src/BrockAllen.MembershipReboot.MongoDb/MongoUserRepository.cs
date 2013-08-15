using System;
using System.Linq;
using MongoDB.Driver.Builders;

namespace BrockAllen.MembershipReboot.MongoDb
{
    public class MongoUserRepository : IUserAccountRepository
    {
        public UserAccount Create()
        {
            return new MongoUserAccount();
        }
        
        public IQueryable<UserAccount> GetAll()
        {
            return MongoDb.Users().FindAll().AsQueryable();
        }

        public UserAccount Get(params object[] keys)
        {
            return MongoDb.Users().FindOne(Query<MongoUserAccount>.EQ(e => e.ID, (Guid)keys[0]));
        }

        public void Add(UserAccount item)
        {
            MongoDb.Users().Insert((MongoUserAccount)item);
        }

        public void Update(UserAccount item)
        {
            MongoDb.Users().Save((MongoUserAccount)item);
        }

        public void Remove(UserAccount item)
        {
            MongoDb.Users().Remove(Query<MongoUserAccount>.EQ(e => e.ID, item.ID));
        }

        public void Dispose()
        {
        }
    }
}
