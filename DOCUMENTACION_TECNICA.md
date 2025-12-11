# 📋 DOCUMENTACIÓN TÉCNICA COMPLETA - PROYECTO TALENTOPLUS

## Versión 1.2 | 11 de diciembre de 2025

---

## 🎯 TABLA DE CONTENIDOS

1. [Descripción General del Sistema](#1-descripción-general-del-sistema)
2. [Tecnologías y Herramientas](#2-tecnologías-y-herramientas)
3. [Arquitectura del Proyecto](#3-arquitectura-del-proyecto)
4. [Proyecto API](#4-proyecto-api)
5. [Integración de Inteligencia Artificial](#5-integración-de-inteligencia-artificial)
6. [Generación de CV en PDF](#6-generación-de-cv-en-pdf)
7. [Envío de Correos Electrónicos](#7-envío-de-correos-electrónicos)
8. [Carga de Datos desde Excel](#8-carga-de-datos-desde-excel)
9. [Proyecto Data](#9-proyecto-data)
10. [Proyecto Web](#10-proyecto-web)
11. [Pruebas Unitarias e Integración](#11-pruebas-unitarias-e-integración)
12. [Conclusiones Técnicas](#12-conclusiones-técnicas)
13. [Diagramas y Visualizaciones](#13-diagramas-y-visualizaciones)
14. [Explicación Detallada del Desarrollo](#14-explicación-detallada-del-desarrollo)

---

## 1. DESCRIPCIÓN GENERAL DEL SISTEMA

### 1.1 Propósito y Objetivos

**TalentoPlus** es un sistema integral de gestión de Recursos Humanos desarrollado con arquitectura de microservicios desacoplada. El sistema está diseñado para:

- **Gestión centralizada de empleados** con almacenamiento de información personal, laboral y académica
- **Generación de documentos profesionales** (CVs en PDF con formato estándar)
- **Análisis inteligente de datos** mediante integración con Google Gemini IA
- **Automatización de procesos** incluyendo importación masiva de datos desde Excel
- **Acceso seguro** mediante autenticación JWT y gestión de roles
- **APIs RESTful** para integración con sistemas externos

### 1.2 Módulos Principales

El proyecto está dividido en cuatro módulos independientes pero relacionados:

```
TalentoPlus/
├── TalentoPlus.Api/          → API REST (ASP.NET Core)
├── TalentoPlus.Web/          → Aplicación Web (ASP.NET Core MVC)
├── TalentoPlus.Data/         → Capa de Datos (Entity Framework Core)
└── TalentoPlus.Tests/        → Pruebas (xUnit + Moq)
```

### 1.3 Flujo General de Funcionamiento

```
Usuario → Web/API
    ↓
[Autenticación JWT]
    ↓
Controladores (ASP.NET Core)
    ↓
Servicios (Lógica de Negocio)
    ↓
Repositorios (Acceso a Datos)
    ↓
Entity Framework Core
    ↓
PostgreSQL (Base de Datos)
```

---

## 2. TECNOLOGÍAS Y HERRAMIENTAS

### 2.1 Lenguajes de Programación

| Tecnología | Uso |
|------------|-----|
| **C# 12** | Lógica backend, APIs, servicios |
| **JavaScript/HTML/CSS** | Frontend (Razor Views con Bootstrap 5) |
| **SQL** | Consultas de base de datos (via EF Core LINQ) |

### 2.2 Frameworks Principales

| Framework | Versión | Propósito |
|-----------|---------|----------|
| **.NET SDK** | 8.0 | Framework principal para aplicaciones |
| **ASP.NET Core** | 8.0 | Framework web para API y MVC |
| **Entity Framework Core** | 8.0 | ORM para acceso a datos |
| **ASP.NET Identity** | 8.0 | Autenticación y gestión de usuarios |
| **xUnit** | 2.4.2 | Framework de pruebas unitarias |
| **Moq** | 4.20.72 | Mocking para pruebas |
| **QuestPDF** | Última | Generación de PDFs en C# |
| **EPPlus** | Última | Lectura de archivos Excel |

### 2.3 Librerías Externas Clave

```csharp
// Autenticación y Seguridad
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

// Acceso a Datos
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

// IA y APIs Externas
using System.Net.Http;
using System.Text.Json;

// Generación de Documentos
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

// Lectura de Excel
using OfficeOpenXml; // EPPlus

// Pruebas
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
```

### 2.4 Base de Datos

- **Motor**: PostgreSQL 16
- **Proveedor**: Npgsql (PostgreSQL provider para EF Core)
- **Conexión**: Connection strings en `appsettings.json` / variables de entorno
- **Migraciones**: Gestionadas con Entity Framework Core Migrations

### 2.5 Herramientas de Desarrollo

| Herramienta | Uso |
|------------|-----|
| **Visual Studio Code / Visual Studio** | IDE principal |
| **Swagger/OpenAPI** | Documentación interactiva de API |
| **Postman** | Testing de endpoints REST |
| **Git + GitHub** | Control de versiones |
| **Docker** | Containerización (opcional) |
| **DotNet CLI** | Herramientas de línea de comandos (.NET) |

---

## 3. ARQUITECTURA DEL PROYECTO

### 3.1 Patrón Arquitectónico: Arquitectura por Capas + Clean Architecture

El proyecto implementa una **arquitectura por capas verticales** con principios de **Clean Architecture**:

```
┌─────────────────────────────────────────────┐
│         CAPA DE PRESENTACIÓN                │
│  (Web Controllers, Views, API Endpoints)    │
├─────────────────────────────────────────────┤
│         CAPA DE APLICACIÓN                  │
│  (Services, DTOs, Middleware, Mappers)     │
├─────────────────────────────────────────────┤
│         CAPA DE DOMINIO                     │
│  (Entidades, Interfaces, Reglas Negocio)   │
├─────────────────────────────────────────────┤
│         CAPA DE INFRAESTRUCTURA              │
│  (DbContext, Repositorios, EF Core)        │
├─────────────────────────────────────────────┤
│         CAPA DE DATOS                       │
│  (PostgreSQL)                              │
└─────────────────────────────────────────────┘
```

### 3.2 Descripción de Capas

#### **Capa de Presentación**
- **Ubicación**: `TalentoPlus.Web/Controllers` y `TalentoPlus.Api/Controllers`
- **Responsabilidad**: Recibir solicitudes HTTP, validar entrada, retornar respuestas
- **Componentes**:
  - `DashboardController`: Gestión del dashboard y asistente IA
  - `EmployeeController`: Operaciones CRUD de empleados
  - `ExcelController`: Importación masiva de datos
  - `AuthController`: Autenticación y generación de JWT
  - `AccountController`: Gestión de cuentas de usuario

#### **Capa de Aplicación (Services)**
- **Ubicación**: `TalentoPlus.Web/Services` y `TalentoPlus.Api/Services`
- **Responsabilidad**: Lógica de negocio, orquestación, transformación de datos
- **Componentes**:
  - `GeminiService`: Integración con IA
  - `CvService`: Generación de PDFs
  - `ExcelService`: Procesamiento de archivos Excel
  - `EmployeeService`: Lógica de empleados
  - `DashboardService`: Estadísticas y reportes

#### **Capa de Dominio (Entities)**
- **Ubicación**: `TalentoPlus.Data/Entities`
- **Responsabilidad**: Definir entidades de negocio y reglas del dominio
- **Entidades Principales**:
  ```csharp
  public class Employee
  {
      public int Id { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Email { get; set; }
      public string JobTitle { get; set; }
      public Department Department { get; set; }
      public EmploymentStatus Status { get; set; }
      public EducationLevel Education { get; set; }
      // ... más propiedades
  }

  public class Department
  {
      public int Id { get; set; }
      public string Name { get; set; }
      public ICollection<Employee> Employees { get; set; }
  }
  ```

#### **Capa de Infraestructura (Data Access)**
- **Ubicación**: `TalentoPlus.Data/Repositories`
- **Responsabilidad**: Acceso a datos, persistencia, migrations
- **Componentes**:
  - `AppDbContext`: Contexto de Entity Framework
  - `EmployeeRepository`: Consultas de empleados
  - `DepartmentRepository`: Consultas de departamentos
  - Entity Framework Migrations

#### **Capa de Datos**
- **Base de Datos**: PostgreSQL
- **Tablas**: Employees, Departments, AspNetUsers, AspNetRoles, etc.

### 3.3 Principios SOLID Aplicados

| Principio | Aplicación en el Proyecto |
|-----------|--------------------------|
| **S**ingle Responsibility | Cada servicio tiene una única responsabilidad (GeminiService solo IA, CvService solo PDFs) |
| **O**pen/Closed | Las interfaces (IGeminiService, ICvService) permiten extensión sin modificación |
| **L**iskov Substitution | Los repositorios implementan interfaces estándar intercambiables |
| **I**nterface Segregation | Interfaces pequeñas y específicas (IGeminiService, ICvService, etc.) |
| **D**ependency Inversion | Inyección de dependencias con contenedor DI de .NET |

### 3.4 Ventajas de la Arquitectura Elegida

✅ **Separación de Responsabilidades**: Cada capa tiene una función clara
✅ **Testabilidad**: Fácil crear tests unitarios e integración
✅ **Mantenibilidad**: Cambios localizados sin afectar otras capas
✅ **Escalabilidad**: Posibilidad de escalar componentes independientemente
✅ **Reutilización de Código**: Lógica compartida en servicios y repositorios

---

## 4. PROYECTO API

### 4.1 Estructura y Construcción

#### **Controladores**
```csharp
// TalentoPlus.Api/Controllers/AuthController.cs
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // Validación de credenciales
        // Generación de JWT
        // Retorno de token
    }
}
```

#### **DTOs (Data Transfer Objects)**
```csharp
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // ... mapeo desde Employee entity
}
```

### 4.2 Autenticación con JWT

#### **4.2.1 Configuración en Program.cs**

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
```

#### **4.2.2 Generación de Token**

```csharp
public class AuthService
{
    public string GenerateToken(User user, string secret, string issuer, string audience)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("role", user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
```

#### **4.2.3 Validación y Middleware**

- **Middleware**: `JwtBearerHandler` (integrado en ASP.NET Core)
- **Validación de Token**: Verificación de firma, expiración, issuer/audience
- **Authorization**: Usando atributo `[Authorize]` en controladores

### 4.3 Endpoints Protegidos vs Públicos

#### **Endpoints Públicos (Sin Autenticación)**
```csharp
[HttpPost("/api/auth/login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    // Retorna JWT token
}

[HttpPost("/api/empleados/autoregistro")]
public async Task<IActionResult> Register([FromBody] RegisterDto dto)
{
    // Registro de nuevo empleado
}

[HttpGet("/api/departamentos")]
public async Task<IActionResult> GetDepartments()
{
    // Listado público de departamentos
}
```

#### **Endpoints Protegidos (Requieren JWT)**
```csharp
[Authorize]
[HttpGet("/api/empleados")]
public async Task<IActionResult> GetAllEmployees()
{
    // Solo usuarios autenticados
}

[Authorize(Roles = "Admin")]
[HttpDelete("/api/empleados/{id}")]
public async Task<IActionResult> DeleteEmployee(int id)
{
    // Solo administradores
}
```

### 4.4 Manejo de Errores Global

```csharp
// Middleware global de excepciones
public class ExceptionHandlingMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
    }
}
```

---

## 5. INTEGRACIÓN DE INTELIGENCIA ARTIFICIAL

### 5.1 Modelo Utilizado

- **Modelo**: Google Gemini 2.5 Flash (generaciones anteriores: Gemini 1.5 Flash)
- **API**: Google Generative Language API (REST)
- **Versión API**: `v1beta`
- **Endpoint**: `https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent`

### 5.2 Arquitectura de Integración

#### **Servicio GeminiService**

```csharp
public class GeminiService : IGeminiService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public async Task<string> AskGeminiAsync(string question)
    {
        // 1. Obtener API Key desde configuración segura (variables de entorno)
        var apiKey = _configuration["Gemini:ApiKey"];
        var model = _configuration["Gemini:Model"] ?? "gemini-2.5-flash";

        // 2. Contexto: Cargar datos de empleados de BD
        var employees = await _context.Employees
            .Include(e => e.Department)
            .ToListAsync();

        // 3. Construir prompt con datos del contexto
        var prompt = ConstructPrompt(question, employees);

        // 4. Realizar petición HTTP a Gemini API
        var response = await _httpClient.PostAsync(url, content);

        // 5. Parsear y retornar respuesta
        return ExtractTextFromResponse(response);
    }
}
```

### 5.3 Flujo Detallado de Comunicación

```
Usuario escriba pregunta
    ↓
DashboardController.AskAI()
    ↓
GeminiService.AskGeminiAsync(question)
    ↓
[Cargar empleados desde BD]
    ↓
[Construir prompt con contexto + pregunta]
    ↓
[HTTP POST a Google Gemini API]
    {
        "contents": [{
            "parts": [{
                "text": "CONTEXTO\n{datos empleados}\n\nPREGUNTA\n{pregunta usuario}"
            }]
        }]
    }
    ↓
[Google Gemini procesa y retorna respuesta]
    ↓
[Parsear JSON de respuesta]
    ↓
[Retornar texto al usuario]
```

### 5.4 Manejo Seguro de API Keys

```csharp
// ❌ INCORRECTO - No hacer esto
var apiKey = "AIzaSyCR_F-V3Whhor5Rs8g4aE55dV02TIZqbig"; // NUNCA hardcodear

// ✅ CORRECTO - Usar variables de entorno
public GeminiService(IConfiguration config)
{
    var apiKey = config["Gemini:ApiKey"]; // Desde variables de entorno
    var model = config["Gemini:Model"];
}
```

**Configuración en `appsettings.json`**:
```json
{
  "Gemini": {
    "ApiKey": "${GEMINI_API_KEY}",
    "Model": "gemini-2.5-flash"
  }
}
```

### 5.5 Casos de Uso de la IA

| Caso de Uso | Pregunta Ejemplo | Respuesta |
|------------|------------------|-----------|
| Consultas de Estadísticas | "¿Cuántos empleados hay activos?" | Cuenta y análisis |
| Búsquedas Inteligentes | "¿Quién es el desenvolvedor mejor pagado?" | Información específica |
| Análisis de Datos | "¿Cuál es el salario promedio?" | Análisis y reportes |
| Consultas de Departamentos | "¿Cuántos empleados tiene IT?" | Datos de departamento |

---

## 6. GENERACIÓN DE CV EN PDF

### 6.1 Librería Utilizada: QuestPDF

- **Librería**: QuestPDF v2024.x
- **Licencia**: Community (gratuita)
- **Ventajas**: 
  - Síntaxis fluida y declarativa
  - Excelente manejo de layouts
  - Tipografía profesional

### 6.2 Arquitectura del Generador de PDFs

#### **CvService - Implementación**

```csharp
public class CvService : ICvService
{
    public byte[] GenerateCvPdf(Employee emp)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                // Configuración de página
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                // Encabezado
                page.Header()
                    .Text($"Hoja de Vida – {emp.FullName}")
                    .SemiBold().FontSize(22).FontColor(Colors.Blue.Medium);

                // Contenido principal
                page.Content().Column(col =>
                {
                    // Sección: Información Personal
                    col.Item().Text("Información Personal")
                        .SemiBold().FontSize(16);
                    col.Item().Text($"Nombre: {emp.FullName}");
                    col.Item().Text($"Nacimiento: {emp.BirthDate:dd/MM/yyyy}");

                    // Sección: Contacto
                    col.Item().Text("Contacto")
                        .SemiBold().FontSize(16);
                    col.Item().Text($"Email: {emp.Email}");
                    col.Item().Text($"Teléfono: {emp.Phone}");

                    // Sección: Experiencia Laboral
                    col.Item().Text("Experiencia Laboral")
                        .SemiBold().FontSize(16);
                    col.Item().Text($"Puesto: {emp.JobTitle}");
                    col.Item().Text($"Departamento: {emp.Department?.Name}");
                    col.Item().Text($"Ingreso: {emp.HireDate:dd/MM/yyyy}");
                    col.Item().Text($"Salario: ${emp.Salary:N2}");

                    // Sección: Educación
                    col.Item().Text("Educación")
                        .SemiBold().FontSize(16);
                    col.Item().Text($"Nivel: {emp.Education}");

                    // Sección: Perfil Profesional
                    col.Item().Text("Perfil Profesional")
                        .SemiBold().FontSize(16);
                    col.Item().Text(emp.ProfessionalProfile);
                });

                // Pie de página
                page.Footer()
                    .AlignCenter()
                    .Text($"Página {Page.GetCurrentPageNumber()}");
            });
        }).GeneratePdf();
    }
}
```

### 6.3 Flujo de Generación y Descarga

```
Usuario en Web
    ↓
[Hacer clic en botón "Descargar CV"]
    ↓
GET /api/empleados/{id}/cv
    ↓
[EmployeeController obtiene datos del empleado]
    ↓
[Inyecta dependencia ICvService]
    ↓
CvService.GenerateCvPdf(employee)
    ↓
[QuestPDF construye documento]
    ↓
[Retorna array de bytes (PDF)]
    ↓
[Controller retorna FileResult]
    ↓
[Navegador descarga archivo o abre en nueva pestaña]
```

### 6.4 Endpoint de Descarga de CV

```csharp
[HttpGet("{id}/cv")]
public async Task<IActionResult> DownloadCv(int id)
{
    var employee = await _context.Employees
        .Include(e => e.Department)
        .FirstOrDefaultAsync(e => e.Id == id);

    if (employee == null)
        return NotFound();

    var pdfBytes = _cvService.GenerateCvPdf(employee);

    return File(
        pdfBytes,
        "application/pdf",
        $"CV_{employee.FirstName}_{employee.LastName}.pdf"
    );
}
```

### 6.5 Características de Diseño

| Aspecto | Implementación |
|--------|----------------|
| **Tamaño de Página** | A4 (210 × 297 mm) |
| **Márgenes** | 2 cm en todos los lados |
| **Tipografía** | Sistema de fuentes standard |
| **Colores** | Encabezado azul, texto negro |
| **Secciones** | Información personal, laboral, educación, perfil |
| **Encabezado/Pie** | Nombre del empleado / Número de página |

---

## 7. ENVÍO DE CORREOS ELECTRÓNICOS

### 7.1 Tecnología: SMTP + MailKit

- **Librería**: MailKit (NuGet)
- **Protocolo**: SMTP (Simple Mail Transfer Protocol)
- **Servidor**: Configurable (Gmail, Microsoft 365, etc.)
- **Autenticación**: Usuario/contraseña o tokens

### 7.2 Configuración en appsettings.json

```json
{
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "User": "${SMTP_USER}",
    "Password": "${SMTP_PASSWORD}",
    "FromAddress": "noreply@talentoplus.com",
    "FromName": "TalentoPlus RH"
  }
}
```

### 7.3 Servicio de Correos

```csharp
public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public async Task SendAsync(string to, string subject, string body)
    {
        var smtpHost = _config["Smtp:Host"];
        var smtpPort = int.Parse(_config["Smtp:Port"]);
        var smtpUser = _config["Smtp:User"];
        var smtpPassword = _config["Smtp:Password"];

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUser, smtpPassword);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TalentoPlus", smtpUser));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
```

### 7.4 Casos de Uso

| Evento | Correo Enviado |
|--------|---------------|
| **Registro de usuario** | Bienvenida + credenciales |
| **Cambio de contraseña** | Confirmación de cambio |
| **Reseteo de contraseña** | Link de reseteo |
| **Importación de empleados** | Resumen de importación |
| **Cambio de datos críticos** | Notificación a admin |

---

## 8. CARGA DE DATOS DESDE EXCEL

### 8.1 Librería: EPPlus

- **Librería**: EPPlus (NuGet)
- **Funcionalidad**: Lectura/escritura de archivos XLSX/XLSB
- **Formato Soportado**: Open XML (XLSX)

### 8.2 Flujo de Importación

```
Usuario sube archivo Excel
    ↓
ExcelController.ImportEmployees(IFormFile file)
    ↓
ExcelService.ProcessAsync(file)
    ↓
[Abrir archivo XLSX]
    ↓
[Iterar sobre filas]
    ↓
Para cada fila:
  ├─ Validar columnas requeridas
  ├─ Parsear datos
  ├─ Validar reglas de negocio
  ├─ Crear objeto Employee
  └─ Agregar a DbContext
    ↓
[SaveChanges en BD]
    ↓
[Retornar resumen: insertados, errores]
```

### 8.3 Implementación

```csharp
public class ExcelService : IExcelService
{
    private readonly AppDbContext _context;

    public async Task<ImportResult> ImportEmployeesAsync(IFormFile file)
    {
        var result = new ImportResult();
        var employees = new List<Employee>();

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Inicio en fila 2 (headers)
                {
                    try
                    {
                        var employee = new Employee
                        {
                            FirstName = worksheet.Cells[row, 1].Value?.ToString(),
                            LastName = worksheet.Cells[row, 2].Value?.ToString(),
                            Email = worksheet.Cells[row, 3].Value?.ToString(),
                            JobTitle = worksheet.Cells[row, 4].Value?.ToString(),
                            Phone = worksheet.Cells[row, 5].Value?.ToString(),
                            Address = worksheet.Cells[row, 6].Value?.ToString(),
                            // ... más campos
                        };

                        // Validaciones
                        if (ValidateEmployee(employee))
                        {
                            employees.Add(employee);
                            result.SuccessCount++;
                        }
                        else
                        {
                            result.ErrorCount++;
                            result.Errors.Add($"Fila {row}: Datos inválidos");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Fila {row}: {ex.Message}");
                    }
                }
            }
        }

        // Insertar en BD
        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();

        return result;
    }

    private bool ValidateEmployee(Employee emp)
    {
        return !string.IsNullOrEmpty(emp.FirstName)
            && !string.IsNullOrEmpty(emp.Email)
            && emp.Email.Contains("@");
    }
}
```

### 8.4 Validaciones Realizadas

```csharp
// Validaciones por campo
var validations = new Dictionary<string, Func<Employee, bool>>
{
    ["Email"] = e => IsValidEmail(e.Email),
    ["FirstName"] = e => !string.IsNullOrEmpty(e.FirstName) && e.FirstName.Length <= 100,
    ["LastName"] = e => !string.IsNullOrEmpty(e.LastName) && e.LastName.Length <= 100,
    ["Salary"] = e => e.Salary >= 0,
    ["HireDate"] = e => e.HireDate <= DateTime.Now,
    ["Department"] = e => e.DepartmentId > 0
};
```

### 8.5 Estructura Esperada del Archivo Excel

| Columna | Encabezado | Tipo | Requerido |
|---------|-----------|------|----------|
| 1 | FirstName | String | Sí |
| 2 | LastName | String | Sí |
| 3 | Email | String | Sí |
| 4 | JobTitle | String | Sí |
| 5 | Phone | String | No |
| 6 | Address | String | No |
| 7 | Salary | Decimal | Sí |
| 8 | DepartmentId | Int | Sí |

---

## 9. PROYECTO DATA

### 9.1 Propósito

El proyecto `TalentoPlus.Data` es una librería de clases (.NET Class Library) que centraliza:
- **Definición de entidades** (modelos de dominio)
- **Configuración de contexto** (Entity Framework DbContext)
- **Migraciones** de base de datos
- **Repositorios** para acceso a datos

### 9.2 Configuración del DbContext

```csharp
public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relaciones
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId);

        // Índices y constraints
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Name)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Email)
            .HasMaxLength(150);
    }
}
```

### 9.3 Gestión de Conexión

#### **Connection String**
```csharp
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=TalentoDB;Username=postgres;Password=${DB_PASSWORD}"
  }
}

// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.MigrationsAssembly("TalentoPlus.Web")
    )
);
```

### 9.4 Migraciones con Entity Framework Core

#### **Crear nueva migración**
```bash
dotnet ef migrations add NombreMigracion --project TalentoPlus.Data
```

#### **Aplicar migraciones**
```bash
dotnet ef database update --project TalentoPlus.Web
```

#### **Archivo de migración generado**
```csharp
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Departments",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", 
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Departments", x => x.Id);
            });

        // ... más tablas
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Departments");
    }
}
```

### 9.5 Implementación de Repositorios

```csharp
public interface IEmployeeRepository
{
    Task<Employee> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
}

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public async Task<Employee> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }
}
```

---

## 10. PROYECTO WEB

### 10.1 Propósito

`TalentoPlus.Web` es una aplicación ASP.NET Core MVC que proporciona la interfaz de usuario para:
- Gestión de empleados (CRUD)
- Visualización de datos
- Generación de reportes y CVs
- Interacción con el asistente IA
- Importación de datos desde Excel

### 10.2 Tecnologías del Frontend

| Tecnología | Uso |
|-----------|-----|
| **Razor** | Plantillas HTML dinámicas |
| **Bootstrap 5** | Framework CSS responsivo |
| **JavaScript/jQuery** | Interactividad |
| **Chart.js** | Gráficos y estadísticas |

### 10.3 Estructura de Vistas

```
Views/
├── Account/
│   ├── Login.cshtml
│   └── Register.cshtml
├── Dashboard/
│   └── Index.cshtml (AI Assistant)
├── Employee/
│   ├── Index.cshtml (Listado)
│   ├── Create.cshtml
│   ├── Edit.cshtml
│   └── Details.cshtml
└── Excel/
    └── Import.cshtml
