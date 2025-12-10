using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TalentoPlus.Api.Services.Interfaces;
using TalentoPlus.Data.Entities;
using TalentoPlus.Data.Entities.Enums;

namespace TalentoPlus.Api.Services.Implementations;

public class CvService : ICvService
{
    public CvService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateCvPdf(Employee emp)
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
                    .Text($"Hoja de Vida – {emp.FullName}")
                    .SemiBold().FontSize(22).FontColor(Colors.Blue.Medium);

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    col.Item().Text("Información Personal")
                        .SemiBold().FontSize(16);

                    col.Item().Text($"Nombre Completo: {emp.FullName}");
                    col.Item().Text($"Fecha de Nacimiento: {emp.BirthDate:dd/MM/yyyy}");
                    col.Item().Text($"Dirección: {emp.Address}");

                    col.Item().Text("Información de Contacto")
                        .SemiBold().FontSize(16);

                    col.Item().Text($"Teléfono: {emp.Phone}");
                    col.Item().Text($"Email: {emp.Email}");

                    col.Item().Text("Información Laboral")
                        .SemiBold().FontSize(16);

                    col.Item().Text($"Cargo: {emp.JobTitle}");
                    col.Item().Text($"Salario: ${emp.Salary:N2}");
                    col.Item().Text($"Fecha de Ingreso: {emp.HireDate:dd/MM/yyyy}");
                    
                    string statusEs = emp.Status switch {
                        EmploymentStatus.Active => "Activo",
                        EmploymentStatus.Inactive => "Inactivo",
                        EmploymentStatus.Vacation => "Vacaciones",
                        _ => emp.Status.ToString()
                    };
                    col.Item().Text($"Estado: {statusEs}");

                    col.Item().Text("Nivel Educativo")
                        .SemiBold().FontSize(16);

                    col.Item().Text(emp.Education.ToString());

                    col.Item().Text("Departamento Asignado")
                        .SemiBold().FontSize(16);

                    col.Item().Text(emp.Department?.Name ?? "N/A");

                    col.Item().Text("Perfil Profesional")
                        .SemiBold().FontSize(16);

                    col.Item().Text(emp.ProfessionalProfile)
                        .FontSize(12);
                });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                    });
            });
        }).GeneratePdf();
    }
}
