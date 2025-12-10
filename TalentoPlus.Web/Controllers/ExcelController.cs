using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Controllers
{
    // Controller for Excel file import operations
    [Authorize]
    public class ExcelController : Controller
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        // Displays the upload view
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // Processes the uploaded Excel file
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Por favor selecciona un archivo Excel válido.";
                return RedirectToAction("Upload");
            }

            var result = await _excelService.ProcessExcelAsync(file);

            if (result)
            {
                TempData["Message"] = "Empleados importados exitosamente.";
            }
            else
            {
                TempData["Error"] = "Error al procesar el archivo. Verifica el formato.";
            }

            return RedirectToAction("Upload");
        }
    }
}