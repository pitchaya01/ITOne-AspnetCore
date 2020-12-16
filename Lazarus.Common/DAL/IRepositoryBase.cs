using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.DAL
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        TEntity Add(TEntity item);
        TEntity GetById(int id);
        List<TEntity> Get();
        bool Update(TEntity item);
        int Count();
        void Dispose();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity entity);
        IQueryable<TEntity> Query();
        void Commit();
    }


}