```

### 10.4 Comunicación con API

#### **Llamadas AJAX a Endpoints Protegidos**

```javascript
// Solicitud a endpoint protegido con JWT
async function askAI(question) {
    const response = await fetch('/Dashboard/AskAI', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify({ question })
    });

    const data = await response.json();
    displayAnswer(data.answer);
}
```

### 10.5 Manejo de Autenticación

```csharp
// Login
[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    var user = await _userManager.FindByEmailAsync(model.Email);
    var result = await _signInManager.PasswordSignInAsync(
        user,
        model.Password,
        isPersistent: false,
        lockoutOnFailure: true
    );

    if (result.Succeeded)
    {
        return RedirectToAction("Index", "Dashboard");
    }

    return View(model);
}
```

---

## 11. PRUEBAS UNITARIAS E INTEGRACIÓN

### 11.1 Framework: xUnit

- **Framework**: xUnit 2.4.2
- **Mocking**: Moq 4.20.72
- **Test Server**: Microsoft.AspNetCore.Mvc.Testing

### 11.2 Diferencia entre Pruebas Unitarias e Integración

| Aspecto | Unitarias | Integración |
|--------|-----------|------------|
| **Alcance** | Una clase/método | Flujo completo |
| **BD** | Mock/In-Memory | Real o In-Memory |
| **Velocidad** | Muy rápida | Más lenta |
| **Aislamiento** | Completo | Con dependencias |
| **Propósito** | Lógica individual | Comportamiento del sistema |

### 11.3 Pruebas Unitarias

#### **Ejemplo: Prueba de Entidad Employee**

```csharp
[Fact]
public void FullName_ReturnsConcatenatedName()
{
    // Arrange
    var emp = new Employee
    {
        FirstName = "John",
        LastName = "Doe"
    };

    // Act
    var fullName = emp.FullName;

    // Assert
    Assert.Equal("John Doe", fullName);
}
```

#### **Ejemplo: Prueba de Servicio**

```csharp
[Fact]
public void GenerateCvPdf_ReturnsBytes_WhenEmployeeIsValid()
{
    // Arrange
    var service = new CvService();
    var employee = CreateTestEmployee();

    // Act
    var result = service.GenerateCvPdf(employee);

    // Assert
    Assert.NotNull(result);
    Assert.True(result.Length > 0);
}
```

### 11.4 Pruebas de Integración

#### **Configuración con WebApplicationFactory**

```csharp
public class CustomWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Reemplazar BD con In-Memory
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        });
    }
}
```

#### **Ejemplo: Test de Endpoint**

```csharp
public class DepartmentsIntegrationTests 
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    [Fact]
    public async Task Get_Departments_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/departamentos");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }
}
```

### 11.5 Ejecución de Pruebas

```bash
# Ejecutar todas las pruebas
dotnet test TalentoPlus.sln

