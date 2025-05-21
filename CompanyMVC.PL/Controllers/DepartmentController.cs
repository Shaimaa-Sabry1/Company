using CompanyMVC.BLL.Interfaces;
using CompanyMVC.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CompanyMVC.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        //Ask CLR Create Object From  DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        [HttpGet] //Get :/Department/Index
        public IActionResult Index()
        {
           
            var departments= _departmentRepository.GetAll();

            return View(departments);
        }
    }
}
