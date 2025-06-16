using AutoMapper;
using CompanyMVC.DAL.Models;
using CompanyMVC.PL.Dtos;

namespace CompanyMVC.PL.Mapping
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee,CreateEmployeeDto>();
        }
    }
}
