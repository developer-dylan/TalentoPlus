using OfficeOpenXml;
using TalentoPlus.Web.DTOs;
using TalentoPlus.Web.Services.Interfaces;
using TalentoPlus.Web.Repositories.Interfaces;

namespace TalentoPlus.Web.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        private readonly IExcelRepository _excelRepository;

        public ExcelService(IExcelRepository excelRepository)
        {
            _excelRepository = excelRepository;
        }

        public async Task<bool> ProcessExcelAsync(IFormFile file)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null) return false;

                var employees = new List<ExcelEmployeeDto>();
                int totalRows = worksheet.Dimension.End.Row;

                for (int row = 2; row <= totalRows; row++)
                {
                    if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text))
                        continue;

                    // COL 1 is the ID/Code (e.g. "9M", "2A") -> IGNORE
                    
                    employees.Add(new ExcelEmployeeDto
                    {
                        FirstName = CleanString(worksheet.Cells[row, 2].Text),    
                        LastName = CleanString(worksheet.Cells[row, 3].Text),     

                        BirthDate = ParseDate(worksheet.Cells[row, 4].Text),
                        Address = CleanString(worksheet.Cells[row, 5].Text),
                        Phone = CleanString(worksheet.Cells[row, 6].Text),
                        Email = CleanString(worksheet.Cells[row, 7].Text),
                        
                        JobTitle = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 8].Text) 
                                   ? CleanString(worksheet.Cells[row, 8].Text) 
                                   : "Sin cargo asignado",

                        Salary = decimal.TryParse(worksheet.Cells[row, 9].Text, out var salary)
                            ? salary : 0,

                        HireDate = ParseDate(worksheet.Cells[row, 10].Text),

                        Status = ParseStatus(worksheet.Cells[row, 11].Text),
                        Education = CleanString(worksheet.Cells[row, 12].Text),

                        ProfessionalProfile = CleanString(worksheet.Cells[row, 13].Text),
                        Department = CleanString(worksheet.Cells[row, 14].Text)
                    });
                }

                await _excelRepository.SaveEmployeesFromExcelAsync(employees);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string ParseStatus(string statusText)
        {
            if (string.IsNullOrWhiteSpace(statusText)) return "Active";
            
            statusText = statusText.Trim().ToLower();
            if (statusText.Contains("inactivo") || statusText.Contains("inactive")) return "Inactive";
            if (statusText.Contains("vacacion") || statusText.Contains("vacation")) return "Vacation";
            
            return "Active";
        }

        // Helper method: safe date parsing
        private DateTime ParseDate(string input)
        {
            return DateTime.TryParse(input, out var date)
                ? date
                : DateTime.Now; // fallback to avoid crashes
        }
        private string CleanString(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.Trim().ToLower());
        }
    }
}
