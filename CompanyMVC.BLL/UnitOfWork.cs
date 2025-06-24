using CompanyMVC.BLL.Interfaces;
using CompanyMVC.BLL.Repositories;
using CompanyMVC.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyMVC.BLL
{
   public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _companyDb;

        public IDepartmentRepository DepartmentRepository { get; }

        public IEmployeeRepository EmployeeRepository { get; }

        public UnitOfWork(CompanyDbContext companyDb)
        {
           _companyDb = companyDb;
            DepartmentRepository = new DepartmentRepository(_companyDb);
            EmployeeRepository = new EmployeeRepository(_companyDb);

        }

        public async Task<int> CompleteAsync()
        {
            return await _companyDb.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _companyDb.DisposeAsync();
        }
    }
}
