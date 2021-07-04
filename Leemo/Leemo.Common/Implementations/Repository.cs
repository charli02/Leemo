using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TPSS.Common.Interfaces;

/// <summary>
/// Represents Leemo data project
/// </summary>
namespace TPSS.Common.Implementations
{
    /// <summary>
    /// This generic repository class provide CRUD for the passed entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryBase<TEntity, TContext> :
        IRepository<TEntity> where TEntity : class
                                where TContext : DbContext, new()
    {

        private TContext _context;// = new TContext();
        public TContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        // not needed, just keeping it, will delete later
        protected DbSet<TEntity> _DbSet { get; set; }

        public RepositoryBase()
        {
        }

        public RepositoryBase(TContext context)
        {
            _context = context;
            _DbSet = _context.Set<TEntity>();
        }

        //public List<TEntity> GetAll()
        //{
        //    if (_DbSet.Count() > 0)
        //        return _DbSet.ToList();

        //    return new List<TEntity>();
        //}

        public IEnumerable<TEntity> GetAll()
        {
            IEnumerable<TEntity> query = _context.Set<TEntity>();
            return query;
        }

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> query = _context.Set<TEntity>().Where(predicate);
            return query;
        }

        public TEntity Get(Guid id)
        {
            return _DbSet.Find(id);
        }

        //3. Mark entity as modified
        public void MarkModified(TEntity entity)
        {
            _context.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            //_DbSet.Add(entity);
        }

        // Update/Save Entity in database
        public void Edit(TEntity entity)
        {
            _context.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            if (entity != null)
                _context.Set<TEntity>().Remove(entity);

            //_DbSet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            //_DbSet.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public void EditRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public int GetCount()
        {
            return _DbSet.Count();
        }

        public int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.Count(predicate);
        }

        //public virtual List<TEntity> GetWithRawSql(string query, params object[] parameters)
        //{
        //    return _DbSet.SqlQuery(query, parameters).ToList();
        //}
    }
}
