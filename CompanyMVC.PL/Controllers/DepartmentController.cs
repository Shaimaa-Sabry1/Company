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

        [HttpGet]
        public IActionResult Details(int id)
        {
            var department = _departmentRepository.Get(id);
            if (department == null)
            {
                return NotFound(); // Prevents view from rendering null object
            }

            return View(department);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = _departmentRepository.Get(id);
          
            ;
            if (department == null)
            {
                return NotFound(); // Prevents view from rendering null object
            }

            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(Department model)
        {
            if(ModelState.IsValid)
            {
                var department = _departmentRepository.Get(model.Id);

                
                if (department == null)
                {
                    return NotFound(); // Prevents view from rendering null object
                }
                department.Code = model.Code;
                department.Name = model.Name;
                department.CreateAt = model.CreateAt;
                var count = _departmentRepository.Update(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                
            }
            return View(model);

        }
    }
}
