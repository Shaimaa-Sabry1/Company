using CompanyMVC.BLL.Interfaces;
using CompanyMVC.DAL.Data.Contexts;
using CompanyMVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyMVC.BLL.Repositories
{
   public class GenericRepository<T> : IGenericRepository<T> where T:BaseEntity
    {
        private readonly CompanyDbContext _dbContext;
        public GenericRepository(CompanyDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public int Add(T model)
        {
            _dbContext.Set<T>().Add(model);
            return _dbContext.SaveChanges();
        }

        public int Delete(T model)
        {
            _dbContext.Set<T>().Remove(model);
            return _dbContext.SaveChanges();
        }

        public T? Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public int Update(T model)
        {
            _dbContext.Set<T>().Update(model);
            return _dbContext.SaveChanges();
        }
    }
}
