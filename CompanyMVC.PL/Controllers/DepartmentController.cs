using AutoMapper;
using CompanyMVC.BLL.Interfaces;
using CompanyMVC.BLL.Repositories;
using CompanyMVC.DAL.Models;
using CompanyMVC.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
           
            var departments=await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if(ModelState.IsValid)//Server Side Validation
            {
                var department = _mapper.Map<Department>(model);

                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count =await _unitOfWork.CompleteAsync();
                if(count>0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest("Invalid ID");
            var department =await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department == null)
            {
                return NotFound(); // Prevents view from rendering null object
            }
            var dto = _mapper.Map<CreateDepartmentDto>(department);


            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string viewName = "Edit")
        {
            if (id is null) return BadRequest("Invalid Id");
            var department =await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
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
        public async Task<IActionResult> Edit([FromRoute]int id,CreateDepartmentDto model,string viewName="Edit")
        {
            if(ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);


                _unitOfWork.DepartmentRepository.Update(department);
                var count =await _unitOfWork.CompleteAsync();

                if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
                
               

                
            }
            return View(viewName,model);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            

            return await Edit(id,"Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
           

                var department =await _unitOfWork.DepartmentRepository.GetAsync(id); // Fetch from DB
                if (department == null)
                {
                    return NotFound();
                }
                _unitOfWork.DepartmentRepository.Delete(department);
            
                var count =await _unitOfWork.CompleteAsync();

            if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }





            
            return View(department);

        }

    }
}
