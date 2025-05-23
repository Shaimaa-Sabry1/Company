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
        public IActionResult Details(int? id,string viewName="Details")
        {
            if (id is null) return BadRequest("Invalid ID");
            var department = _departmentRepository.Get(id.Value);
            if (department == null)
            {
                return NotFound(); // Prevents view from rendering null object
            }

            return View(viewName,department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //var department = _departmentRepository.Get(id.Value);
          
            //;
            //if (department == null)
            //{
            //    return NotFound(); // Prevents view from rendering null object
            //}

            return Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,Department model)
        {
            if(ModelState.IsValid)
            {
                var department = new Department()
                {
                    Id = id,
                    Name=model.Name,
                    Code=model.Code,
                    CreateAt=model.CreateAt
                };
               
                    var count = _departmentRepository.Update(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
                
               

                
            }
            return View(model);

        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            

            return Details(id,"Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department model)
        {
            if (ModelState.IsValid)
            {
               

                var count = _departmentRepository.Delete(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }





            }
            return View(model);

        }

    }
}
