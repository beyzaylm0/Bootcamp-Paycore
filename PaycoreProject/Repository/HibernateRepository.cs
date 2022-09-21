using NHibernate;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace PaycoreProject.Repository
{
    public class HibernateRepository<Entity> : IHibernateRepository<Entity> where Entity : class
    {
        private readonly ISession session;
        private ITransaction transaction;

        public HibernateRepository(ISession session)
        {
            this.session = session;
        }

        public IQueryable<Entity> Entities => session.Query<Entity>();

        //begin transaction
        public void BeginTransaction()
        {
            transaction = session.BeginTransaction();
        }
        // commit transaction
        public void Commit()
        {
            transaction.Commit();
        }

        // rollback transaction
        public void Rollback()
        {
            transaction.Rollback();
        }

        //close transection
        public void CloseTransaction()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        //save transection
        public void Save(Entity entity)
        {
            session.Save(entity);
        }
        // update transection
        public void Update(Entity entity)
        {
            session.Update(entity);
        }
        //delete transection
        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                session.Delete(entity);
            }
        }

        public List<Entity> GetAll()
        {
            return session.Query<Entity>().ToList();
        }

        public Entity GetById(int id)
        {
            var entity = session.Get<Entity>(id);
            return entity;
        }

        public IEnumerable<Entity> Where(Expression<Func<Entity, bool>> where)
        {
            return session.Query<Entity>().Where(where).AsQueryable();
        }

        public IEnumerable<Entity> Find(Expression<Func<Entity, bool>> expression)
        {
            return session.Query<Entity>().Where(expression).ToList();
        }

    }
}
