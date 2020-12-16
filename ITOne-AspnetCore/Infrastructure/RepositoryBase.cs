using Autofac;
using ITOne_AspnetCore.Api.User.Database;
using Microsoft.EntityFrameworkCore;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ITOne_AspnetCore.Infrastructure
{
    public class RepositoryBase<TEntity> : IDisposable, Lazarus.Common.DAL.IRepositoryBase<TEntity> where TEntity : class
    {
        public DbDataContext _db;
        public DbDataReadContext _DbRead;

        public RepositoryBase(DbDataContext db, DbDataReadContext DbRead)
        {
            _DbRead = DbRead;
            _db = db;
        }
        public void Dispose()
        {
            //   Db.Dispose();
        }

        public RepositoryBase()
        {

        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbRead.Set<TEntity>().Where(predicate).AsQueryable();
        }
        public void Delete(TEntity entity)
        {
            _db.Set<TEntity>().Remove(entity);
            _db.SaveChanges();
        }
        public TEntity Add(TEntity item)
        {
            try
            {
                _db.Add(item);
                _db.SaveChanges();
                return item;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
       
            }

        }

        public TEntity GetById(int id)
        {
            return _db.Set<TEntity>().Find(id);
        }

        public List<TEntity> Get()
        {

            return _DbRead.Set<TEntity>().ToList();
        }
        public void DetachAllEntities()
        {
            var changedEntriesCopy = _db.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public bool Update(TEntity item)
        {

            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {

                    _db.Entry(item).State = EntityState.Modified;
                    _db.SaveChanges();
                    return true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update original values from the database
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    //// Get the current entity values and the values in the database
                    //var entry = ex.Entries.Single();
                    //var currentValues = entry.CurrentValues;
                    //var databaseValues = entry.GetDatabaseValues();

                    //// Choose an initial set of resolved values. In this case we
                    //// make the default be the values currently in the database.
                    //var resolvedValues = databaseValues.Clone();

                    //// Have the user choose what the resolved values should be
                    //HaveUserResolveConcurrency(currentValues, databaseValues, resolvedValues);

                    //// Update the original values with the database values and
                    //// the current values with whatever the user choose.
                    //entry.OriginalValues.SetValues(databaseValues);
                    //entry.CurrentValues.SetValues(currentValues);


                }
            } while (saveFailed);
            return true;
            //try
            //{

            //    Db.Entry(item).State = EntityState.Modified;

            //    Db.SaveChanges();


            //    return true;
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    Console.WriteLine(ex.InnerException);
            //    throw ex;
            //}
        }
        public IQueryable<TEntity> Query()
        {
            return _db.Set<TEntity>();
        }

        public int Count()
        {
            return _db.Set<TEntity>().Count();
        }


    }
}
