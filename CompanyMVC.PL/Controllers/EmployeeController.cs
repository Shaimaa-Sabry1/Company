using AutoMapper;
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
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
             IEnumerable<Employee>employee;

            if(string.IsNullOrEmpty(SearchInput))
            {
                 employee = _employeeRepository.GetAll();

            }
            else
            {
                 employee = _employeeRepository.GetByName(SearchInput);
            }

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
            if (ModelState.IsValid)
                {
                //    var employee = new Employee()
                //    {
                //        Name = model.Name,
                //        Address = model.Address,
                //        Age = model.Age,
                //        Phone = model.Phone,
                //        Email = model.Email,
                //        Salary = model.Salary,
                //        CreateAt = model.CreateAt,
                //        HiringDate = model.HiringDate,
                //        IsActive = model.IsActive,
                //        IsDeleted = model.IsDeleted,
                //        DepartmentId=model.DepartmentId
                //    };
               var employee= _mapper.Map<Employee>(model);
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
           
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);

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
