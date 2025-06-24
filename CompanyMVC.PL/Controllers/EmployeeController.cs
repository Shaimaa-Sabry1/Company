using AutoMapper;
using CompanyMVC.BLL.Interfaces;
using CompanyMVC.DAL.Models;
using CompanyMVC.PL.Dtos;
using CompanyMVC.PL.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompanyMVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

       
        private readonly IMapper _mapper;

        public EmployeeController(
        IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
           
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employee;

            if (string.IsNullOrEmpty(SearchInput))
            {
                employee =await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                employee =await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

            return View(employee);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var department =await _unitOfWork.DepartmentRepository.GetAllAsync();
            //Dictionary: 2 Property
            //1.ViewData: Transfer Extra Information From Controller (Action) To View
            //2.ViewBag: Transfer Extra Information From Controller (Action) To View

            ViewData["departments"] = department;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {

                if(model.Image is not null)
                {
                  model.ImageName=  DocumentSetting.UploadFile(model.Image, "Images");

                }
                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count =await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Is Created !!";
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id : {id} is Not Found" });
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string viewName = "Edit")
        {
            var department =await _unitOfWork.DepartmentRepository.GetAllAsync();


            ViewData["departments"] = department;
            if (id is null) return BadRequest("Invalid Id");
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

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
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {

                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSetting.Delete(model.ImageName, "Images");
                }
                if(model.Image is not null)
                {
                    model.ImageName=   DocumentSetting.UploadFile(model.Image, "Images");
                }
                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Update(employee);
                var count =await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(viewName, model);

        }
        [HttpGet]

        public async Task<IActionResult> Delete(int? id)
        {
            return await Edit(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id); // Fetch from DB
            if (employee == null)
            {
                return NotFound();
            }

             _unitOfWork.EmployeeRepository.Delete(employee); // Now tracked and valid
            var count =await _unitOfWork.CompleteAsync();
            if (count > 0)
            {
                if(employee.ImageName is not null)
                {
                    DocumentSetting.Delete(employee.ImageName, "Images");

                }
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }
    }
    }
