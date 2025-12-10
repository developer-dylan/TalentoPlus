using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TalentoPlus.Data.Entities;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations
{
    public class PdfService : IPdfService
    {
        public PdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateEmployeePdf(Employee emp)
        {
            return Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Resume – {emp.FirstName} {emp.LastName}")
                        .SemiBold().FontSize(22).FontColor(Colors.Blue.Medium);

                    page.Content().Column(col =>
                    {
                        col.Spacing(15);

                        col.Item().Text("Personal Information")
                            .SemiBold().FontSize(16);

                        col.Item().Text($"Full Name: {emp.FullName}");
                        col.Item().Text($"Birth Date: {emp.BirthDate:dd/MM/yyyy}");
                        col.Item().Text($"Address: {emp.Address}");

                        col.Item().Text("Contact Information")
                            .SemiBold().FontSize(16);

                        col.Item().Text($"Phone: {emp.Phone}");
                        col.Item().Text($"Email: {emp.Email}");

                        col.Item().Text("Work Information")
                            .SemiBold().FontSize(16);

                        col.Item().Text($"Job Title: {emp.JobTitle}");
                        col.Item().Text($"Salary: ${emp.Salary:N2}");
                        col.Item().Text($"Hire Date: {emp.HireDate:dd/MM/yyyy}");
                        col.Item().Text($"Status: {emp.Status}");

                        col.Item().Text("Education Level")
                            .SemiBold().FontSize(16);

                        col.Item().Text(emp.Education.ToString());

                        col.Item().Text("Assigned Department")
                            .SemiBold().FontSize(16);

                        col.Item().Text(emp.Department?.Name ?? "N/A");

                        col.Item().Text("Professional Profile")
                            .SemiBold().FontSize(16);

                        col.Item().Text(emp.ProfessionalProfile)
                            .FontSize(12);
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                        });
                });
            }).GeneratePdf();
        }
    }
}
