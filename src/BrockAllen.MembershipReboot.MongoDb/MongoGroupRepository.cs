using System;
using System.Linq;
using MongoDB.Driver.Builders;

namespace BrockAllen.MembershipReboot.MongoDb
{
    public class MongoGroupRepository : IGroupRepository
    {
        public Group Create()
        {
            return new MongoGroup();
        }

        public IQueryable<Group> GetAll()
        {
            return MongoDb.Groups().FindAll().AsQueryable();
        }

        public Group Get(params object[] keys)
        {
            return MongoDb.Groups().FindOne(Query<MongoGroup>.EQ(e => e.ID, (Guid)keys[0]));
        }

        public void Add(Group item)
        {
            MongoDb.Groups().Insert((MongoGroup)item);
        }

        public void Update(Group item)
        {
            MongoDb.Groups().Save((MongoGroup)item);
        }

        public void Remove(Group item)
        {
            MongoDb.Groups().Remove(Query<MongoUserAccount>.EQ(e => e.ID, item.ID));
        }

        public void Dispose()
        {
        }
    }
}