# Ejecutar proyecto específico
dotnet test ./TalentoPlus.Tests/TalentoPlus.Tests.csproj -v normal

# Con cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

---

## 12. CONCLUSIONES TÉCNICAS

### 12.1 Problemas Resueltos

✅ **Gestión Centralizada de RRHH**: Sistema único para todos los datos de empleados
✅ **Automatización de Reportes**: Generación automática de CVs en PDF profesional
✅ **Análisis Inteligente**: Integración de IA para responder preguntas sobre datos
✅ **Importación Masiva**: Carga rápida de datos desde Excel
✅ **Seguridad**: Autenticación JWT y autorización basada en roles
✅ **Escalabilidad**: Arquitectura modular y desacoplada

### 12.2 Buenas Prácticas Aplicadas

| Práctica | Implementación |
|----------|----------------|
| **Separation of Concerns** | Servicios, Repositorios, Controladores |
| **Dependency Injection** | Inyección de dependencias en Program.cs |
| **Async/Await** | Operaciones no-bloqueantes |
| **Entity Framework** | ORM para acceso a datos |
| **JWT Tokens** | Autenticación sin estado |
| **Validación de Entrada** | ModelState y Data Annotations |
| **Logging** | Microsoft.Extensions.Logging |
| **Configuración Segura** | Variables de entorno, no hardcoding |
| **Tests Automatizados** | xUnit + Moq + WebApplicationFactory |
| **Versionamiento** | Git + GitHub |

