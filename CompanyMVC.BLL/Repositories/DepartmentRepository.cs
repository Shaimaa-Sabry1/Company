﻿using CompanyMVC.BLL.Interfaces;
using CompanyMVC.DAL.Data.Contexts;
using CompanyMVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyMVC.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
    {
        public DepartmentRepository(CompanyDbContext dbContext):base(dbContext)
        {
            
        }

    }
}
