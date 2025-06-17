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
            IEnumerable<Employee> employee;

            if (string.IsNullOrEmpty(SearchInput))
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

                var employee = _mapper.Map<Employee>(model);
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee Is Created !!";
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id : {id} is Not Found" });
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);
        }
        [HttpGet]
        public IActionResult Edit(int? id, string viewName = "Edit")
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
            return View(viewName, dto);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {

                var employee = _mapper.Map<Employee>(model);
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(viewName, model);

        }
        [HttpGet]

        public IActionResult Delete(int? id)
        {
            return Edit(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            var employee = _employeeRepository.Get(id); // Fetch from DB
            if (employee == null)
            {
                return NotFound();
            }

            var count = _employeeRepository.Delete(employee); // Now tracked and valid
            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }
    }
    }
