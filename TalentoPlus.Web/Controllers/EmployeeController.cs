using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TalentoPlus.Data.Entities;
using TalentoPlus.Data.Entities.Enums;
using TalentoPlus.Web.Services.Interfaces;
using TalentoPlus.Web.Repositories.Interfaces;
using TalentoPlus.Web.ViewModels.Employees;

namespace TalentoPlus.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;
        private readonly IDepartmentRepository _deptRepo;
        private readonly IPdfService _pdfService;

        public EmployeeController(
            IEmployeeService service,
            IDepartmentRepository deptRepo,
            IPdfService pdfService)
        {
            _service = service;
            _deptRepo = deptRepo;
            _pdfService = pdfService;
        }

        // LIST
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var employees = await _service.GetAllAsync();
            int pageSize = 5;
            return View(TalentoPlus.Web.Models.PaginatedList<Employee>.Create(employees, pageNumber ?? 1, pageSize));
        }

        // CREATE (GET)
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new CreateEmployeeViewModel());
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(vm);
            }

            var employee = new Employee
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,
                Address = vm.Address,
                Phone = vm.Phone,
                Email = vm.Email,
                JobTitle = vm.JobTitle,
                Salary = vm.Salary,
                HireDate = vm.HireDate,
                Status = vm.Status,
                Education = vm.Education,
                ProfessionalProfile = vm.ProfessionalProfile,
                DepartmentId = vm.DepartmentId
            };

            await _service.CreateAsync(employee);
            TempData["Message"] = "Empleado creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _service.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            var vm = new EditEmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Address = employee.Address,
                Phone = employee.Phone,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                Status = employee.Status,
                Education = employee.Education,
                ProfessionalProfile = employee.ProfessionalProfile,
                DepartmentId = employee.DepartmentId
            };

            await LoadDropdowns();
            return View(vm);
        }

        // EDIT (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(vm);
            }

            var employee = await _service.GetByIdAsync(vm.Id);
            if (employee == null)
                return NotFound();

            employee.FirstName = vm.FirstName;
            employee.LastName = vm.LastName;
            employee.BirthDate = vm.BirthDate;
            employee.Address = vm.Address;
            employee.Phone = vm.Phone;
            employee.Email = vm.Email;
            employee.JobTitle = vm.JobTitle;
            employee.Salary = vm.Salary;
            employee.HireDate = vm.HireDate;
            employee.Status = vm.Status;
            employee.Education = vm.Education;
            employee.ProfessionalProfile = vm.ProfessionalProfile;
            employee.DepartmentId = vm.DepartmentId;

            await _service.UpdateAsync(employee);

            TempData["Message"] = "Empleado actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _service.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // CV VIEW
        public async Task<IActionResult> CV(int id)
        {
            var employee = await _service.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _service.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Message"] = "Empleado eliminado.";
            return RedirectToAction(nameof(Index));
        }

        // PDF EXPORT
        public async Task<IActionResult> ResumePdf(int id)
        {
            var employee = await _service.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            var pdf = _pdfService.GenerateEmployeePdf(employee);
            return File(pdf, "application/pdf", $"Employee_{id}.pdf");
        }

        // DROPDOWNS
        private async Task LoadDropdowns()
        {
            var departments = await _deptRepo.GetAllAsync();

            // IMPORTANT: Department.Name should be in English
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(EmploymentStatus)));
            ViewBag.Education = new SelectList(Enum.GetValues(typeof(EducationLevel)));
        }
    }
}
