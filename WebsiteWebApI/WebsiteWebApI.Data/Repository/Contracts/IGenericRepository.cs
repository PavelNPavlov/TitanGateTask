using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebsiteWebApI.Data.Repository.Contracts
{
    public interface IGenericRepository<TDb, TEntity>
        where TEntity : class
        where TDb : DbContext
    {

        IQueryable<TEntity> All();
        void Create(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Dispose();
        void Edit(TEntity entityToUpdate);
        TEntity GetByID(object id);
        int SaveChanges();
        string GetPrimaryKey();
    }

}
