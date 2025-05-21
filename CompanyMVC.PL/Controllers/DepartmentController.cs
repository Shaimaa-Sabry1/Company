using CompanyMVC.BLL.Interfaces;
using CompanyMVC.BLL.Repositories;
using CompanyMVC.DAL.Models;
using CompanyMVC.PL.Dtos;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if(ModelState.IsValid)//Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name=model.Name,
                    CreateAt=model.CreateAt
                };

               var count= _departmentRepository.Add(department);
                if(count>0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }
    }
}
