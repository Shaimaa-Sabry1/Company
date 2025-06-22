using AutoMapper;
using CompanyMVC.BLL.Interfaces;
using CompanyMVC.BLL.Repositories;
using CompanyMVC.DAL.Models;
using CompanyMVC.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CompanyMVC.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        
        private readonly IMapper _mapper;
        //Ask CLR Create Object From  DepartmentRepository
        public DepartmentController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet] //Get :/Department/Index
        public IActionResult Index()
        {
           
            var departments= _unitOfWork.DepartmentRepository.GetAll();

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
                var department = _mapper.Map<Department>(model);

                 _unitOfWork.DepartmentRepository.Add(department);
                var count = _unitOfWork.Complete();
                if(count>0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest("Invalid ID");
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department == null)
            {
                return NotFound(); // Prevents view from rendering null object
            }
            var dto = _mapper.Map<CreateDepartmentDto>(department);


            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int? id, string viewName = "Edit")
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null) return NotFound(new
            {
                StatusCode = 404,
                message = $"Department With Id:{id} Is Not Found"
            });
            var dto = _mapper.Map<CreateDepartmentDto>(department);

            return View(viewName,dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,CreateDepartmentDto model,string viewName="Edit")
        {
            if(ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);


                _unitOfWork.DepartmentRepository.Update(department);
                var count = _unitOfWork.Complete();

                if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
                
               

                
            }
            return View(viewName,model);

        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            

            return Edit(id,"Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
           

                var department = _unitOfWork.DepartmentRepository.Get(id); // Fetch from DB
                if (department == null)
                {
                    return NotFound();
                }
                _unitOfWork.DepartmentRepository.Delete(department);
            
                var count = _unitOfWork.Complete();

            if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }





            
            return View(department);

        }

    }
}
