﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace PaycoreProject.Repository
{
    public interface IHibernateRepository<Entity> where Entity : class
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        void CloseTransaction();
        void Save(Entity entity);
        void Update(Entity entity);
        void Delete(int id);
        List<Entity> GetAll();
        Entity GetById(int id);
        IEnumerable<Entity> Find(Expression<Func<Entity, bool>> expression);
        IEnumerable<Entity> Where(Expression<Func<Entity, bool>> where);

        IQueryable<Entity> Entities { get; }
    }
}
