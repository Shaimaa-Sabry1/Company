using CompanyMVC.BLL.Interfaces;
using CompanyMVC.DAL.Models;
using CompanyMVC.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CompanyMVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var employee = _employeeRepository.GetAll();

            return View(employee);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if(ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    Phone = model.Phone,
                    Email = model.Email,
                    Salary = model.Salary,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted
                };
                var count = _employeeRepository.Add(employee);
                if(count>0)
                {
                    return RedirectToAction(nameof(Index));
                }
               
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Details(int?id,string viewName="Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id : {id} is Not Found" });
            return View(viewName, employee);
        }
        [HttpGet]
        public IActionResult Edit(int?id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,Employee employee)
        {
            if(ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest();
                var count = _employeeRepository.Update(employee);
                if(count>0)
                {
                    return RedirectToAction(nameof(Index));
                }
                
            }
            return View(employee);

        }
        [HttpGet]
        
        public IActionResult Delete(int?id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id,Employee employee)
        {
            if(ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest();
                var count = _employeeRepository.Delete(employee);
                if(count>0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(employee);
        }


    }
}