### 12.3 Patrones Utilizados

- **Repository Pattern**: Abstracción del acceso a datos
- **Service Layer Pattern**: Lógica de negocio centralizada
- **Factory Pattern**: WebApplicationFactory para tests
- **Dependency Injection**: Contenedor DI nativo de .NET
- **Unit of Work**: Entity Framework DbContext

### 12.4 Mejoras Futuras

#### **Corto Plazo**
- [ ] Agregar caching con Redis
- [ ] Implementar rate limiting en API
- [ ] Mejorar cobertura de tests (>80%)
- [ ] Agregar logging centralizado (Serilog)

#### **Mediano Plazo**
- [ ] Implementar búsqueda full-text (PostgreSQL FTS)
- [ ] Agregar notificaciones en tiempo real (SignalR)
- [ ] Implementar reportes avanzados (Power BI)
- [ ] Agregar soporte multiidioma i18n

#### **Largo Plazo**
- [ ] Migrar a microservicios (si escala)
- [ ] Implementar CQRS para lectura/escritura separadas
- [ ] Agregar machine learning para análisis predictivo
- [ ] Implementar blockchain para auditoría

### 12.5 Seguridad

**Medidas Implementadas:**
- ✅ Contraseñas hasheadas con Identity
- ✅ JWT con expiración
- ✅ HTTPS en producción
- ✅ Validación de entrada
- ✅ API Keys en variables de entorno
- ✅ CORS configurado
- ✅ SQL Injection prevenido (Entity Framework)

