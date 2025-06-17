using AutoMapper;
using CompanyMVC.DAL.Models;
using CompanyMVC.PL.Dtos;

namespace CompanyMVC.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();
        }
    }
}
