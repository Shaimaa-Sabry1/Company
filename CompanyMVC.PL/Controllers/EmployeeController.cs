using CompanyMVC.BLL.Interfaces;
using CompanyMVC.DAL.Models;
using CompanyMVC.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CompanyMVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public EmployeeController(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
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
            var department = _departmentRepository.GetAll();
            //Dictionary: 2 Property
            //1.ViewData: Transfer Extra Information From Controller (Action) To View
            //2.ViewBag: Transfer Extra Information From Controller (Action) To View
            
            ViewData["departments"] = department;
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
                    IsDeleted = model.IsDeleted,
                    DepartmentId=model.DepartmentId
                };
                var count = _employeeRepository.Add(employee);
                if(count>0)
                {
                    TempData["Message"] = "Employee Is Created !!";
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
            var department = _departmentRepository.GetAll();

            ViewData["departments"] = department;
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new
            {
                StatusCode = 404,
                message = $"Employee With Id:{id} Is Not Found"
            });
            var employeeDto = new CreateEmployeeDto()
            {
                
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                Phone = employee.Phone,
                Email = employee.Email,
                Salary = employee.Salary,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                DepartmentId=employee.DepartmentId
            };
            return View(employeeDto);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,CreateEmployeeDto model)
        {
            if(ModelState.IsValid)
            {

                var employee = new Employee()
                {
                    Id=id,
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    Phone = model.Phone,
                    Email = model.Email,
                    Salary = model.Salary,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    DepartmentId=model.DepartmentId
                };
                var count = _employeeRepository.Update(employee);
                if(count>0)
                {
                    return RedirectToAction(nameof(Index));
                }
                
            }
            return View(model);

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
