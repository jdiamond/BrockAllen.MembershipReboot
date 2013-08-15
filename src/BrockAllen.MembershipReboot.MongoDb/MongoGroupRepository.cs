using System;
using System.Linq;
using MongoDB.Driver.Builders;

namespace BrockAllen.MembershipReboot.MongoDb
{
    public class MongoGroupRepository : IGroupRepository
    {
        private MongoDb _db;

        public MongoGroupRepository(MongoDb db)
        {
            _db = db;
        }

        public Group Create()
        {
            return new MongoGroup();
        }

        public IQueryable<Group> GetAll()
        {
            return _db.Groups().FindAll().AsQueryable();
        }

        public Group Get(params object[] keys)
        {
            return _db.Groups().FindOne(Query<MongoGroup>.EQ(e => e.ID, (Guid)keys[0]));
        }

        public void Add(Group item)
        {
            _db.Groups().Insert((MongoGroup)item);
        }

        public void Update(Group item)
        {
            _db.Groups().Save((MongoGroup)item);
        }

        public void Remove(Group item)
        {
            _db.Groups().Remove(Query<MongoUserAccount>.EQ(e => e.ID, item.ID));
        }

        public void Dispose()
        {
        }
    }
}