**Medidas Recomendadas:**
- 🔒 Implementar 2FA (Two-Factor Authentication)
- 🔒 Agregar detección de fraude
- 🔒 Realizar auditoría de seguridad regular
- 🔒 Implementar WAF (Web Application Firewall)

### 12.6 Rendimiento

**Optimizaciones Implementadas:**
- ✅ Queries optimizadas con `AsNoTracking()`
- ✅ Eager Loading con `Include()`
- ✅ Índices en BD
- ✅ Operaciones async/await

**Recomendaciones:**
- 🚀 Implementar caché de nivel aplicación
- 🚀 Usar Redis para sesiones
- 🚀 Comprimir respuestas (GZIP)
- 🚀 Usar CDN para assets estáticos

---

## 13. DIAGRAMAS Y VISUALIZACIONES

### 13.1 Diagrama de Arquitectura General

```plaintext
┌─────────────────────────────────────────────┐
│         CAPA DE PRESENTACIÓN                │
│  (Web Controllers, Views, API Endpoints)    │
├─────────────────────────────────────────────┤
│         CAPA DE APLICACIÓN                  │
│  (Services, DTOs, Middleware, Mappers)     │
├─────────────────────────────────────────────┤
│         CAPA DE DOMINIO                     │
│  (Entidades, Interfaces, Reglas Negocio)   │
├─────────────────────────────────────────────┤
│         CAPA DE INFRAESTRUCTURA              │
│  (DbContext, Repositorios, EF Core)        │
├─────────────────────────────────────────────┤
│         CAPA DE DATOS                       │
│  (PostgreSQL)                              │
└─────────────────────────────────────────────┘
```

### 13.2 Flujo de Generación de CV en PDF

```plaintext
Usuario en Web
    ↓
[Hacer clic en botón "Descargar CV"]
    ↓
GET /api/empleados/{id}/cv
    ↓
[EmployeeController obtiene datos del empleado]
    ↓
[Inyecta dependencia ICvService]
    ↓
CvService.GenerateCvPdf(employee)
    ↓
[QuestPDF construye documento]
    ↓
[Retorna array de bytes (PDF)]
    ↓
[Controller retorna FileResult]
    ↓
[Navegador descarga archivo o abre en nueva pestaña]
```

### 13.3 Flujo de Integración con Gemini AI

