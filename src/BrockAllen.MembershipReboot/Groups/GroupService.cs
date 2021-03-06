﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockAllen.MembershipReboot
{
    public class GroupService
    {
        MembershipRebootConfiguration configuration;
        IGroupRepository groupRepository;

        public SecuritySettings SecuritySettings
        {
            get
            {
                return configuration.SecuritySettings;
            }
        }

        public GroupService(IGroupRepository groupRepository)
            : this(new MembershipRebootConfiguration(), groupRepository)
        {
        }

        public GroupService(MembershipRebootConfiguration configuration, IGroupRepository groupRepository)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (groupRepository == null) throw new ArgumentNullException("groupRepository");

            this.configuration = configuration;
            this.groupRepository = groupRepository;
        }

        public IQueryable<Group> GetAll()
        {
            return GetAll(null);
        }
        
        public IQueryable<Group> GetAll(string tenant)
        {
            if (!SecuritySettings.MultiTenant)
            {
                tenant = SecuritySettings.DefaultTenant;
            }

            if (String.IsNullOrWhiteSpace(tenant)) throw new ArgumentNullException("tenant");

            return this.groupRepository.GetAll().Where(x=>x.Tenant == tenant);
        }

        public Group Get(Guid groupID)
        {
            return this.groupRepository.Get(groupID);
        }

        public Group Create(string name)
        {
            return Create(null, name);
        }

        bool NameAlreadyExists(string tenant, string name, Guid? exclude = null)
        {
            var query = GetAll(tenant).Where(x => x.Name == name);
            if (exclude.HasValue)
            {
                query = query.Where(x => x.ID != exclude.Value);
            }
            return query.Any();
        }

        public Group Create(string tenant, string name)
        {
            if (!SecuritySettings.MultiTenant)
            {
                tenant = SecuritySettings.DefaultTenant;
            }

            if (NameAlreadyExists(tenant, name))
            {
                throw new ValidationException("That name is already in use.");
            }

            var grp = this.groupRepository.Create();
            grp.Init(tenant, name);

            this.groupRepository.Add(grp);

            return grp;
        }

        public void Delete(Guid groupID)
        {
            var grp = Get(groupID);
            if (grp == null) throw new ArgumentException("Invalid GroupID");

            this.groupRepository.Remove(grp);
            RemoveChildGroupFromOtherGroups(groupID);
        }

        private void RemoveChildGroupFromOtherGroups(Guid childGroupID)
        {
            var query =
                from g in this.groupRepository.GetAll()
                from c in g.Children
                where c.ChildGroupID == childGroupID
                select g;
            foreach (var group in query.ToArray())
            {
                group.RemoveChild(childGroupID);
                Update(group);
            }
        }

        private void Update(Group group)
        {
            group.LastUpdated = group.UtcNow;
            this.groupRepository.Update(group);
        }

        public void ChangeName(Guid groupID, string name)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ValidationException("Invalid name.");

            var group = Get(groupID);
            if (group == null) throw new ArgumentException("Invalid GroupID");
            
            if (NameAlreadyExists(group.Tenant, name, groupID))
            {
                throw new ValidationException("That name is already in use.");
            }

            group.Name = name;
            Update(group);
        }

        public void AddChildGroup(Guid groupID, Guid childGroupID)
        {
            var group = Get(groupID);
            if (group == null) throw new ArgumentException("Invalid GroupID");

            var childGroup = Get(childGroupID);
            if (childGroup == null) throw new ArgumentException("Invalid ChildGroupID");

            group.AddChild(childGroupID);
            Update(group);
        }
        
        public void RemoveChildGroup(Guid groupID, Guid childGroupID)
        {
            var group = Get(groupID);
            if (group == null) throw new ArgumentException("Invalid GroupID");

            group.RemoveChild(childGroupID);
            Update(group);
        }
    }
}
