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
        public async Task AddAsync(T model)
        {
           await _dbContext.Set<T>().AddAsync(model);
        }

        public void Delete(T model)
        {
            _dbContext.Set<T>().Remove(model);
        }

        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _dbContext.Employees.Include(E => E.Department).FirstOrDefaultAsync(E => E.Id == id) as T;
            }
            return _dbContext.Set<T>().Find(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           
            return await _dbContext.Set<T>().ToListAsync();
        }

        public void Update(T model)
        {
            _dbContext.Set<T>().Update(model);
        }
    }
}