```plaintext
Usuario escriba pregunta
    ↓
DashboardController.AskAI()
    ↓
GeminiService.AskGeminiAsync(question)
    ↓
[Cargar empleados desde BD]
    ↓
[Construir prompt con contexto + pregunta]
    ↓
[HTTP POST a Google Gemini API]
    {
        "contents": [{
            "parts": [{
                "text": "CONTEXTO\n{datos empleados}\n\nPREGUNTA\n{pregunta usuario}"
            }]
        }]
    }
    ↓
[Google Gemini procesa y retorna respuesta]
    ↓
[Parsear JSON de respuesta]
    ↓
[Retornar texto al usuario]
```

### 13.4 Diagrama de Componentes

```plaintext
┌──────────────────────────────┐
│        Usuario Final         │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│   TalentoPlus.Web (MVC)      │
│ - Controladores              │
│ - Vistas (Razor)             │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│   TalentoPlus.Api (REST)     │
│ - Endpoints                  │
│ - Autenticación JWT          │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│   TalentoPlus.Data           │
│ - DbContext                  │
│ - Repositorios               │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│ PostgreSQL (Base de Datos)   │
└──────────────────────────────┘
```

---

## 14. EXPLICACIÓN DETALLADA DEL DESARROLLO

### 14.1 Configuración Inicial del Proyecto

#### **Creación de la Solución y Proyectos**
1. Se utilizó el comando `dotnet new sln` para crear la solución principal `TalentoPlus.sln`.
2. Se añadieron tres proyectos principales:
   - `TalentoPlus.Web`: Aplicación web MVC.
   - `TalentoPlus.Api`: API RESTful.
   - `TalentoPlus.Data`: Biblioteca de clases para la capa de datos.
3. Se configuró la estructura de carpetas para mantener una separación clara entre controladores, servicios, repositorios y modelos.

#### **Configuración de Entity Framework Core**
1. Se instaló el paquete `Microsoft.EntityFrameworkCore` junto con el proveedor `Npgsql` para PostgreSQL.
2. Se creó el archivo `AppDbContext` en el proyecto `TalentoPlus.Data` para manejar las entidades y las relaciones.
3. Se configuraron las migraciones en el proyecto `TalentoPlus.Web` para que EF Core pudiera generar las tablas en la base de datos.
4. Se utilizó el comando `dotnet ef migrations add InitialCreate` para crear la primera migración y `dotnet ef database update` para aplicar los cambios.

#### **Configuración de Autenticación y JWT**
1. Se instaló el paquete `Microsoft.AspNetCore.Authentication.JwtBearer` para manejar la autenticación basada en tokens.
2. En `Program.cs`, se configuraron los parámetros de validación de tokens, como el emisor, el público y la clave de firma.
3. Se creó un servicio `AuthService` para generar tokens JWT utilizando `JwtSecurityTokenHandler`.

### 14.2 Desarrollo de la API REST

#### **Diseño de Endpoints**
1. Se diseñaron los controladores en `TalentoPlus.Api/Controllers` siguiendo el principio RESTful:
   - `GET /api/empleados`: Retorna todos los empleados.
   - `POST /api/auth/login`: Genera un token JWT para autenticación.
   - `GET /api/departamentos`: Lista los departamentos disponibles.
2. Se utilizaron DTOs (Data Transfer Objects) para transferir datos entre el cliente y el servidor, asegurando que solo se expongan los campos necesarios.

#### **Manejo de Errores**
1. Se implementó un middleware global para capturar excepciones y retornar respuestas JSON con códigos de error apropiados.
2. Ejemplo de manejo de errores:
   ```csharp
   public class ExceptionHandlingMiddleware
   {
       public async Task InvokeAsync(HttpContext context)
       {
           try
           {
               await _next(context);
           }
           catch (Exception ex)
           {
               context.Response.StatusCode = StatusCodes.Status500InternalServerError;
               await context.Response.WriteAsJsonAsync(new { message = ex.Message });
           }
       }
   }
   ```

### 14.3 Integración con Google Gemini AI

#### **Configuración del Servicio**
1. Se creó el servicio `GeminiService` en `TalentoPlus.Web/Services` para manejar las llamadas a la API de Google Generative Language.
2. Se utilizó `HttpClient` para realizar solicitudes HTTP POST al endpoint de Gemini.
3. Se configuraron las claves API en el archivo `.env` para evitar exponerlas en el código fuente.

#### **Construcción del Prompt**
1. Se diseñó un método para construir dinámicamente el prompt que se envía a la IA, incluyendo datos contextuales de los empleados.
2. Ejemplo de prompt:
   ```plaintext
   CONTEXTO:
   - Empleado: Juan Pérez, Departamento: IT, Salario: $5000
   - Empleado: Ana Gómez, Departamento: HR, Salario: $4500

   PREGUNTA:
   ¿Cuál es el salario promedio?
   ```

### 14.4 Generación de CV en PDF

#### **Uso de QuestPDF**
1. Se instaló la librería `QuestPDF` para generar documentos PDF con diseño profesional.
2. Se creó el servicio `CvService` que utiliza un diseño fluido para estructurar el contenido del CV.
3. Se definieron secciones claras en el PDF: Información Personal, Contacto, Experiencia Laboral, Educación y Perfil Profesional.

#### **Flujo de Generación**
1. El usuario solicita el CV desde la interfaz web.
2. El controlador `EmployeeController` obtiene los datos del empleado desde la base de datos.
3. Se llama al método `GenerateCvPdf` del servicio `CvService`, que retorna un array de bytes.
4. El controlador retorna el archivo PDF al navegador del usuario.

### 14.5 Importación de Datos desde Excel

#### **Uso de EPPlus**
1. Se instaló la librería `EPPlus` para leer y procesar archivos Excel.
2. Se creó el servicio `ExcelService` para manejar la lógica de importación.
3. Se validaron los datos de cada fila antes de insertarlos en la base de datos.

