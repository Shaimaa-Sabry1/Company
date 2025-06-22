using CompanyMVC.BLL.Interfaces;
using CompanyMVC.DAL.Data.Contexts;
using CompanyMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
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
        public void Add(T model)
        {
            _dbContext.Set<T>().Add(model);
        }

        public void Delete(T model)
        {
            _dbContext.Set<T>().Remove(model);
        }

        public T? Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T)==typeof(Employee))
            {
                return (IEnumerable<T>) _dbContext.Employees.Include(E => E.Department).ToList();
            }
            return _dbContext.Set<T>().ToList();
        }

        public void Update(T model)
        {
            _dbContext.Set<T>().Update(model);
        }
    }
}
