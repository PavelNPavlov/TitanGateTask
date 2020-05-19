using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels.BaseModels;
using WebsiteWebApI.Infrastructure;

namespace WebsiteWebApI.Data.Repository
{
    public class GenericRepository<TDb, TEntity> : IDisposable, IGenericRepository<TDb, TEntity>
        where TDb : ApplicationDbContext
        where TEntity : BaseDbModel, new()
    {
        internal TDb context;
        internal DbSet<TEntity> dbSet;

        public IList<PropertyInfo> Properties { get; set; }

        public GenericRepository(TDb context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
            TEntity entry = new TEntity();

            this.Properties = typeof(TEntity)
               .GetProperties()
               .Where(x => x.PropertyType.IsClass && x.PropertyType.IsTypeDefinition && x.PropertyType.Name != "String")
               .ToList();



        }

        public virtual IQueryable<TEntity> All()
        {
            var filteredOutDelete = this.dbSet.Where(x => x.IsDeleted == false);
            return filteredOutDelete;
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Create(TEntity entity)
        {
            entity.DateCreated = UtilityConstants.Now;
            entity.DateModified = UtilityConstants.Now;

            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            entityToDelete.DateModified = UtilityConstants.Now;
            dbSet.Remove(entityToDelete);
        }

        public virtual void Edit(TEntity entityToUpdate)
        {
            entityToUpdate.DateModified = UtilityConstants.Now;
            dbSet.Attach(entityToUpdate);
            var t = context.Entry(entityToUpdate);
            context.SetState(entityToUpdate, EntityState.Modified);
            foreach (var item in this.Properties)
            {
                var value = item.GetValue(entityToUpdate);
                if (value != null)
                {
                    context.SetState(value, EntityState.Modified);
                    //context.Entry(value).State = EntityState.Modified;
                }
            }


        }
       


        public virtual void EditComplex(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;

        }

        public virtual int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual string GetPrimaryKey()
        {
            TEntity entryObj = new TEntity();
            var entry = context.Entry<TEntity>(entryObj);
            string keyName;
            try
            {
                keyName = entry.Metadata.FindPrimaryKey()
                         .Properties
                         .Select(p => p.Name).FirstOrDefault();
            }
            catch
            {
                keyName = "Id";
            }
            
            return keyName;
        }


    }

}