#### **Validaciones Implementadas**
1. Se verificó que los campos requeridos (nombre, email, salario) no estuvieran vacíos.
2. Se validó el formato del correo electrónico y que el salario fuera un número positivo.
3. Se registraron los errores en un resumen para informar al usuario.

### 14.6 Pruebas Automatizadas

#### **Pruebas Unitarias**
1. Se utilizó `xUnit` para escribir pruebas unitarias de los servicios y entidades.
2. Ejemplo: Prueba del método `FullName` en la entidad `Employee`:
   ```csharp
   [Fact]
   public void FullName_ReturnsConcatenatedName()
   {
       var emp = new Employee { FirstName = "John", LastName = "Doe" };
       Assert.Equal("John Doe", emp.FullName);
   }
   ```

#### **Pruebas de Integración**
1. Se configuró `WebApplicationFactory` para realizar pruebas de integración con una base de datos en memoria.
2. Ejemplo: Prueba del endpoint `/api/departamentos`:
   ```csharp
   [Fact]
   public async Task Get_Departments_ReturnsOk()
   {
       var client = _factory.CreateClient();
       var response = await client.GetAsync("/api/departamentos");
       response.EnsureSuccessStatusCode();
   }
   ```

---

## 14.7 Diseño de Capas y Principios de Clean Architecture

#### **¿Qué es Clean Architecture?**
Clean Architecture es un enfoque de diseño de software que busca crear sistemas altamente mantenibles, escalables y fáciles de probar. Este patrón organiza el código en capas bien definidas, separando las preocupaciones y asegurando que los detalles de implementación (como bases de datos o frameworks) estén desacoplados de la lógica de negocio.

#### **Capas Implementadas en TalentoPlus**
En TalentoPlus, se adotó una arquitectura por capas inspirada en los principios de Clean Architecture. Cada capa tiene una responsabilidad específica y está diseñada para ser independiente de las demás:

1. **Capa de Presentación (UI)**
   - **Responsabilidad**: Manejar las interacciones con el usuario (vistas web o endpoints API).
   - **Ubicación**: `TalentoPlus.Web/Controllers` y `TalentoPlus.Api/Controllers`.
   - **Ejemplo**: El controlador `DashboardController` recibe solicitudes del usuario y delega la lógica al servicio correspondiente.

2. **Capa de Aplicación (Servicios)**
   - **Responsabilidad**: Contiene la lógica de negocio y orquesta las operaciones entre la capa de presentación y la capa de dominio.
   - **Ubicación**: `TalentoPlus.Web/Services` y `TalentoPlus.Api/Services`.
   - **Ejemplo**: El servicio `GeminiService` encapsula la lógica para interactuar con la API de Google Gemini.

3. **Capa de Dominio (Entidades)**
   - **Responsabilidad**: Define las reglas de negocio y las entidades principales del sistema.
   - **Ubicación**: `TalentoPlus.Data/Entities`.
   - **Ejemplo**: La entidad `Employee` contiene propiedades como `FirstName`, `LastName` y métodos como `FullName`.

4. **Capa de Infraestructura (Acceso a Datos)**
   - **Responsabilidad**: Maneja la persistencia de datos y las interacciones con la base de datos.
   - **Ubicación**: `TalentoPlus.Data/Repositories`.
   - **Ejemplo**: El repositorio `EmployeeRepository` implementa métodos para consultar y modificar empleados en la base de datos.

5. **Capa de Datos (Base de Datos)**
   - **Responsabilidad**: Almacena los datos persistentes del sistema.
   - **Tecnología**: PostgreSQL.
   - **Ejemplo**: Las tablas `Employees` y `Departments` almacenan la información de los empleados y departamentos.

#### **Principios de Clean Architecture Aplicados**

1. **Independencia de Frameworks**
   - El sistema no depende de ningún framework específico. Por ejemplo, aunque se utiliza Entity Framework Core para el acceso a datos, la lógica de negocio no está acoplada a este.

2. **Independencia de la UI**
   - La lógica de negocio no depende de la interfaz de usuario. Esto permite cambiar la UI (por ejemplo, de MVC a una API REST) sin afectar las capas internas.

3. **Independencia de la Base de Datos**
   - La lógica de negocio no está acoplada a PostgreSQL. Si se quisiera cambiar a otro motor de base de datos, como MySQL, los cambios serían mínimos y estarían limitados a la capa de infraestructura.

4. **Reglas de Negocio en el Centro**
   - Las reglas de negocio están encapsuladas en la capa de dominio, asegurando que sean independientes de los detalles de implementación.

#### **Ventajas de Este Diseño**
- **Mantenibilidad**: Las capas desacopladas facilitan la modificación y el mantenimiento del sistema.
- **Escalabilidad**: Es sencillo agregar nuevas funcionalidades sin afectar las existentes.
- **Testabilidad**: Las capas independientes permiten realizar pruebas unitarias e integración de manera aislada.
- **Flexibilidad**: Cambiar tecnologías (como el motor de base de datos o el framework de UI) no afecta la lógica de negocio.

#### **Ejemplo Práctico: Flujo de Solicitud de CV**
1. **Capa de Presentación**: El usuario solicita un CV desde la interfaz web.
2. **Capa de Aplicación**: El controlador delega la solicitud al servicio `CvService`.
3. **Capa de Dominio**: El servicio utiliza la entidad `Employee` para obtener los datos necesarios.
4. **Capa de Infraestructura**: El repositorio `EmployeeRepository` consulta la base de datos para obtener la información del empleado.
5. **Capa de Datos**: PostgreSQL almacena los datos del empleado.

Este flujo demuestra cómo cada capa tiene una responsabilidad clara y cómo trabajan juntas para cumplir con los requisitos del sistema.

