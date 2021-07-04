using System;
using System.Collections.Generic;
using System.Linq.Expressions;

/// <summary>
/// Represents Leemo common project
/// </summary>
namespace TPSS.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        int GetCount();
        int GetCount(Expression<Func<TEntity, bool>> predicate);

        void MarkModified(TEntity entity);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Delete(int id);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);

        void Edit(TEntity entity);
        void EditRange(IEnumerable<TEntity> entities);

        void Save();

        // List<TEntity> GetWithRawSql(string query, params object[] parameters);
    }
}
