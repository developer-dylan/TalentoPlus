using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Data;
using TalentoPlus.Data;
using TalentoPlus.Data.Entities.Enums;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IGeminiService _geminiService;

        public DashboardController(AppDbContext context, IGeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        // MAIN DASHBOARD
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalEmployees = await _context.Employees.CountAsync();
            ViewBag.OnVacation = await _context.Employees.CountAsync(e => e.Status == EmploymentStatus.Vacation);
            ViewBag.ActiveEmployees = await _context.Employees.CountAsync(e => e.Status == EmploymentStatus.Active);

            return View();
        }

        // ================================
        // IA endpoint: interpreta y responde
        // ================================
        [HttpPost]
        public async Task<IActionResult> AskAI([FromBody] QuestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Question))
                return Json(new { answer = "Por favor escribe una pregunta." });

            var answer = await _geminiService.AskGeminiAsync(request.Question);
            return Json(new { answer });
        }

        public class QuestionRequest
        {
            public string Question { get; set; }
        }
    }
}
