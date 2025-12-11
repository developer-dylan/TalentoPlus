using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Data;
using TalentoPlus.Data;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations
{
    public class GeminiService : IGeminiService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GeminiService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<string> AskGeminiAsync(string question)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return "Error: Gemini API Key is missing. Please configure 'Gemini:ApiKey' in appsettings.json.";
            }

            // Read model from configuration (optional). If not set, use a stable default.
            var model = _configuration["Gemini:Model"] ?? "gemini-2.5-flash";
            // Remove "models/" prefix if present in the configuration (to avoid duplication in URL)
            if (model.StartsWith("models/"))
            {
                model = model.Substring("models/".Length);
            }

            // 1. Get Context Data
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Select(e => new
                {
                    e.FullName,
                    e.JobTitle,
                    Department = e.Department.Name,
                    Status = e.Status.ToString(),
                    e.Salary
                })
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Eres un asistente de IA para un sistema de Recursos Humanos llamado TalentoPlus.");
            sb.AppendLine("Aquí están los datos actuales de los empleados:");
            foreach (var emp in employees)
            {
                string statusEs = emp.Status switch
                {
                    "Active" => "Activo",
                    "Inactive" => "Inactivo",
                    "Vacation" => "Vacaciones",
                    _ => emp.Status
                };
                sb.AppendLine($"- {emp.FullName}, {emp.JobTitle}, Dept: {emp.Department}, Estado: {statusEs}, Salario: {emp.Salary}");
            }
            sb.AppendLine();
            sb.AppendLine("Responde la siguiente pregunta basándote en los datos anteriores. Responde siempre en español:");
            sb.AppendLine(question);

            // 2. Call Gemini API
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = sb.ToString() }
                        }
                    }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // If model not found, try to list available models to help debugging
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        try
                        {
                            var listUrl = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";
                            var listResp = await _httpClient.GetAsync(listUrl);
                            var listBody = await listResp.Content.ReadAsStringAsync();
                            return $"Error calling Gemini API: {response.StatusCode}. Body: {responseString}. Available models: {listBody}";
                        }
                        catch (Exception exList)
                        {
                            return $"Error calling Gemini API: {response.StatusCode}. Body: {responseString}. Additionally, listing models failed: {exList.Message}";
                        }
                    }

                    return $"Error calling Gemini API: {response.StatusCode}. Body: {responseString}";
                }

                try
                {
                    var responseJson = JsonDocument.Parse(responseString);

                    var text = responseJson.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    return text ?? "No response generated.";
                }
                catch (Exception exParse)
                {
                    // If parsing fails, return raw body for debugging
                    return $"Response parsing failed: {exParse.Message}. Raw body: {responseString}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
    }
}
